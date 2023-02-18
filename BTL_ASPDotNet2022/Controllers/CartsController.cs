using Microsoft.AspNetCore.Mvc;

using BTL_ASPDotNet2022.Models;
using BTL_ASPDotNet2022.Extensions;
using BTL_ASPDotNet2022.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace BTL_ASPDotNet2022.Controllers
{
    public class CartsController : Controller
    {
        private readonly BTL_ASPDotNet2022Context _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartsController(BTL_ASPDotNet2022Context _context, IHttpContextAccessor _contextAccessor)
        {
            this._context = _context;
            this._contextAccessor = _contextAccessor;

            // Initial cart if it not exists
            if (_contextAccessor.HttpContext.Session.Get<Cart>("MyCart") is null)
            {
                _contextAccessor.HttpContext.Session.Set<Cart>("MyCart",new Cart());
            }
        }

        public IActionResult Index()
        {
            var cart = this._contextAccessor.HttpContext.Session.Get<Cart>("MyCart");
            return View(cart.Items);
        }

        public IActionResult Add(string id)
        {
            // Get cart object in session
            var cart = HttpContext.Session.Get<Cart>("MyCart");

            // Get book object with id == id 
            var book = _context.Book.FirstOrDefault(h => h.Id == Convert.ToInt32(id));

            // Add into list item of cart
            if(book != null)
            {
                cart.AddItem(new Cart_Item(book, 1));
                HttpContext.Session.Set<Cart>("MyCart", cart);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string id)
        {
            var cart = HttpContext.Session.Get<Cart>("MyCart");
            cart.RemoveItem(Convert.ToInt32(id));
            HttpContext.Session.Set<Cart>("MyCart", cart);

            return RedirectToAction("Index", "Carts");
        }

        public IActionResult Order()
        {
            // Check login , if user isn't login => go to login screen
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("UsernameLogin")))
            {
                this._contextAccessor.HttpContext.Session.Set<SweetAlert>(
                    "SweetAlert",
                    new SweetAlert()
                    {
                        Title = "DTH book store",
                        Message = "You must login fisrt to buy!",
                        Type = "info"
                    }
                );
                return RedirectToAction("SignIn", "DTHBookStore");
            }

            return RedirectToAction("ExecuteOrder");
        }

        public IActionResult ExecuteOrder()
        {
            Account acc = _context.Account.FirstOrDefault(m => m.Username == HttpContext.Session.GetString("UsernameLogin"));
            if (acc is null) acc = new Account();

            // Step 1: Add order into table Order
            Models.Order newOrder = new Models.Order()
            {
                Account = acc,
                OrderDate = DateTime.Now,
                OrderStatus = "waiting",
                Username = acc.Username
            };
            _context.Order.Add(newOrder);
            _context.SaveChanges();

            // Step 2: Add data into table Order_Details
            Cart cart = HttpContext.Session.Get<Cart>("MyCart");

            foreach(Cart_Item cart_Item in cart.Items)
            {
                _context.Order_Details.Add(new Order_Details()
                {
                    //Order = newOrder,
                    OrderId = newOrder.Id,
                    //Book = cart_Item.Book,   // Don't assign then working
                    BookId = cart_Item.Book.Id,
                    Amount = cart_Item.Quantity
                });

                _context.SaveChanges();
            }

            List<Order_Details> ds = _context.Order_Details.Where(m => m.OrderId == newOrder.Id).ToList();


            /*
            using (var _transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Book ON;");
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Book OFF;");
                _transaction.Commit();
            }
            */

            //Last step: Delete cart
            HttpContext.Session.Remove("MyCart");

            //Final: Go to history order of current account
            return RedirectToAction("OrderHistory", "Accounts");
        }
    }
}
