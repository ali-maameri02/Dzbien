namespace Propelo.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Date { get; set; }

        public int? ApartmentID { get; set; }
        public Apartment? Apartment { get; set; }
    }
}
