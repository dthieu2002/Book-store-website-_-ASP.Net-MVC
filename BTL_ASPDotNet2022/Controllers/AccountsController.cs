using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_ASPDotNet2022.Data;
using BTL_ASPDotNet2022.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Http;
using BTL_ASPDotNet2022.Extensions;

// Note: Default form method is "POST" , Default HTTP is "HttpGet"
namespace BTL_ASPDotNet2022.Controllers
{
    public class AccountsController : Controller
    {
        private readonly BTL_ASPDotNet2022Context _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountsController(BTL_ASPDotNet2022Context context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(string SearchString, string RoleFilter)
        {
            // === Only 'master' and 'staff' are allowed ===
            if (_contextAccessor.HttpContext.Session.GetString("RoleLogin") is null || _contextAccessor.HttpContext.Session.GetString("RoleLogin") == "customer")
            {
                return NotFound();
            }

            IQueryable<string> rolesQuery = from m in _context.Account
                             orderby m.Role
                             select m.Role;

            var accounts = from m in _context.Account
                           select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                accounts = accounts.Where(m => m.Username!.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(RoleFilter))
            {
                accounts = accounts.Where(m => m.Role!.Equals(RoleFilter));
            }

            var accountFilterVM = new AccountFilter()
            {
                Roles = new SelectList(await rolesQuery.Distinct().ToListAsync()),
                Accounts = await accounts.ToListAsync()
            };

            return View(accountFilterVM);

            //return View(await _context.Account.Where(m=> m.Username.Contains(SearchString) && m.Role.Contains(RoleFilter)).ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            // === Redirect to not found if user are not logged in or view info of id != UsernameLogin ===
            if (_contextAccessor.HttpContext.Session.GetString("UsernameLogin") is null )
            {
                return NotFound();
            }
            if(HttpContext.Session.GetString("RoleLogin")!="master" && !_contextAccessor.HttpContext.Session.GetString("UsernameLogin").Equals(id))
            {
                return NotFound();
            }

            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Username == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create     ( From register account of user )
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username, Password, FullName, BirthDay, Gender, Address, Phone, Role")] Account account)
        {
            // Check username, if already exists then raise error
            if (this._context.Account.Any(x => x.Username == account.Username))
            {
                ModelState.AddModelError("PKErr", "Username '"+account.Username+"' is already existst!");
                return View(account);
            }

            // Validation data ( required, minLength, range, ... )
            if (ModelState.IsValid)
            {
                account.RegisterDate = DateTime.Now;
                _context.Add(account); // Add new account
                await _context.SaveChangesAsync();

                // Break point ( case is master add new account in master view )
                if(_contextAccessor.HttpContext.Session.GetString("UsernameLogin") != null)
                {
                    return RedirectToAction("Index", "Accounts");
                }
                
                // Set sweet alert to show message welcome ( first login ) 
                _contextAccessor.HttpContext.Session.Set<SweetAlert>(
                    "SweetAlert",
                    new SweetAlert()
                    {
                        Title = "DTH book store",
                        Message = "Welcome to DTH book store",
                        Type = "success"
                    }
                );

                // Save username and role login in session
                _contextAccessor.HttpContext.Session.SetString("UsernameLogin", account.Username);
                _contextAccessor.HttpContext.Session.SetString("RoleLogin", account.Role);

                // Redirect to home screen ( DTHBookStore/Index )
                if (account.Role == "customer")
                {
                    return RedirectToAction("Index", "DTHBookStore");
                }

                // Case role is master or staff
                return RedirectToAction("Index", "Administrator");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            // === Redirect to not found if user are not logged in or view info of id != UsernameLogin ===
            if (_contextAccessor.HttpContext.Session.GetString("RoleLogin") is null)
            {
                return NotFound();
            }
            else if(_contextAccessor.HttpContext.Session.GetString("RoleLogin") != "master")
            {
                if(_contextAccessor.HttpContext.Session.GetString("UsernameLogin") != id)
                {
                    return NotFound();
                }
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,FullName,BirthDay,Gender,Address,Phone,Role,RegisterDate")] Account account)
        {
            if (id != account.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Accounts/Details/"+account.Username);
            }
            return View(account);
        }

        // GET: Accounts/Delete/5   ( only master can use this function )
        public async Task<IActionResult> Delete(string id)
        {
            // === Redirect to not found if user are not logged in or view info of id != UsernameLogin ===
            if (_contextAccessor.HttpContext.Session.GetString("UsernameLogin") is null )
            {
                return NotFound();
            }
            if(HttpContext.Session.GetString("RoleLogin") != "master")
            {
                return NotFound();
            }

            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Username == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'BTL_ASPDotNet2022Context.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
          return _context.Account.Any(e => e.Username == id);
        }


        // ======== Function was write =========
        [HttpPost]
        public IActionResult CheckSignIn(Account account)
        {
            // Step 1: Check username and password (Case sensitive => not use LINQ)
            if (this.CheckAccount(account))
            {
                // Successful, add username and role of account login into session
                this._contextAccessor.HttpContext.Session.SetString("UsernameLogin", account.Username);
                
                Account x = (Account)this._context.Account.FirstOrDefault(m => m.Username == account.Username);
                this._contextAccessor.HttpContext.Session.SetString(
                    "RoleLogin", 
                    x.Role
                );

                // === Authorize
                if (x.Role.Equals("customer"))
                {
                    // Set sweet alert to show message welcome ( first login ) 
                    _contextAccessor.HttpContext.Session.Set<SweetAlert>(
                        "SweetAlert",
                        new SweetAlert()
                        {
                            Title = "DTH book store",
                            Message = "Welcome to DTH book store",
                            Type = "success"
                        }
                    );

                    //Go to home screen
                    return RedirectToAction("Index", "DTHBookStore");
                }
                else 
                {
                    // is staff or master
                    return RedirectToAction("Index", "Administrator");
                }
                
            }
            else
            {
                // Login fail , go to sign in screen with error 
                _contextAccessor.HttpContext.Session.Set<SweetAlert>(
                    "SweetAlert",
                    new SweetAlert()
                    {
                        Title = "Login failed",
                        Message = "Username or password is wrong!",
                        Type = "error"
                    }
                );

                return RedirectToAction("SignIn", "DTHBookStore");
            }
        }

        private bool CheckAccount(Account account)
        {
            List<Account> accounts = this._context.Account.ToList();

            foreach(Account item in accounts)
            {
                if(!string.IsNullOrEmpty(item.Username) && !string.IsNullOrEmpty(item.Password))
                {
                    if (item.Username.Equals(account.Username) && item.Password.Equals(account.Password))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // History order
        public IActionResult OrderHistory()
        {
            string? usernameLogin = HttpContext.Session.GetString("UsernameLogin");
            Account? acc = this._context.Account.FirstOrDefault(m => m.Username == usernameLogin);

            if(acc != null)
            {
                List<Order> orders = this._context.Order.Where(m => m.Username == acc.Username).OrderByDescending(m=>m.OrderDate).ToList();

                // return list order of current account
                return View(orders);
            }

            throw new Exception("Account not valid!");
        }
    }
}
