using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        //Trainer relationship
        public Trainer Trainer { get; set; } = default!;
        public int TrainerId { get; set; }


        //Category relationship
        public Category Category { get; set; } = default!;
        public  int CategoryId { get; set; }


        //Booging relationship
        public ICollection<Booking> SessionMembers { get; set; } = default!;
    }
}
