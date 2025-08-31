using Propelo.Models;

namespace Propelo.Interfaces
{
    //Mabey
    public interface IAreaRepository
    {
        ICollection<Area> GetAreas();
        Area GetArea(int areaId);
        bool AreaExists(int areaId);
        bool CreateArea(Area area);
        bool UpdateArea(Area area);
        bool DeleteArea(Area area);
        bool Save();
    }
}
