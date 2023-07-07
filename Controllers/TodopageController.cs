using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using System.Data.SqlClient;

namespace WebApplication6.Controllers
{
    public class TodopageController : Controller
    {
        private readonly CredentialsValidator _credentialsValidator;

        public TodopageController()
        {
            string connectionString = "Data Source=DESKTOP-3EIFCQR;Initial Catalog=signupDB;Integrated Security=True";
            _credentialsValidator = new CredentialsValidator(connectionString);
        }

        public ActionResult Index()
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            return View();
        }
        public ActionResult ViewTodo()
        {
            // Logic to retrieve and display the todo list
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            try
            {
                List<List<string>> items = _credentialsValidator.getList();
                var model = new TodoModel { Items = items };
                return View(model);
            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }
            
        }
        public ActionResult InsertTodo()
        {
            ViewBag.ErrorMessage = _credentialsValidator.Get();
            return View();
        }

        [HttpPost]
        public ActionResult InsertTodo(List model)
        {
            if (model.lists == null && model.startTime == null && model.endTime == null )
            {
                ViewBag.ErrorMessage = "InsertValues";
                return View(); 
            }

            if (TimeSpan.Parse(model.startTime) >= TimeSpan.Parse(model.endTime))
            {
                ViewBag.ErrorMessage = "Enter Valid Time";
                return View();
            }

            if (!(_credentialsValidator.insertValid(model)))
            {
                ViewBag.ErrorMessage = "Task already sheduled between this time frame !";
                return View();
            }
            try
            {
                _credentialsValidator.insert(model);
            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }
            // Invalid login, display error message
            TempData["ErrorMessage"] = "Inserted";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            try
            {
                _credentialsValidator.Delete(id);
            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }
           
            return RedirectToAction("ViewTodo");

        }

        [HttpPost]
        public ActionResult Completed(int id )
        {
            try
            {
                _credentialsValidator.Completed(id);
            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }


            return RedirectToAction("ViewTodo");

        }

        public ActionResult ViewCompleted()
        {
            // Logic to retrieve and display the todo list
            try
            {
                List<List<string>> items = _credentialsValidator.ViewCompleted();
                var model = new TodoModel { Items = items };
                return View(model);

            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }

        }

        public ActionResult StoreDate(Datestring model)
        {
            if(model.DateString == null)
            {
                TempData["ErrorMessage"] = "The DateString is null.";
                return RedirectToAction("Index");
            }
            _credentialsValidator.Set(model.DateString);

            return RedirectToAction("InsertTodo"); // Redirect to another action or view
        }


        public ActionResult StoreView(Datestring model)
        {
            if (model.DateString == null)
            {
                TempData["ErrorMessage"] = "The DateString is null.";
                return RedirectToAction("Index");
            }
            _credentialsValidator.Set(model.DateString);

            return RedirectToAction("ViewToDo"); // Redirect to another action or view
        }


        [HttpPost]
        public ActionResult Reschedule(int id ,string task , string endTime,TodoModel model)
        {

            int i = _credentialsValidator.Reschedule(id,task,endTime,model);
            if(i == 1)
            {
                TempData["ErrorMessage"] = "rescheduled";
            }
            else if(i == 2)
            {
                TempData["ErrorMessage"] = "Task completed ! cannot be rescheduled";
            }
            else
            {
                TempData["ErrorMessage"] = "No time for Rescheduling";
            }
            return RedirectToAction("ViewTodo");

        }
    }
}