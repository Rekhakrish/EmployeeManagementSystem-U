using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeManagementSystem.Models
{
    [Table("States")]
    public class StateViewModel
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}