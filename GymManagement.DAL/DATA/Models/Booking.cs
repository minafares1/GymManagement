using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class Booking : BaseEntity
    {
        //BookingDate = CreatedAt of BaseEntity
        public bool IsAttended { get; set; }

        //Member relationship
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        //Session relationship  
        public Session Session { get; set; } = default!;
        public int SessionId { get; set; }
    }
}
