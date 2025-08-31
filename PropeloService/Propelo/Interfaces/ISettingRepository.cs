using Propelo.Models;

namespace Propelo.Interfaces
{
    //Done
    public interface ISettingRepository
    {
        public ICollection<Setting> GetSettings();
        bool SettingExists(int settingId);
        bool CreateSetting(Setting setting);
        bool UpdateSetting(Setting setting);
        bool Save();
    }
}
