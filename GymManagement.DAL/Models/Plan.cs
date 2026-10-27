namespace GymManagement.DAL.Models
{
    public class Plan : BaseEntity
    {
    
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }



    }
}
