namespace Propelo.Models
{
    public class Logo
    {
        public int Id { get; set; }
        public string? LogoName { get; set; }
        public string? LogoPath { get; set; }
        public long? LogoSize { get; set; }

        //one to one
        public int? SettingId { get; set; }
        public Setting? Setting { get; set; }
    }
}
