using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Models.Enums;
using ArtSharingApp.Backend.Service.ServiceInterface;
using AutoMapper;
using UnauthorizedAccessException = ArtSharingApp.Backend.Exceptions.UnauthorizedAccessException;

namespace ArtSharingApp.Backend.Service;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IArtworkRepository _artworkRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    
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
            throw new BadRequestException("An auction is already scheduled for this artwork during the specified time.");
        
        if (await _auctionRepository.HasFutureAuctionScheduledAsync(artworkId, DateTime.UtcNow))
            throw new BadRequestException("An auction is already scheduled for this artwork in the future.");
        
        
        var auction = _mapper.Map<Auction>(request);
        auction.ArtworkId = artworkId;
        await _auctionRepository.AddAsync(auction);
        await _auctionRepository.SaveAsync();
    }

    public async Task MakeAnOfferAsync(int auctionId, int userId, OfferRequestDTO request)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");
        
        if (auction.StartTime > DateTime.UtcNow || auction.EndTime < DateTime.UtcNow)
            throw new BadRequestException("Auction is not active.");
        
        if (request.Amount <= auction.StartingPrice || request.Amount <= await _offerRepository.GetMaxOfferAmountAsync(auctionId))
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

    public async Task<decimal?> GetMaxOfferAsync(int auctionId)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");
        
        return await _offerRepository.GetMaxOfferAmountAsync(auctionId);
    }

    public async Task AcceptOfferAsync(int offerId, int userId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, includes: o => o.Auction);
        if (offer == null)
            throw new NotFoundException("Offer not found.");
        
        var auction = await _auctionRepository.GetByIdAsync(offer.AuctionId, includes: ac => ac.Artwork);
        if (auction.Artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to accept this offer.");
        
        if (offer.Status != OfferStatus.SUBMITTED)
            throw new BadRequestException("Offer cannot be or is already accepted.");
        
        offer.Status = OfferStatus.ACCEPTED;
        _offerRepository.UpdateOfferStatus(offer);
        await _offerRepository.SaveAsync();
    }

    public async Task WithdrawOfferAsync(int offerId, int userId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, includes: o => o.Auction);
        if (offer == null)
            throw new NotFoundException("Offer not found.");
        
        var auction = await _auctionRepository.GetByIdAsync(offer.AuctionId, includes: ac => ac.Artwork);
        if (auction.Artwork.PostedByUserId == userId || offer.UserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to withdraw this offer.");
        
        offer.Status = OfferStatus.WITHDRAWN;
        _offerRepository.UpdateOfferStatus(offer);
        await _offerRepository.SaveAsync();
    }

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

    public async Task UpdateAuctionEndTimeAsync(int auctionId, int userId, AuctionUpdateEndDTO request)
    {
        var auction = await _auctionRepository.GetByIdAsync(auctionId, includes: ac => ac.Artwork);
        if (auction == null)
            throw new NotFoundException("Auction not found.");
        
        if (auction.Artwork.PostedByUserId != userId)
            throw new UnauthorizedAccessException("You are not authorized to update this auction.");
        
        if (request.EndTime <= auction.StartTime)
            throw new BadRequestException("Auction end time must be after the start time.");
        
        auction.EndTime = request.EndTime;
        _auctionRepository.UpdateEndTime(auction);
        await _auctionRepository.SaveAsync();
    }
}