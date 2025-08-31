using Propelo.Data;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class PromoterRepository : IPromoterRepository
    {
        private readonly ApplicationDBContext _context;

        public PromoterRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public ICollection<Promoter> GetPromoters()
        {
            return _context.Promoters.OrderBy(p => p.Id).ToList();
        }
        public bool CreatePromoter(Promoter promoter)
        {
            _context.Add(promoter);
            return Save();
        }

        public bool PromoterExists(int promoterId)
        {
            return _context.Promoters.Any(p => p.Id == promoterId);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdatePromoter(Promoter promoter)
        {
            _context.Update(promoter);
            return Save();
        }

        public Promoter GetPromoter(int promoterId)
        {
            return _context.Promoters.Where(p => p.Id == promoterId).FirstOrDefault();
        }

        public bool DeletePromoter(Promoter promoter)
        {
            _context.Remove(promoter);
            return Save();
        }
    }
}
