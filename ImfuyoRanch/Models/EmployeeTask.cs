using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class EmployeeTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeTaskId { get; set; }
        public string EmployeeId { get; set; }
        public virtual Employee employee { get; set; }
        public int TaskId { get; set; }
        public virtual Tasks tasks { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }




    }
}