namespace Propelo.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //one to one
        public Logo? Logo { get; set; }
    }
}
