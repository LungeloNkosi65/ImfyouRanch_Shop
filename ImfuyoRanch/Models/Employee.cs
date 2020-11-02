using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Surname")]
        public string EmployeeSurname { get; set; }
        [DisplayName("Email")]
        public string EmployeeEmail { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        [DisplayName("Manager Email")]
        public string ManagerEmail { get; set; }
    }
}