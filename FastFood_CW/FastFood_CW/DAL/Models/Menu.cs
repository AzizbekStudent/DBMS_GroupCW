namespace FastFood_CW.DAL.Models
{
    // Students ID: 00013836, 00014725, 00014896
    public class Menu
    {
        public int? Meal_ID { get; set; }

        public required string Meal_title { get; set; }

        public required decimal Price { get; set; }

        public string? Size { get; set; }

        public DateTime? TimeToPrepare { get; set; }

        public byte[]? Image { get; set; }

        public bool? IsForVegan { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
