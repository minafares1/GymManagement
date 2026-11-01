using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class MemberShip : BaseEntity
    {
        //public DateTime StartDate { get; set; }   //= CreatedAt of BaseEntity
        public DateTime EndDate { get; set; }
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;


        //Member relationship
        public Member Member { get; set; } = default!;
        public  int MemberId { get; set; }

        //Plan relationship
        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }

    }
}
