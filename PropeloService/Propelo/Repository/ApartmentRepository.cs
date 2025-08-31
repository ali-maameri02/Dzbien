using Propelo.Data;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDBContext _context;

        public ApartmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool ApartmentExists(int apartmentId)
        {
            return _context.Apartments.Any(a => a.Id == apartmentId);
        }

        public bool CreateApartment(Apartment apartment)
        {
            _context.Add(apartment);
            return Save();
        }

        public bool DeleteApartment(Apartment apartment)
        {
            _context.Remove(apartment);
            return Save();
        }

        public Apartment GetApartment(int apartmentId)
        {
            return _context.Apartments.Where(a => a.Id == apartmentId).FirstOrDefault();
        }

        public ICollection<ApartmentDocument> GetApartmentDocumentsByApartment(int apartmentId)
        {
            return _context.ApartmentDocuments.Where(d => d.ApartmentId == apartmentId).ToList();
        }

        public ICollection<ApartmentPicture> GetApartmentPicturesByApartment(int apartmentId)
        {
            return _context.ApartmentPictures.Where(a => a.ApartmentId == apartmentId).ToList();
        }

        public ICollection<Apartment> GetApartments()
        {
            return _context.Apartments.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Area> GetAreasByApartment(int apartmentId)
        {
            return _context.Areas.Where(a => a.ApartmentId == apartmentId).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateApartment(Apartment apartment)
        {
            _context.Update(apartment);
            return Save();
        }
    }
}
