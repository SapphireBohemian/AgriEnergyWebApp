using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyWebApp.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AgriEnergyWebApp.Controllers
{
    public class EmployeesController : Controller

    {
        private readonly AgriConnectContext _context;
        AgriConnectContext db = new AgriConnectContext();

        public EmployeesController(AgriConnectContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        //Shows the Login form
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        //Recieves data from the login form
        [HttpPost]
        public ActionResult Login(Employee e)
        {
            //Code below to hash password 
            //Learnt and adapted from :
            //Author : Afzaal Ahmad Zeeshan
            //Link : https://www.c-sharpcorner.com/article/hashing-passwords-in-net-core-with-tips/
            using (var sha256 = SHA256.Create())
            {

                HttpContext.Session.SetString("CurrentUser", e.AdminId);
                //Variables declared to take in employee id and password from login 
                string adminID = HttpContext.Session.GetString("CurrentUser");
                string pass = e.Password;


                // Sending the password to be hashed
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string verison of the password
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                //Employee is matched to the employee table in the database in order to allow for login
                Employee user = db.Employees.Where(us => us.AdminId.Equals(e.AdminId) && us.Password.Equals(pass)).FirstOrDefault();

                if (user != null)
                {
                    //The employee can now enter and will be taken to the main menu page
                    return RedirectToAction("Emp", "Home");
                }
                else
                {

                    //wrong credentials,diplay error message
                    ViewBag.Login = "INVALID USERNAME OR PASSWORD";
                    return View();

                }
            }


        }
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.AdminId == id);
        }
    }
}
