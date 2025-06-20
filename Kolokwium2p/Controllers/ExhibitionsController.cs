using System.ComponentModel.DataAnnotations;
using Kolokwium2p.Data;
using Kolokwium2p.Models;
using Kolokwium2p.Services;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2p.Controllers;

using Microsoft.AspNetCore.Mvc;



[Route("api/[controller]")]
[ApiController]
public class ExhibitionsController : ControllerBase
{
    private readonly AppDbContext _context;


    public ExhibitionsController(AppDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> CreateExhibition([FromBody] CreateExhibitionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var gallery = await _context.Galleries.FirstOrDefaultAsync(g => g.Name == request.Gallery);
        if (gallery == null)
        {
            return NotFound(new { Message = $"Gallery with name '{request.Gallery}' not found." });
        }


        var artworkIds = request.Artworks.Select(a => a.ArtworkId).ToList();
        var existingArtworks = await _context.Artworks
            .Where(a => artworkIds.Contains(a.ArtworkId))
            .ToListAsync();


        if (existingArtworks.Count != request.Artworks.Count)
        {
            var missingIds = artworkIds.Except(existingArtworks.Select(a => a.ArtworkId));
            return NotFound(new { Message = $"Artworks with IDs {string.Join(", ", missingIds)} not found." });
        }


        var exhibition = new Exhibition
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GalerryId = gallery.GalerryId
        };


        _context.Exhibitions.Add(exhibition);
        await _context.SaveChangesAsync();


        foreach (var artworkRequest in request.Artworks)
        {
            var exhibitionArtwork = new Exhibition_Artwork
            {
                ExhibitionId = exhibition.ExhibitionId,
                ArtworkId = artworkRequest.ArtworkId,
                InsuranceValue = artworkRequest.InsuranceValue
            };
            _context.ExhibitionArtworks.Add(exhibitionArtwork);
        }


        await _context.SaveChangesAsync();

        return Ok();
    }


    public class CreateExhibitionRequest
    {
        [Required] public string Title { get; set; }

        [Required] public string Gallery { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Required] public DateTime EndDate { get; set; }

        [Required] public List<ExhibitionArtworkRequest> Artworks { get; set; }
    }


    public class ExhibitionArtworkRequest
    {
        [Required] public int ArtworkId { get; set; }

        [Required] public decimal InsuranceValue { get; set; }
    }
}