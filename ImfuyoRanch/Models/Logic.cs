using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ImfuyoRanch.Models
{
    public class Logic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private static readonly ApplicationDbContext _db = new ApplicationDbContext();
        public bool checkdate(EmployeeTask employeeTask)
        {
            bool result = false;

            var empId = (from e in db.EmployeeTasks
                        where employeeTask.EmployeeId == e.EmployeeId
                        select e.EmployeeId).FirstOrDefault();

            var date = (from t in db.EmployeeTasks
                        where employeeTask.TaskId == t.TaskId
                        select t.DueDate).FirstOrDefault();

            var TaskId = (from tsk in db.EmployeeTasks
                          where tsk.TaskId == employeeTask.TaskId
                          select tsk.TaskId).FirstOrDefault();



            var dbRecord = db.EmployeeTasks;
            foreach (var item in dbRecord)
            {
                if (item.EmployeeId == employeeTask.EmployeeId && item.TaskId == employeeTask.TaskId) 
                {
                    if (employeeTask.DueDate<item.DueDate)
                    {
                        result = true;
                    }
                }
            }

            //if (empId == employeeTask.EmployeeId && TaskId == employeeTask.TaskId)
            //{
            //    if (date <= employeeTask.DueDate)
            //    {
            //        result = true;
            //    }
            //    //if(date<)
            //}


                //if (empId == employeeTask.EmployeeId && date <= employeeTask.tasks.DueDate)
                //{
                //    result = true;
                //}
                return result;

        }


        public string getEmployeeId (int? id)
        {
            var employeeId = (from employeeTask in db.EmployeeTasks
                              where employeeTask.EmployeeTaskId == id
                              select employeeTask.EmployeeId).FirstOrDefault();
            return employeeId;
        }

        public void changeStatus(int? id)
        {
            var dbRecord = db.EmployeeTasks.Find(id);
            var task = db.Tasks.Where(x=>x.TaskId==dbRecord.TaskId && x.Status!="Completed").FirstOrDefault();
            task.Status = "Completed";
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }

        public string GetManagerEmail(int? id)
        {
            string empId = getEmployeeId(id);
            var managerEmail = (from m in db.Employees
                                where m.EmployeeId == empId
                                select m.ManagerEmail).FirstOrDefault();
            return managerEmail;
        }


        public static string GetCustomerName(string userName)
        {
            var name = (from cust in _db.Customers
                        where cust.Email == userName
                        select cust.FirstName).FirstOrDefault();
            return name;
        }



    }
}