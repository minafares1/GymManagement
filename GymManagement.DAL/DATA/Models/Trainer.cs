using GymManagement.DAL.DATA.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class Trainer : GymUser
    {
        //HireDate = CreatedAt of BaseEntity
        public Speciality Speciality { get; set; }


        //Sessions relationship
        public ICollection<Session> Sessions { get; set; } = default!;
    }
}
