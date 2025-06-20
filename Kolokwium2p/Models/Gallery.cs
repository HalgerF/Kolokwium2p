using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Kolokwium2p.Models;

public class Gallery
{
    [Key]
    public int GalerryId { get; set; } 
    public string Name { get; set; }
    public DateTime EstablishedDate { get; set; }
    
    public ICollection<Exhibition> Exhibitions { get; set; }
    
}

public class Exhibition
{
    [Key]
    public int ExhibitionId { get; set; }
    [ForeignKey(nameof(Gallery))]
    public int GalerryId { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfArtworks { get; set; }
    public Gallery Gallery { get; set; }
    public ICollection<Exhibition_Artwork> ExhibitionsArtworks { get; set; }
    
    
}

public class Exhibition_Artwork
{
    [ForeignKey(nameof(Artwork))]
    public int ArtworkId { get; set; }
    [ForeignKey(nameof(Exhibition))]
    public int ExhibitionId { get; set; }
    public decimal InsuranceValue { get; set; }
    
    public Artwork Artwork { get; set; }
    public Exhibition Exhibition { get; set; }
}

public class Artwork
{
    [Key]
    public int ArtworkId { get; set; }
    [ForeignKey(nameof(Artist))]
    public int ArtistId { get; set; }
    public string Title { get; set; }
    public DateTime YearCreated { get; set; }
    public ICollection<Exhibition_Artwork> ExhibitionArtworks { get; set; }
    
    
    public Artist Artist { get; set; }
}

public class Artist
{
    [Key]
    public int ArtistId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}