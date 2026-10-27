using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public abstract class GymUser : BaseEntity //Members , Trainers
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }

        public Address Address { get; set; } = default!;
        public Gender Gender { get; set; }

    }
}
