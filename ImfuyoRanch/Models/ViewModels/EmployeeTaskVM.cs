using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models.ViewModels
{
    public class EmployeeTaskVM
    {
        //public int EmployeeTaskVmId { get; set; }
        [Key]
        public string EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Email")]
        public string EmployeeEmail { get; set; }
        public int TaskId { get; set; }
        [DisplayName("Task Description")]
        public string TaskDescription { get; set; }
        [DisplayName("Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
}