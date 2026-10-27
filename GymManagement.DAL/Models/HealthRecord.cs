using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; } //cm
        public decimal Weight { get; set; } //Kg
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }

        //LastUpdated = UpdatedAt of BaseEntity
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
