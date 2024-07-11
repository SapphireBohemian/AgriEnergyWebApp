using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyWebApp.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AgriEnergyWebApp.Controllers
{
    public class FarmersController : Controller
    {
        private readonly AgriConnectContext _context;
        AgriConnectContext db = new AgriConnectContext();

        public FarmersController(AgriConnectContext context)
        {
            _context = context;
        }

        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Farmers.ToListAsync());
        }
        // GET: Products
        public async Task<IActionResult> Products(string id,DateTime toDate,DateTime fromDate,string searchString)
        {

            ViewData["FarmerId"] = id;
            ViewData["toDate"] = toDate;
            ViewData["fromDate"] = fromDate;
            Helper.frmID = id;
            if (!String.IsNullOrEmpty(searchString))
            {   // Code to create filter function adapted from:
                //Author:Rick Anderson
                //Link:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search?view=aspnetcore-6.0
                // Filters database for searched product type
                var types = _context.Products.Where(p => p.FarmerId == id && p.ProductType!.Contains(searchString));
                return View(await types.ToListAsync());
            }
            else if (toDate != default(DateTime) && fromDate != default(DateTime))
            {
                
              //Filters database to show products between 2 dates
                var type = _context.Products.Where(p => p.FarmerId == id && p.ProductDate >= fromDate && p.ProductDate <= toDate);
                return View(await type.ToListAsync());
            }
            else { 

            //Displays list of user specific products
            var farmCentralContext = _context.Products.Where(p => p.FarmerId == id);
            return View(await farmCentralContext.ToListAsync());

        }
        }

        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Products/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductQuantity,ProductType,ProductDate,FarmerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.FarmerId = HttpContext.Session.GetString("CurrentUser");
                _context.Add(@product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            ViewData["FarmerId"] = id;
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            // ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductQuantity,ProductType,ProductDate,FarmerId")] Product product,String frm)
        {
            frm = Helper.frmID;
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.FarmerId = frm;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        //Shows the Login form
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        //Recieves data from the login form
        [HttpPost]
        public ActionResult Login(Farmer f)
        {
            //Code below to hash password 
            //Learnt and adapted from :
            //Author : Afzaal Ahmad Zeeshan
            //Link : https://www.c-sharpcorner.com/article/hashing-passwords-in-net-core-with-tips/
            using (var sha256 = SHA256.Create())
            {

                HttpContext.Session.SetString("CurrentUser", f.FarmerId);
                //Variables declared to take in farmer ID and password from login 
                string farmerID = HttpContext.Session.GetString("CurrentUser");
                string pass = f.Password;


                // Sending the password to be hashed
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string verison of the password
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                //Farmer is matched to the farmer table in the database in order to allow for login
              Farmer user = db.Farmers.Where(us => us.FarmerId.Equals(f.FarmerId) && us.Password.Equals(hash)).FirstOrDefault();

                if (user != null)
                {
                    //The user can now enter and will be taken to the main menu page
                    return RedirectToAction("New", "Home");
                }
                else
                {

                    //wrong credentials,diplay error message
                    ViewBag.Login = "INVALID USERNAME OR PASSWORD";
                    return View();

                }
            }


        }


        //Shows the form
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //recieves data
        [HttpPost]
        public ActionResult Register(Farmer f)
        {
            HttpContext.Session.SetString("CurrentUser", f.FarmerId);
            string farmerID = HttpContext.Session.GetString("CurrentUser");
            string pass = f.Password;
            string fname = f.FarmerName;
            using (var sha256 = SHA256.Create())
            {
                // Sending the password to be hashed
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                // Get the hashed string version of the password
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                //Create a new farmer object which will be stored in the database
                Farmer newF = new Farmer();
                newF.FarmerId = farmerID;
                newF.Password = hash;
                newF.FarmerName = fname;
                //Validation to check if user is in database already
                var UserAlreadyExists = db.Farmers.Any(x => x.FarmerId == newF.FarmerId);
                if (UserAlreadyExists)
                {

                    ViewBag.Fail = "USER WITH THIS STUDENT NUMBER ALREADY EXISTS";
                }
                else
                {
                    //New farmer added to the database
                    db.Farmers.Add(newF);
                    //Code to save user in database using Async method
                    db.SaveChangesAsync();

                    return RedirectToAction("Emp", "Home");
                }
            }
            return View();
        }

        // GET: Farmers1/Edit/5
        public async Task<IActionResult> Reset(string id)
        {
            id = HttpContext.Session.GetString("CurrentUser");
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: Farmers1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(string id, [Bind("FarmerId,FarmerName,Password")] Farmer farmer)
        {      id = HttpContext.Session.GetString("CurrentUser");
            if (id != farmer.FarmerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var sha256 = SHA256.Create())
                    {

                        HttpContext.Session.SetString("CurrentUser", farmer.FarmerId);
                        //Variables declared to take in farmer ID and password from login 
                        string farmerID = HttpContext.Session.GetString("CurrentUser");
                        string pass = farmer.Password;


                        // Sending the password to be hashed
                        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                        // Get the hashed string verison of the password
                        var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        farmer.Password = hash;
                        _context.Update(farmer);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.FarmerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("New", "Home"); 
            }
            return View(farmer);
        }
        private bool FarmerExists(string id)
        {
            return _context.Farmers.Any(e => e.FarmerId == id);
        }
    }
}
