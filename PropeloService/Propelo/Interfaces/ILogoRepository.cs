using Propelo.DTO;
using Propelo.Models;

namespace Propelo.Interfaces
{
    public interface ILogoRepository
    {
        Task<Logo> CreateLogoAsync(LogoDTO logoDTO);
        Task<IEnumerable<Logo>> GetLogosAsync();
        Task<Logo> GetLogoByIdAsync(int id);
        Task<Logo> UpdateLogoAsync(LogoDTO logoDTO, int logoId);
        Task<bool> DeleteLogoAsync(int id);
        Task<bool> SaveAllAsync();
    }
}
