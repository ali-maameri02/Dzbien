using Propelo.Data;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly ApplicationDBContext _context;

        public SettingRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public ICollection<Setting> GetSettings()
        {
            return _context.Settings.OrderBy(s => s.Id).ToList();
        }
        public bool CreateSetting(Setting setting)
        {
            _context.Add(setting);
            return Save();
        }

        public bool Save()
        {
            var save=_context.SaveChanges();
            return save>0? true: false;
        }

        public bool SettingExists(int settingId)
        {
           return _context.Settings.Any(s=>s.Id == settingId);        
        }

        public bool UpdateSetting(Setting setting)
        {
            _context.Update(setting);
            return Save();
        }
    }
}
