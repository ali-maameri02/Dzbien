namespace Propelo.Models
{
    public class ApartmentPicture
    {
        public int Id { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public long PictureSize { get; set; }
        //one to many
        public int? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

    }
}
