using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ImfuyoRanch.Models;
using Microsoft.AspNet.Identity;

namespace ImfuyoRanch.Controllers
{
    public class EmployeeTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Logic logic = new Logic();
        // GET: EmployeeTasks
        public ActionResult Index()
        {
            var employeeTasks = db.EmployeeTasks.Include(e => e.employee).Include(e => e.tasks);
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Employee"))
            {
                return View(employeeTasks.ToList().Where(x=>x.employee.EmployeeEmail==userName));
            }
            else if (User.IsInRole("Manager"))
            {

                return View(employeeTasks.ToList().Where(x=>x.employee.ManagerEmail==userName));
            }
            else
            {
                return View(employeeTasks.ToList());
            }
           
        }

        // GET: EmployeeTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            if (employeeTask == null)
            {
                return HttpNotFound();
            }
            return View(employeeTask);
        }

        // GET: EmployeeTasks/Create
        public ActionResult Create()
        {
            var userName = User.Identity.GetUserName();

            ViewBag.EmployeeId = new SelectList(db.Employees.Where(p=>p.ManagerEmail == userName), "EmployeeId", "EmployeeName");
            ViewBag.TaskId = new SelectList(db.Tasks.Where(p=>p.ManagerId ==userName && p.Status!="Completed"), "TaskId", "Description");
            return View();
        }

        // POST: EmployeeTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeTaskId,EmployeeId,TaskId")] EmployeeTask employeeTask)
        {
            var userName = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                if (logic.checkdate(employeeTask)==false)
                {
                    var task = db.Tasks.Where(p => p.TaskId == employeeTask.TaskId).FirstOrDefault();
                    var employee = db.Employees.Where(p => p.EmployeeId == employeeTask.EmployeeId).FirstOrDefault();
                    task.Status = "Assigned";
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    employeeTask.DueDate = task.DueDate;
                    db.EmployeeTasks.Add(employeeTask);
                    db.SaveChanges();

                    var mailTo = new List<MailAddress>();
                    mailTo.Add(new MailAddress(employee.EmployeeEmail, employee.EmployeeName));
                    var body = $"Good Day {employee.EmployeeName}, you have been assigned to a {task.Description} task.<br/> The task should be completed by {task.DueDate}";

                    ImfuyoRanch.Models.EmailService emailService = new ImfuyoRanch.Models.EmailService();
                    emailService.SendEmail(new EmailContent()
                    {
                        mailTo = mailTo,
                        mailCc = new List<MailAddress>(),
                        mailSubject = "New task alert!!  | Ref No.:" + employeeTask.EmployeeTaskId,
                        mailBody = body,
                        mailFooter = $"<br/> Kind Regards, <br/> <b>Your Manager {employeeTask.employee.ManagerEmail} </b>",
                        mailPriority = MailPriority.High,
                        mailAttachments = new List<Attachment>()

                    });
                    TempData["AlertMessage"] = $"{employee.EmployeeName} has been assigned";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AlertMessage"] = $"Cannot Assign Employee because he/she is currently busy with anohter task";
                }


            }

            ViewBag.EmployeeId = new SelectList(db.Employees.Where(p => p.ManagerEmail == userName), "EmployeeId", "EmployeeName", employeeTask.EmployeeId);
            ViewBag.TaskId = new SelectList(db.Tasks.Where(p => p.ManagerId == userName && p.Status != "Completed"), "TaskId", "Description", employeeTask.TaskId);
            return View(employeeTask);
        }

        // GET: EmployeeTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            var userName = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            if (employeeTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees.Where(p => p.ManagerEmail == userName), "EmployeeId", "EmployeeName", employeeTask.EmployeeId);
            ViewBag.TaskId = new SelectList(db.Tasks.Where(p => p.ManagerId == userName && p.Status != "Completed"), "TaskId", "Description", employeeTask.TaskId);
            return View(employeeTask);
        }

        // POST: EmployeeTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeTaskId,EmployeeId,TaskId")] EmployeeTask employeeTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", employeeTask.EmployeeId);
            ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Description", employeeTask.TaskId);
            return View(employeeTask);
        }

        // GET: EmployeeTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            if (employeeTask == null)
            {
                return HttpNotFound();
            }
            return View(employeeTask);
        }

        // POST: EmployeeTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeTask employeeTask = db.EmployeeTasks.Find(id);
            db.EmployeeTasks.Remove(employeeTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Done(int? id)
        {
            logic.changeStatus(id);
            var userName = User.Identity.GetUserName();
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(logic.GetManagerEmail(id), "Manager"));
            var body = $"Good Day Manager please note the task has been completed";

            ImfuyoRanch.Models.EmailService emailService = new ImfuyoRanch.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Task Completed!! ",
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b> Your Employee  {userName} </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()
            });
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
