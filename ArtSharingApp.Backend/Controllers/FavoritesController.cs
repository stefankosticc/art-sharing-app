using Microsoft.AspNetCore.Mvc;
using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service;
using System.Collections.Generic;

namespace ArtSharingApp.Backend.Controllers;

[ApiController]
[Route("api")]
public class FavoritesController : Controller
{
    private readonly IFavoritesService _favoritesService;

    public FavoritesController(IFavoritesService favoritesService)
    {
        _favoritesService = favoritesService;
    }
}
