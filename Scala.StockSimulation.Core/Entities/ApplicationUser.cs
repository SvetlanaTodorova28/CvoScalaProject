using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scala.StockSimulation.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Deleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsTeacher { get; set; }

        public ICollection<Order> Order { get; set; }
        public ICollection<UserProductState> UserProductState { get; set; }

    }
}
