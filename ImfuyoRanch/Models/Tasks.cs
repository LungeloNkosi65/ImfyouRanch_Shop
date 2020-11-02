using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class Tasks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        [DisplayName("Manager Email")]
        public string ManagerId { get; set; }
        public string Description { get; set; }

        [DisplayName("Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [DisplayName("Due Date")]

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

    }
}