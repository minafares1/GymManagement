using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = default!;


        //Sessions relationship
        public ICollection<Session> sessions { get; set; } = default!;
    }
}
