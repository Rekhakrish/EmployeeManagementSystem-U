using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmployeeManagementSystem.Models
{
    public class EmployeeContext : DbContext
    {
        public DbSet<EmployeeViewModel> Employees { get; set; }
        public DbSet<StateViewModel> allState { get; set; }
    }
}