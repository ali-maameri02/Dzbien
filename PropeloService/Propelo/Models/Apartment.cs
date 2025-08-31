namespace Propelo.Models
{
    public class Apartment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public type? Type { get; set; }
        public int? Floor { get; set; }
        public double? Surface { get; set; }
        public string? Description { get; set; }
        public bool? Sold { get; set; }

        //garage and terrain 

        //many to one
        public int? PropertyId { get; set; }
        public Property? Property { get; set; }

        //one to many
        public ICollection<Area> Areas { get; set; } 
        public ICollection<ApartmentDocument> ApartmentDocuments { get; set; } 
        public ICollection<ApartmentPicture> ApartmentPictures { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
