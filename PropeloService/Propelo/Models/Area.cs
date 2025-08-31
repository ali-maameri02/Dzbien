namespace Propelo.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Surface { get; set; }
        public string? Description { get; set; }

        //many to one
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
    }
}
