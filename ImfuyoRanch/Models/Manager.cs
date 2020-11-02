using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class Manager
    {
        [Key]
        public string ManagerId { get; set; }
        [DisplayName("Name")]
        public string ManagerName { get; set; }
        [DisplayName("Surname")]
        public string ManagerSurname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNmber { get; set; }
        public string Address { get; set; }
    }
}