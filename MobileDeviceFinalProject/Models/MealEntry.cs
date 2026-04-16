namespace MobileDeviceFinalProject.Models
{
    public class MealEntry
    {
        public int Id { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public double Calories { get; set; }
        public double ProteinG { get; set; }
        public double CarbsG { get; set; }
        public double FatG { get; set; }
        public double ServingQty { get; set; }
        public string ServingUnit { get; set; } = string.Empty;
        public DateTime LoggedAt { get; set; } = DateTime.Now;
        public string? PhotoPath { get; set; }
    }
}
