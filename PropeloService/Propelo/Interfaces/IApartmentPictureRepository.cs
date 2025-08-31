using Propelo.DTO;
using Propelo.Models;

namespace Propelo.Interfaces
{
    //Mabey
    public interface IApartmentPictureRepository
    {
        Task<List<ApartmentPicture>> CreateApartmentPictureAsync(ApartmentPictureDTO apartmentPictureDTO);
        Task<IEnumerable<ApartmentPicture>> GetPicturesAsync();
        Task<IEnumerable<ApartmentPicture>> GetApartmentPicturesByApartmentIdAsync(int apartmentId);
        Task<ApartmentPicture> GetPictureByIdAsync(int id);
        Task<ApartmentPicture> UpdatePictureAsync(ApartmentPictureDTO apartmentPictureDTO);
        Task<ApartmentPicture> DeletePictureAsync(int id);
        Task<bool> SaveAllAsync();

    }
}
