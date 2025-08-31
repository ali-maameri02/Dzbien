namespace Propelo.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? Date { get; set; }

        public int? ApartmentID { get; set; }
    }
}
