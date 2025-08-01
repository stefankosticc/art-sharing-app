using ArtSharingApp.Backend.Models.Enums;

namespace ArtSharingApp.Backend.Models;

/// <summary>
/// Represents an artwork piece
/// </summary>
public class Artwork
{
    /// <summary>
    /// Unique identifier for the artwork
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the artwork
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Story behind the artwork
    /// </summary>
    public string Story { get; set; }

    /// <summary>
    /// Image of the artwork in byte array format
    /// </summary>
    public byte[] Image { get; set; }

    /// <summary>
    /// Content type of the image
    /// <remarks>
    /// This is used to determine how the image should be displayed in the UI.
    /// </remarks>
    /// <example>
    /// "image/jpeg"
    /// </example>
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Date when the artwork was created
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Techniques or insights used during the creation of the artwork
    /// </summary>
    public string TipsAndTricks { get; set; }

    /// <summary>
    /// Indicates whether the artwork is private or public
    /// <remarks>
    /// If true, the artwork is private and not visible to other users.
    /// If false, the artwork is public and can be viewed by other users.
    /// </remarks>
    /// </summary>
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Indicates whether the artwork is on sale with a fixed price
    /// <remarks>
    /// If true, the artwork is available for purchase.
    /// If false, the artwork is not for sale.
    /// </remarks>
    /// </summary>
    public bool IsOnSale { get; set; }

    /// <summary>
    /// Price of the artwork if it is on sale
    /// <remarks>
    /// Relevant only when <see cref="IsOnSale"/> is true.
    /// </remarks>
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Currency in which the artwork price is represented.
    /// See <see cref="Currency"/>
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Color associated with the artwork
    /// <remarks>
    /// This is used with the color extractor to determine the dominant color of the artwork.
    /// </remarks>
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Identifier for the city where the artwork is located
    /// </summary>
    public int? CityId { get; set; }

    /// <summary>
    /// Navigation property for the city where the artwork is located
    /// </summary>
    public City? City { get; set; }

    /// <summary>
    /// Identifier for the gallery where the artwork is displayed
    /// </summary>
    public int? GalleryId { get; set; }

    /// <summary>
    /// Navigation property for the gallery where the artwork is displayed
    /// </summary>
    public Gallery? Gallery { get; set; }

    /// <summary>
    /// Identifier for the artist who created the artwork
    /// </summary>
    public int CreatedByArtistId { get; set; }

    /// <summary>
    /// Navigation property for the artist who created the artwork
    /// </summary>
    public User CreatedByArtist { get; set; }

    /// <summary>
    /// Identifier for the user who posted the artwork
    /// <remarks>
    /// This may be the same as CreatedByArtistId if the artist is also the one who posted the artwork.
    /// It also allows transferring ownership of the artwork to another user without losing the original artist information.
    /// </remarks>
    /// </summary>
    public int PostedByUserId { get; set; }

    /// <summary>
    /// Navigation property for the user who posted the artwork
    /// </summary>
    public User PostedByUser { get; set; }

    /// <summary>
    /// Collection of likes associated with the artwork
    /// </summary>
    public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();

    /// <summary>
    /// Collection of auctions associated with the artwork
    /// </summary>
    public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
}