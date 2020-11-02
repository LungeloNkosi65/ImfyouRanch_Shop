using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImfuyoRanch.Models;
using ImfuyoRanch.ImfuyoRanchLogic;
using Microsoft.AspNet.Identity;

namespace ImfuyoRanch.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Tasks
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            if (User.IsInRole("Manager"))
            {
                return View(db.Tasks.ToList().Where(p => p.ManagerId == userName));
            }
            else
            {
                return View(db.Tasks.ToList());
            }
        }

        //public ActionResult EmployeeTask()
        //{
        //    var userName = User.Identity.GetUserName();
        //    ImfuyoLogic.CreateEmployeeTaskVm();
        //    return View(ImfuyoLogic._employeeTaskVMs.Where(x => x.EmployeeEmail == userName));
        //}

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,ManagerId,Description,DateCreated,DueDate,Status")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.GetUserName();
                tasks.Status = "Unassigned";
                tasks.DateCreated = System.DateTime.Now.Date;
                tasks.ManagerId = userName;
                if (ImfuyoLogic.ChecDate(tasks))
                {
                    ModelState.AddModelError("", "You can not pick a date that has already passed as a due date");
                    return View(tasks);
                }
                else
                {
                    db.Tasks.Add(tasks);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,ManagerId,Description,DateCreated,DueDate,Status")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = db.Tasks.Find(id);
            db.Tasks.Remove(tasks);
            db.SaveChanges();
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
