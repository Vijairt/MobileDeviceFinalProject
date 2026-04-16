namespace MobileDeviceFinalProject.Models
{
    public class WorkoutEntry
    {
        public int Id { get; set; }
        public string ExerciseType { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public double? DistanceMiles { get; set; }
        public int? Sets { get; set; }
        public int? Reps { get; set; }
        public string? Notes { get; set; }
        public DateTime LoggedAt { get; set; } = DateTime.Now;
    }
}
