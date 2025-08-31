namespace Propelo.Models
{
    public class PropertyPicture
    {
        public int Id { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public long PictureSize { get; set; }

        //one to many
        public int? PropertyId { get; set; }
        public Property? Property { get; set; }

    }
}
