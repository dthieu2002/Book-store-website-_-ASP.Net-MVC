using BTL_ASPDotNet2022.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;
using BTL_ASPDotNet2022.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;

using BTL_ASPDotNet2022.Extensions;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_ASPDotNet2022.Controllers
{
    // Note: The default name view has the same name with the action
    public class DTHBookStoreController : Controller
    {
        private  ILogger<DTHBookStoreController> _logger;
        private  BTL_ASPDotNet2022Context _context;
        private  IHttpContextAccessor _contextAccessor;

        public DTHBookStoreController(ILogger<DTHBookStoreController> logger, BTL_ASPDotNet2022Context _context, IHttpContextAccessor _contextAccessor)
        {
            _logger = logger;
            this._context = _context;
            this._contextAccessor = _contextAccessor;

        }


        public IActionResult Index()
        {
            //Initial list 
            var bookList = from m in _context.Book
                           select m;
            IQueryable<string> bookTitleList = from m in _context.Book
                                               select m.Gender;


            var BookCaseFilterVM = new BookcaseFilter()
            {
                BookList = _context.Book.ToList(),
                BookTitleRenderList = bookTitleList.Distinct().ToList()
            };

            return View(BookCaseFilterVM);
        }

        public async Task<IActionResult> Search(string SearchString="")
        {
            ViewBag.SearchString = SearchString;
            // Return view has list of book
            if (!string.IsNullOrEmpty(SearchString))
            {
                return View(await this._context.Book.Where(hieu => hieu.Name.Contains(SearchString) || hieu.Author.Contains(SearchString) || hieu.Gender.Contains(SearchString)).ToListAsync());
            }
            return View(await _context.Book.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        // Get: DTHBookStore/SignIn
        public IActionResult SignIn(Account account)
        {
            return View(account);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return RedirectToAction("Create", "Accounts");
        }

        public string ForgotPassword()
        {
            return "Forgot pass";
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // == this._contextAccessor.Session.Clear();

            return RedirectToAction("Index");
        }

        public IActionResult Master()
        {
            
            return View();
        }

        public IActionResult ViewAccountInfo(string id)
        {
            return Redirect("/Accounts/Details/" + id);
        }

        public IActionResult Bookcase(string BookTitleFilter)
        {
            //Initial list 
            var bookList = from m in _context.Book
                           select m;
            IQueryable<string> bookTitleList = from m in _context.Book
                                               select m.Gender;


            var BookCaseFilterVM = new BookcaseFilter()
            {
                BookList = _context.Book.ToList(),
                BookTitleList = new SelectList( bookTitleList.Distinct().ToList())
            };

            // Filter
            if (!string.IsNullOrEmpty(BookTitleFilter))
            {
                List<String> x = new List<string>()
                {
                    BookTitleFilter
                };
                BookCaseFilterVM.BookTitleRenderList = x;
            }
            else
            {
                BookCaseFilterVM.BookTitleRenderList = bookTitleList.Distinct().ToList();
            }

            return View(BookCaseFilterVM);
        }
    }
}