using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class ActivateManager
    {
        [Key]
        public int ActiveId { get; set; }
        [DisplayName("Manager Email")]
        public string ManagerEmail { get; set; }
        [DisplayName("Date Activated"),DataType(DataType.Date)]
        public DateTime DateActivated { get; set; }

        [DisplayName("Expiry Time"),DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [DisplayName("Otp Code")]
        public string OtpCode { get; set; }

        public string Status { get; set; }
    }
}