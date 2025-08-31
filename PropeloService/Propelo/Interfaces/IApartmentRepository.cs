using Propelo.Models;

namespace Propelo.Interfaces
{
    //Done
    public interface IApartmentRepository
    {
        ICollection<Apartment> GetApartments();
        Apartment GetApartment(int apartmentId); 
        ICollection<ApartmentPicture> GetApartmentPicturesByApartment(int apartmentId);
        ICollection<ApartmentDocument> GetApartmentDocumentsByApartment(int apartmentId);
        ICollection<Area> GetAreasByApartment(int apartmentId);
        bool ApartmentExists(int apartmentId);
        bool CreateApartment(Apartment apartment);
        bool UpdateApartment(Apartment apartment);
        bool DeleteApartment(Apartment apartment);
        bool Save();
    }
}
