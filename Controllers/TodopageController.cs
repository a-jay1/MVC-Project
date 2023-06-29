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
            // Logic to retrieve and display the todo list
            return View();
        }
        public ActionResult ViewTodo()
        {
            // Logic to retrieve and display the todo list
            return View();
        }

        public ActionResult InsertTodo()
        {
            // Logic to insert a new item in the todo list
            return View();
        }

        [HttpPost]
        public ActionResult InsertTodo(List model)
        {
            string str = model.lists;

            try
            {
                _credentialsValidator.insert(str);
            }

            catch (Exception e)
            {
                throw new Exception("exception" + e);
            }
            // Invalid login, display error message
            ViewBag.ErrorMessage = "Inserted";
            return View();

        }
    }
}