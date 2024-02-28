namespace FastFood_CW.DAL.Models
{
    // Students ID: 00013836, 00014725, 00014896
    public class Menu
    {
        public int? meal_ID { get; set; }

        public required string meal_title { get; set; }

        public required decimal price { get; set; }

        public string? size { get; set; }

        public DateTime? TimeToPrepare { get; set; }

        public byte[]? Image { get; set; }

        public bool? IsForVegan { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
