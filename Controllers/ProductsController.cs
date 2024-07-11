using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace AgriEnergyWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AgriConnectContext _context;
       
        public ProductsController(AgriConnectContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(String FarmerLogin, String searchString, DateTime toDate, DateTime fromDate)
        {
            FarmerLogin = HttpContext.Session.GetString("CurrentUser");
            
            ViewData["toDate"] = toDate;
            ViewData["fromDate"] = fromDate;



                if (!String.IsNullOrEmpty(searchString))
                {
                // Code to create filter function adapted from:
                //Author:Rick Anderson
                //Link:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search?view=aspnetcore-6.0
                // Filters database for searched product type
                var types = _context.Products.Where(x => x.FarmerId == FarmerLogin && x.ProductType!.Contains(searchString));
                    return View(await types.ToListAsync());
                }
                else if (toDate != default(DateTime) && fromDate != default(DateTime))
                {
                    //Filters database for products between date range
                    var type = _context.Products.Where(x => x.FarmerId == FarmerLogin && x.ProductDate >= fromDate && x.ProductDate <= toDate);
                    return View(await type.ToListAsync());
                }
                else
                {
                   //Displays list of user specific products
                    var FarmCentralContext = _context.Products.Where(x => x.FarmerId == FarmerLogin);
                    return View(await FarmCentralContext.ToListAsync());
                }

            }

        
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
           // ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        public async Task<IActionResult> Edit(int? id,String FarmerLogin)
        {
            FarmerLogin = HttpContext.Session.GetString("CurrentUser");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.FarmerId = FarmerLogin;
           // ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductQuantity,ProductType,ProductDate,FarmerId")] Product product)
        {
           String FarmerLogin = HttpContext.Session.GetString("CurrentUser");
            product.FarmerId = FarmerLogin;
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
           // ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "FarmerId", product.FarmerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

      
    }
    
}
