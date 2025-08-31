using Propelo.Models;

namespace Propelo.DTO
{
    public class ApartmentDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public type? Type { get; set; }
        public int?Floor { get; set; }
        public double? Surface { get; set; }
        public string? Description { get; set; }
        public bool? Sold { get; set; }
        public int? PropertyId { get; set; }
    }
}
