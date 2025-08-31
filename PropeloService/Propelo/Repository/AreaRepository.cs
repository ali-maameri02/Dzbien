using Propelo.Data;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ApplicationDBContext _context;

        public AreaRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool AreaExists(int areaId)
        {
            return _context.Areas.Any(a => a.Id == areaId);
        }

        public bool CreateArea(Area area)
        {
            _context.Add(area);
            return Save();
        }

        public bool DeleteArea(Area area)
        {
            _context.Remove(area);
            return Save();
        }

        public Area GetArea(int areaId)
        {
            return _context.Areas.Where(a => a.Id == areaId).FirstOrDefault();
        }

        public ICollection<Area> GetAreas()
        {
            return _context.Areas.OrderBy(a => a.Id).ToList();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateArea(Area area)
        {
            _context.Update(area);
            return Save();
        }
    }
}
