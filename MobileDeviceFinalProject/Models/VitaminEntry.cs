namespace MobileDeviceFinalProject.Models
{
    public class VitaminEntry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;
        public bool IsDaily { get; set; } = true;
    }

    public class VitaminLog
    {
        public int Id { get; set; }
        public int VitaminEntryId { get; set; }
        public DateTime LoggedDate { get; set; } = DateTime.Today;
    }
}
