using ImfuyoRanch.Models;
using ImfuyoRanch.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Configuration;
using System.Security.Policy;

namespace ImfuyoRanch.ImfuyoRanchLogic
{
    public class ImfuyoLogic
    {
        private static ApplicationDbContext _db = new ApplicationDbContext();
        public static List<EmployeeTaskVM> _employeeTaskVMs = new List<EmployeeTaskVM>();

        //List<OrderDetails> orderDetails = new List<OrderDetails>(); SSSS

        public ImfuyoLogic()
        {
            _db = new ApplicationDbContext();
            _employeeTaskVMs = new List<EmployeeTaskVM>();
        }

        public static void CreateEmployeeTaskVm()
        {
            _db = new ApplicationDbContext();
            _employeeTaskVMs = new List<EmployeeTaskVM>();
            var employeTasks = (from empTask in _db.EmployeeTasks
                                join employee in _db.Employees on
                                empTask.EmployeeId equals employee.EmployeeId
                                join task in _db.Tasks
                                on empTask.TaskId equals task.TaskId
                                select new
                                {
                                    employee.EmployeeId,
                                    empTask.DueDate,
                                    task.TaskId,
                                    task.Description,
                                    task.Status,
                                    employee.EmployeeName,
                                    employee.EmployeeEmail
                                }
                               ).ToList().Distinct();

            var employeeTask = new EmployeeTaskVM();
            foreach (var item in employeTasks)
            {
                employeeTask.EmployeeId = item.EmployeeId;
                employeeTask.TaskId = item.TaskId;
                employeeTask.EmployeeName = item.EmployeeName;
                employeeTask.EmployeeEmail = item.EmployeeEmail;
                employeeTask.TaskDescription = item.Description;
                employeeTask.Status = item.Status;
                employeeTask.DueDate = item.DueDate;
                _employeeTaskVMs.Add(employeeTask);
            }
        }


        public static bool ChecDate(Tasks task)
        {
            bool result = false;
            if (task.DueDate < DateTime.Now.Date)
            {
                result = true;
            }
            return result;
        }


        //public static string GetSupplierEmail(int? orderId)
        //{
        //    var itemId = (from orderItem in _db.OrderItems
        //                  where orderItem.Order_ID == orderId
        //                  select orderItem.Item.Supplier.SupplierEmail).FirstOrDefault();
        //    return itemId;
        //}
        //public static Item GetItemDetails(int? orderId)
        //{
        //    var itemId = (from orderItem in _db.OrderItems
        //                  where orderItem.Order_ID == orderId
        //                  select orderItem.Item).FirstOrDefault();
        //    return itemId;
        //}


        public static string GetItemName(int? id)
        {
            var itemName = (from orderItem in _db.Items
                            where orderItem.ItemCode == id
                            select orderItem.Name).FirstOrDefault();
            return itemName;


        }



        //public static string GetManagerEmail(string email)
        //{
        //    var managerEmail = (from orItem in _db.OrderDetails
        //                        where orItem.ManagerEmail == email
        //                        select orItem.ManagerEmail).FirstOrDefault();
        //    return managerEmail;
        //}


        //public static int GetOrderId(int? id)
        //{
        //    var orderId = (from custOrder in _db.CustomerOrder
        //                   where custOrder.OrderId == id
        //                   select custOrder.OrderNumber).FirstOrDefault();
        //    return int.Parse(orderId);
        //}


        public static string GenerateOtp(string managerEmail)
        {
            Random ran = new Random();
            int num = ran.Next(1, 101);
            string firstTwo = managerEmail.Substring(0, 2);
            string OTPCodeGenerated = "";
            OTPCodeGenerated = (firstTwo).ToUpper() + "-" + (num.ToString());
            return OTPCodeGenerated;
        }


        public static string GetNameFromEmail(string email)
        {
            string Name = email.Substring(0, email.LastIndexOf("@"));
            return Name;
        }

        public static void ActivateEmial(ActivateManager activateManager)
        {
            //var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = User.Id, code = code }, protocol: Request.Url.Scheme);
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(activateManager.ManagerEmail, activateManager.ManagerEmail));
            var body = $"Hello {GetNameFromEmail(activateManager.ManagerEmail)}, Imfuyo Ranch has activated you as a manager to register. <br/><br/>Your One Time Pin is :<br/><br/>" +
                "OTP  : " + activateManager.OtpCode +
                "<br/><br/> " +
                "Please note that the OTP Code is valid for a 24 hour period after that it will expire";

            ImfuyoRanch.Models.EmailService emailService = new ImfuyoRanch.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Activation OTP",
                mailBody = body,
                mailFooter = "<br/> Kind Regards, <br/> <b>ImfuyoRanch</b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()
            });
        }


    }
}