using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing auctions.
/// </summary>
public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IArtworkRepository _artworkRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionService"/> class.
    /// </summary>
    /// <param name="auctionRepository">Repository for auction data access.</param>
    /// <param name="artworkRepository">Repository for artwork data access.</param>
    /// <param name="offerRepository">Repository for offer data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public AuctionService(
        IAuctionRepository auctionRepository,
        IArtworkRepository artworkRepository,
        IOfferRepository offerRepository,
        IMapper mapper)
    {
        _auctionRepository = auctionRepository;
        _artworkRepository = artworkRepository;
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Starts a new auction for the specified artwork.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <param name="userId">The ID of the user starting the auction (must be the one who posted the artwork).</param>
    /// <param name="request">Auction start data.</param>
    /// <exception cref="NotFoundException">Thrown if the artwork is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized.</exception>
    /// <exception cref="BadRequestException">Thrown if auction parameters are invalid or an auction is already scheduled.</exception>
    public async Task StartAuctionAsync(int artworkId, int userId, AuctionStartDTO request)
    {
        var artwork = await _artworkRepository.GetByIdAsync(artworkId);
        if (artwork == null)
            throw new NotFoundException("Artwork not found.");

        if (artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to start an auction for this artwork.");

        if (request.StartTime < DateTime.UtcNow.AddMinutes(-1))
            throw new BadRequestException("Auction start time cannot be in the past.");

        if (request.EndTime <= request.StartTime)
            throw new BadRequestException("Auction end time must be after the start time.");

        if (await _auctionRepository.IsAuctionScheduledAsync(artworkId, request.StartTime, request.EndTime))
            throw new BadRequestException(
                "An auction is already scheduled for this artwork during the specified time.");

        if (await _auctionRepository.HasFutureAuctionScheduledAsync(artworkId, DateTime.UtcNow))
            throw new BadRequestException("An auction is already scheduled for this artwork in the future.");


        var auction = _mapper.Map<Auction>(request);
        auction.ArtworkId = artworkId;
        await _auctionRepository.AddAsync(auction);
        await _auctionRepository.SaveAsync();
    }

    /// <summary>
    /// Makes an offer on an active auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user making the offer.</param>
    /// <param name="request">Offer data.</param>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="BadRequestException">Thrown if the auction is not active or offer amount is invalid.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is making an offer on their own auction.</exception>
    public async Task MakeAnOfferAsync(int auctionId, int userId, OfferRequestDTO request)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");

        if (!auction.IsActive())
            throw new BadRequestException("Auction is not active.");

        if (request.Amount <= auction.StartingPrice ||
            request.Amount <= await _offerRepository.GetMaxOfferAmountAsync(auctionId))
            throw new BadRequestException("Offer amount must be greater than the maximum offer or starting price.");

        if (auction.Artwork.PostedByUserId == userId)
            throw new UnauthorizedAccessException("You cannot make an offer on your own auction.");

        var offer = _mapper.Map<Offer>(request);
        offer.UserId = userId;
        offer.AuctionId = auctionId;
        offer.Timestamp = DateTime.UtcNow;
        offer.Status = OfferStatus.SUBMITTED;

        await _offerRepository.AddAsync(offer);
        await _offerRepository.SaveAsync();
    }

    /// <summary>
    /// Retrieves all offers for a specific auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user requesting offers (must be the one who posted the artwork).</param>
    /// <returns>A collection of <see cref="OfferResponseDTO"/> for the auction.</returns>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to view offers for this auction.</exception>
    public async Task<IEnumerable<OfferResponseDTO>?> GetOffersAsync(int auctionId, int userId)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");

        if (auction.Artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to view offers for this auction.");

        var offers = await _offerRepository.GetOffersByAuctionIdAsync(auctionId);
        return _mapper.Map<IEnumerable<OfferResponseDTO>>(offers);
    }

    /// <summary>
    /// Gets the maximum offer amount for a specific auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <returns>The maximum offer amount, or null if no offers exist.</returns>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    public async Task<decimal?> GetMaxOfferAsync(int auctionId)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");

        return await _offerRepository.GetMaxOfferAmountAsync(auctionId);
    }

    /// <summary>
    /// Accepts an offer for an auction.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user accepting the offer (must be the one who posted the artwork).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to accept this offer.</exception>
    /// <exception cref="BadRequestException">Thrown if the offer cannot be accepted.</exception>
    public async Task AcceptOfferAsync(int offerId, int userId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, includes: o => o.Auction);
        if (offer == null)
            throw new NotFoundException("Offer not found.");

        var auction = await _auctionRepository.GetByIdAsync(offer.AuctionId, includes: ac => ac.Artwork);
        if (auction.Artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to accept this offer.");

        if (await _offerRepository.AuctionHasAcceptedOffer(auction.Id))
            throw new BadRequestException("Offer is already accepted.");

        if (!offer.CanBeModified())
            throw new BadRequestException("Offer cannot be or is already accepted.");

        offer.Accept();
        _offerRepository.UpdateOfferStatus(offer);
        await _offerRepository.SaveAsync();
    }

    /// <summary>
    /// Rejects an offer for an auction.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user rejecting the offer (must be the one who posted the artwork and not the one who made the offer).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to reject this offer.</exception>
    public async Task RejectOfferAsync(int offerId, int userId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, includes: o => o.Auction);
        if (offer == null)
            throw new NotFoundException("Offer not found.");

        var auction = await _auctionRepository.GetByIdAsync(offer.AuctionId, includes: ac => ac.Artwork);
        if (auction.Artwork.PostedByUserId != userId || offer.UserId == userId)
            throw new UnauthorizedAccessException("You are not authorized to reject this offer.");

        offer.Reject();
        _offerRepository.UpdateOfferStatus(offer);
        await _offerRepository.SaveAsync();
    }

    /// <summary>
    /// Withdraws an offer made by the user.
    /// </summary>
    /// <param name="offerId">The offer ID.</param>
    /// <param name="userId">The ID of the user withdrawing the offer (must be the one who made the offer and not the one who posted the artwork).</param>
    /// <exception cref="NotFoundException">Thrown if the offer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to withdraw this offer.</exception>
    public async Task WithdrawOfferAsync(int offerId, int userId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, includes: o => o.Auction);
        if (offer == null)
            throw new NotFoundException("Offer not found.");

        var auction = await _auctionRepository.GetByIdAsync(offer.AuctionId, includes: ac => ac.Artwork);
        if (auction.Artwork.PostedByUserId == userId || offer.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to withdraw this offer.");

        offer.Withdraw();
        _offerRepository.UpdateOfferStatus(offer);
        await _offerRepository.SaveAsync();
    }

    /// <summary>
    /// Retrieves the active auction for a specific artwork.
    /// </summary>
    /// <param name="artworkId">The artwork ID.</param>
    /// <returns>An <see cref="AuctionResponseDTO"/> for the active auction, or null if none exists.</returns>
    public async Task<AuctionResponseDTO?> GetActiveAuctionAsync(int artworkId)
    {
        var auction = await _auctionRepository.GetActiveAuctionByArtworkIdAsync(artworkId, DateTime.UtcNow);
        if (auction == null)
            return null;

        var maxOffer = await _offerRepository.GetMaxOfferAmountAsync(auction.Id);
        var offerCount = await _offerRepository.GetOfferCountByAuctionIdAsync(auction.Id);

        return new AuctionResponseDTO
        {
            Id = auction.Id,
            StartTime = auction.StartTime,
            EndTime = auction.EndTime,
            Currency = auction.Currency,
            OfferCount = offerCount,
            CurrentPrice = maxOffer == 0 ? auction.StartingPrice : maxOffer
        };
    }

    /// <summary>
    /// Updates the end time of an auction.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <param name="userId">The ID of the user updating the auction (must be the one who posted the artwork).</param>
    /// <param name="request">New end time details.</param>
    /// <exception cref="NotFoundException">Thrown if the auction is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user is not authorized to update this auction.</exception>
    /// <exception cref="BadRequestException">Thrown if the new end time is invalid.</exception>
    public async Task UpdateAuctionEndTimeAsync(int auctionId, int userId, AuctionUpdateEndDTO request)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");

        if (auction.Artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this auction.");

        try
        {
            auction.UpdateEndTime(request.EndTime);
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }

        _auctionRepository.UpdateEndTime(auction);
        await _auctionRepository.SaveAsync();
    }

    /// <summary>
    /// Retrieves high-stakes auctions (with high and many offers).
    /// </summary>
    /// <param name="count">The number of auctions to retrieve.</param>
    /// <returns>A collection of <see cref="HighStakesAuctionDTO"/>.</returns>
    public async Task<IEnumerable<HighStakesAuctionDTO>?> GetHighStakesAuctionsAsync(int count)
    {
        var now = DateTime.UtcNow;
        return await _auctionRepository.GetHighStakesAuctionsAsync(count, now);
    }
}