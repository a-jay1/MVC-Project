﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class UserController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        /*
        [HttpPost]
        public ActionResult Login(User user)
        {
            // Perform authentication logic using the user model
            if (user.Username == "admin" && user.Password == "password")
            {
                // Authentication successful, redirect to home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Authentication failed, display an error message
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }
        */

        private readonly CredentialsValidator _credentialsValidator;

        public UserController()
        {
            string connectionString = "Data Source=DESKTOP-3EIFCQR;Initial Catalog=signupDB;Integrated Security=True";
            _credentialsValidator = new CredentialsValidator(connectionString);
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            string email = model.Username;
            string password = model.Password;

            bool isValid = _credentialsValidator.IsValidCredentials(email, password);

            if (isValid)
            {
                // Login successful, perform necessary actions (e.g., set authentication cookie)
                return RedirectToAction("Index", "Todopage");
            }
            else
            {
                // Invalid login, display error message
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }
        }
    }
}