
using Propelo.Data;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDBContext _context;

        public PropertyRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool CreateProperty(Property property)
        {
            _context.Add(property);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteProperty(Property property)
        {
            _context.Remove(property);
            return Save();
        }

        public ICollection<Property> GetProperties()
        {
            return _context.Properties.OrderBy(a => a.Id).ToList();
        }

        public Property GetProperty(int propertyId)
        {
            return _context.Properties.Where(a => a.Id == propertyId).FirstOrDefault();
        }

        public Property GetPropertyByApartment(int apartmentId)
        {
            return _context.Apartments.Where(a => a.Id == apartmentId).Select(a => a.Property).FirstOrDefault();
        }

        public ICollection<PropertyPicture> GetPropertyPicturesByProperty(int propertyId)
        {
            return _context.PropertyPictures.Where(p=> p.PropertyId ==propertyId).ToList();
        }

        public bool PropertyExists(int propertyId)
        {
            return _context.Properties.Any(a => a.Id == propertyId);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateProperty(Property property)
        {
            _context.Update(property);
            return Save();
        }
    }

}
