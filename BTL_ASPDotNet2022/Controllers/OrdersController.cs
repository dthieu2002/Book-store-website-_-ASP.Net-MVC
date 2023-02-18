using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_ASPDotNet2022.Data;
using BTL_ASPDotNet2022.Models;
using BTL_ASPDotNet2022.Extensions;

namespace BTL_ASPDotNet2022.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BTL_ASPDotNet2022Context _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrdersController(BTL_ASPDotNet2022Context context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var bTL_ASPDotNet2022Context = _context.Order.Include(o => o.Account);
            return View(await bTL_ASPDotNet2022Context.ToListAsync());
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            List<Order_Details> orders = this._context.Order_Details.Where(m => m.OrderId == id).ToList();

            if (orders is null) return NotFound();
            else
            {
                // Initial Book and Order
                foreach(Order_Details item in orders)
                {
                    // Book
                    item.Book = this._context.Book.FirstOrDefault(m => m.Id == item.BookId);
                    
                    // Order
                    //item.Book = this._context.Book.FirstOrDefault(m => m.Id == item.BookId);

                }
            }

            var order = this._context.Order.FirstOrDefault(m => m.Id == id);
            if(order != null)
            {
                ViewBag.CurrentOrder = order;


                // Set username handle if the order has been handle
                if (!order.OrderStatus!.ToLower().Equals("waiting"))
                {
                    var processingOrder = _context.Order_Processing.FirstOrDefault(m => m.OrderId == id);
                    if(processingOrder != null)
                    {
                        ViewBag.UsernameHandle = processingOrder.Username;
                        ViewBag.TimeHandle = processingOrder.ProcessingDate.ToString();
                    }
                }
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Account, "Username", "Username");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,OrderDate,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Account, "Username", "Username", order.Username);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.Account, "Username", "Username", order.Username);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,OrderDate,OrderStatus")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["Username"] = new SelectList(_context.Account, "Username", "Username", order.Username);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'BTL_ASPDotNet2022Context.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderManager");
        }

        private bool OrderExists(int id)
        {
          return _context.Order.Any(e => e.Id == id);
        }


        public IActionResult OrderManager(string SearchString, string StatusFilter)
        {
            IQueryable<string> statusList = from m in _context.Order
                                          orderby m.OrderStatus
                                          select m.OrderStatus;
            var orders = from m in _context.Order
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                orders = orders.Where(m => m.Id == Convert.ToInt32(SearchString));
            }

            if (!string.IsNullOrEmpty(StatusFilter))
            {
                orders = orders.Where(m=>m.OrderStatus == StatusFilter);
            }


            // Set account in order
            foreach(Order item in orders.ToList())
            {
                item.Account = _context.Account.FirstOrDefault(m => m.Username == item.Username);
            }


            var orderFilterVM = new OrderFilter()
            {
                OrderList = orders.OrderByDescending(m=>m.OrderDate).ToList(),
                StatusList = new SelectList(statusList.Distinct().ToList())
            };

            return View(orderFilterVM);
        }

        public IActionResult HandleOrder(string Choose, string OrderId)
        {
            // Change status
            var currentOrder = _context.Order.FirstOrDefault(m => m.Id == Convert.ToInt32(OrderId));


            if(Choose == "accept")
            {
                // Check number of book has buy with inventory quantity of book
                List<Order_Details> orderDetailList = _context.Order_Details.Where(m => m.OrderId == currentOrder!.Id).ToList();
                foreach (var item in orderDetailList)
                {
                    item.Book = _context.Book.FirstOrDefault(m => m.Id == item.BookId);

                    if (item.Book.InventoryQuantity < item.Amount)
                    {
                        this._contextAccessor.HttpContext!.Session.Set<SweetAlert>(
                            "SweetAlert",
                            new SweetAlert()
                            {
                                Title = "Book name: " + item.Book.Name + " is not enought!",
                                Message = "Please ADDTIONAL the INVENTORY QUANTITY OF BOOK before accepting the order!",
                                Type = "error"
                            }
                        );

                        return Redirect("/Orders/Details/" + OrderId);   // OUT POINT
                    }
                }


                // Decending the inventory quantity of book in DB
                foreach (var item in orderDetailList)
                {
                    var book = _context.Book.FirstOrDefault(m => m.Id == item.BookId);

                    if(book != null && item != null)
                    {
                        book.InventoryQuantity -= (int)item.Amount;

                        // Update in dbContext ( BOOK ) 
                        _context.Update(book);
                        _context.SaveChanges();
                    }
                }

            }
            
            
            if(currentOrder != null)
            {
                currentOrder.OrderStatus = Choose;

                // Add new records in tbl Processing_Order
                _context.Order_Processing.Add(new Order_Processing()
                {
                    OrderId = Convert.ToInt32(OrderId),
                    Username = HttpContext.Session.GetString("UsernameLogin"), // User was handle order
                    ProcessingDate = DateTime.Now
                });

                int x = _context.SaveChanges();
                if(x == 0)
                {
                    return NotFound(); // Error
                }
                else
                {
                    return Redirect("/Orders/Details/" + OrderId);
                }
            }
            else
            {
                return NotFound(); // Error
            }
        }
    }
}
