using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    [Owned]
    public class Address
    {
        public string Street { get; set; } = default!;
        public string city { get; set; } = default!;
        public int BuildingNumber { get; set; } 
    }
}
