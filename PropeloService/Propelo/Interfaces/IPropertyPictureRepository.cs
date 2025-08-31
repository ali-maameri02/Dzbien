using Propelo.DTO;
using Propelo.Models;

namespace Propelo.Interfaces
{
    //Maybe
    public interface IPropertyPictureRepository
    {
        Task<List<PropertyPicture>> CreatePropertyPictureAsync(PropertyPictureDTO propertyPictureDTO);
        Task<IEnumerable<PropertyPicture>> GetPropertyPicturesAsync();
        Task<PropertyPicture> GetPropertyPictureByIdAsync(int id);
        Task<IEnumerable<PropertyPicture>> GetPropertyPicturesByPropertyIdAsync(int propertyId);
        Task<PropertyPicture> UpdatePropertyPictureAsync(PropertyPictureDTO propertyPictureDTO);
        Task<PropertyPicture> DeletePropertyPictureAsync(int id);
        Task<bool> SaveAllAsync();
    }
}
