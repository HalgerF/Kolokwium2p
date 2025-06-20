using Kolokwium2p.Data;
using Kolokwium2p.DTOs;

namespace Kolokwium2p.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class GalleriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GalleriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}/exhibitions")]
    public async Task<ActionResult<GalleryExhibitionsResponse>> GetGalleryExhibitions(int id)
    {
        var gallery = await _context.Galleries
            .Include(g => g.Exhibitions)
            .ThenInclude(e => e.ExhibitionsArtworks)
            .ThenInclude(ea => ea.Artwork)
            .ThenInclude(a => a.Artist)
            .FirstOrDefaultAsync(g => g.GalerryId == id);

        if (gallery == null)
        {
            return NotFound();
        }

        var response = new GalleryExhibitionsResponse
        {
            GalleryId = gallery.GalerryId,
            Name = gallery.Name,
            EstablishedDate = gallery.EstablishedDate,
            Exhibitions = gallery.Exhibitions.Select(e => new ExhibitionDto
            {
                Title = e.Title,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                NumberOfArtworks = e.ExhibitionsArtworks.Count,
                Artworks = e.ExhibitionsArtworks.Select(ea => new ArtworkDto
                {
                    Title = ea.Artwork.Title,
                    InsuranceValue = ea.InsuranceValue,
                    Artist = new ArtistDto
                    {
                        FirstName = ea.Artwork.Artist.FirstName,
                        LastName = ea.Artwork.Artist.LastName,
                        BirthDate = ea.Artwork.Artist.BirthDate
                    }
                }).ToList()
            }).ToList()
        };

        return Ok(response);
    }
}
