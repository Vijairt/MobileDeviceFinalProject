namespace MobileDeviceFinalProject.Models
{
    public class NutritionResult
    {
        public string FoodName { get; set; } = string.Empty;
        public double Calories { get; set; }
        public double ProteinG { get; set; }
        public double CarbsG { get; set; }
        public double FatG { get; set; }
        public double ServingQty { get; set; }
        public string ServingUnit { get; set; } = string.Empty;

        public string DisplayText =>
            $"{FoodName} — {Calories:F0} kcal | P:{ProteinG:F0}g C:{CarbsG:F0}g F:{FatG:F0}g ({ServingQty} {ServingUnit})";
    }
}
