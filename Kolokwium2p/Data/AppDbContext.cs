using Kolokwium2p.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2p.Data;
using Kolokwium2p.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class AppDbContext : DbContext
{
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Exhibition_Artwork> ExhibitionArtworks { get; set; }
    public DbSet<Exhibition> Exhibitions { get; set; }
    public DbSet<Gallery> Galleries { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exhibition_Artwork>()
            .HasKey(ea => new { ea.ExhibitionId, ea.ArtworkId });


        modelBuilder.Entity<Exhibition_Artwork>()
            .HasOne(ea => ea.Exhibition)
            .WithMany(e => e.ExhibitionsArtworks)
            .HasForeignKey(ea => ea.ExhibitionId);


        modelBuilder.Entity<Exhibition_Artwork>()
            .HasOne(ea => ea.Artwork)
            .WithMany(a => a.ExhibitionArtworks)
            .HasForeignKey(ea => ea.ArtworkId);

        
        modelBuilder.Entity<Exhibition_Artwork>().ToTable("Exhibition_Artwork");
    }


    public static void SeedData(AppDbContext context)
    {
        if (!context.Artists.Any())
        {
            var artists = new List<Artist>
            {
                new() { FirstName = "Pablo", LastName = "Picasso", BirthDate = new DateTime(1881, 10, 25) },
                new() { FirstName = "Frida", LastName = "Kahlo", BirthDate = new DateTime(1907, 7, 6) }
            };
            context.Artists.AddRange(artists);
            context.SaveChanges();


            var artworks = new List<Artwork>
            {
                new() { Title = "Guernica", YearCreated = DateTime.Today, ArtistId = artists[0].ArtistId },
                new() { Title = "The Two Fridas", YearCreated = DateTime.Today, ArtistId = artists[1].ArtistId }
            };
            context.Artworks.AddRange(artworks);
            context.SaveChanges();


            var gallery = new Gallery
            {
                Name = "Modern Art Space",
                EstablishedDate = new DateTime(2001, 9, 12)
            };
            context.Galleries.Add(gallery);
            context.SaveChanges();


            var exhibition = new Exhibition
            {
                Title = "20th Century Giants",
                StartDate = new DateTime(2024, 5, 1),
                EndDate = new DateTime(2024, 9, 1),
                GalerryId= gallery.GalerryId
            };
            context.Exhibitions.Add(exhibition);
            context.SaveChanges();


            var exhibitionArtworks = new List<Exhibition_Artwork>
            {
                new() { ExhibitionId = exhibition.ExhibitionId, ArtworkId = artworks[0].ArtworkId, InsuranceValue = 1000000.00m },
                new() { ExhibitionId = exhibition.ExhibitionId, ArtworkId = artworks[1].ArtworkId, InsuranceValue = 800000.00m }
            };
            context.ExhibitionArtworks.AddRange(exhibitionArtworks);
            context.SaveChanges();
        }
    }
}