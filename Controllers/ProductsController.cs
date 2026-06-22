using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Filters;
using ProductManagement.Models;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, string category, string sortOrder)
        {
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["StockSortParam"] = sortOrder == "stock" ? "stock_desc" : "stock";
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSort"] = sortOrder;

            var products = _context.Products.AsQueryable();

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.Description!.Contains(searchString) ||
                    p.Category.Contains(searchString));
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // Sorting
            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "stock" => products.OrderBy(p => p.Stock),
                "stock_desc" => products.OrderByDescending(p => p.Stock),
                _ => products.OrderBy(p => p.Name)
            };

            // Load categories for the filter dropdown
            ViewData["Categories"] = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        [AdminOnly]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Price,Stock,Category,IsActive")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.UtcNow;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Product '{product.Name}' was created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [AdminOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Edit/5
        [AdminOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Description,Price,Stock,Category,IsActive,CreatedAt")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdatedAt = DateTime.UtcNow;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Product '{product.Name}' was updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [AdminOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [AdminOnly]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                var name = product.Name;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Product '{name}' was deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id) =>
            _context.Products.Any(e => e.Id == id);
    }
}
