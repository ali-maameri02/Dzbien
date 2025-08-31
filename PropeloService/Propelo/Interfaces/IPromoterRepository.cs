using Propelo.Models;

namespace Propelo.Interfaces
{
    //Done
    public interface IPromoterRepository
    {
        ICollection<Promoter> GetPromoters();
        Promoter GetPromoter(int promoterId);
        bool PromoterExists(int promoterId);
        bool CreatePromoter(Promoter promoter);
        bool UpdatePromoter(Promoter promoter);
        bool DeletePromoter(Promoter promoter);
        bool Save();
    }
}
