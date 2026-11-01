using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        //JoinDate = CreatedAt of BaseEntity
        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MemberShips { get; set; } = default!;

        public ICollection<Booking> MemberSessions { get; set; } = default!;


    }
}
