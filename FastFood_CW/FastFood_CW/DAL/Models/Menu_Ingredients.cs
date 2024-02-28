namespace FastFood_CW.DAL.Models
{
    // Students ID: 00013836, 00014725, 00014896
    public class Menu_Ingredients
    {
        // Junction Table
        // Foreign key
        public int? Meal_ID { get; set; }
        // getting object according to foreign key
        public Menu? Meal { get; set; }

        // Foreign key
        public int? Ingredient_ID { get; set; }
        // getting object according to foreign key
        public Ingredients? Ingredient { get; set; }
    }
}
