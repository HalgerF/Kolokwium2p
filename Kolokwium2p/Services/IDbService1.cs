using Kolokwium2p.Models;

namespace Kolokwium2p.Services;

public interface IDbService1
{
    public Task<Exhibition> GetGalleryExhibitions(int exhibitionId);
}