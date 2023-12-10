using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using _08122023adminapproval.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _14112023crudcodefirst.Controllers
{

    public class UserController : Controller
    {
        private readonly Role1Context _context;

        public UserController(Role1Context context)
        {
            _context = context;
        }

        // GET: Employee
        //[Authorize(Roles = "User,Admin")]

        public IActionResult Index()
        {
            var employees = _context.Registrations.Where(u=> u.Id!=1).ToList();
            return View(employees);
        }

        public IActionResult Registration() {
            var roles = _context.Roles.ToList();
            ViewData["Roles"] = new SelectList(roles, "Roleid", "Rolename");
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration register)
        {
            //if(_context.Registrations.Any(u=> u.Email == register.Email){
            //    return View();
            //}

            _context.Registrations.Add(register);
            _context.SaveChanges();
            return RedirectToAction("login");
        }

        public IActionResult Details(int id)
        {
            var user = _context.Registrations.Include("Role").FirstOrDefault(u => u.Id == id);
            return View(user);
        }


        public IActionResult Error(string message)
        {
            ViewBag.errorMessage = message;
            return View();
        }

        public IActionResult Approve(string id)
        {
            var user = _context.Registrations.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
            user.IsActive = 1;
            _context.Registrations.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


        public IActionResult DeApprove(string id)
        {
            var user = _context.Registrations.FirstOrDefault(u => u.Id == Convert.ToInt32(id));
            user.IsActive = 0;
            _context.Registrations.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }







        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(String username, string password)
        {
            if (username != null && password != null)
            {
                var user = await _context.Registrations.FirstOrDefaultAsync(u => u.Email == username && u.Password == password);
                var userRole = _context.Roles.FirstOrDefault(r => r.Roleid == user.Roleid);
                ClaimsIdentity identity = null;
                bool isAuthenticated = false;
                if (user != null)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                         new Claim(ClaimTypes.Name, user.Email),
                         new Claim(ClaimTypes.Role, userRole.Rolename)
                     },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticated = true;
                }
                else
                {
                    return RedirectToAction("Error", new { message = "Invalid Credentials" });

                }
                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (user.Roleid == 1 && user.IsActive == 1)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (user.Roleid == 1 && user.IsActive == 0)
                    {
                        return RedirectToAction("Error", new { message = "You are not Approved by Admin. Please contact Admin" });
                    }
                    else if (user.Roleid == 2 && user.IsActive == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.Roleid == 2 && user.IsActive == 0)
                    {
                        return RedirectToAction("Error", new { message = "You are not Approved by Admin. Please contact Admin" });
                    }
                    else
                    {
                        return RedirectToAction("Error", new { message = "Internal Server Error. Please contact Admin" });
                    }

                }
            }
            else
            {
                return View("Email & Password are not provided.");
            }
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("login");
        }

        // GET: Employee/Create
        //[Authorize(Roles = "Admin")]
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // POST: Employee/Create
        //[HttpPost]
        // public IActionResult Create(Employee employee)
        // {
        //     try
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             _context.Employee.Add(employee);
        //             _context.SaveChanges();
        //             return RedirectToAction("Index");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         ModelState.AddModelError("", "An error occurred while saving the record.");
        //     }

        //     return View(employee);
        // }

        // GET: Employee/Edit
        //[Authorize(Roles = "Admin")]
        // public IActionResult Edit(int id)
        // {
        //     var employee = _context.Employee.Find(id);
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(employee);
        // }

        // POST: Employee/Edit
        //[HttpPost]
        // public IActionResult Edit(Employee employee)
        // {
        //     try
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             _context.Entry(employee).State = EntityState.Modified;
        //             _context.SaveChanges();
        //             return RedirectToAction(nameof(Index));
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         ModelState.AddModelError("", "An error occurred while updating the record.");
        //     }

        //     return View(employee);
        // }

        // GET: Employee/Delete
        //[Authorize(Roles = "Admin")]
        // public IActionResult Delete(int id)
        // {
        //     var employee = _context.Employee.Find(id);
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(employee);
        // }

        // POST: Employee/Delete
        //[HttpPost, ActionName("Delete")]
        // public IActionResult DeleteConfirmed(int id)
        // {
        //     var employee = _context.Employee.Find(id);
        //     if (employee == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Employee.Remove(employee);
        //     _context.SaveChanges();
        //     return RedirectToAction(nameof(Index));
        // }


        // public person Authenticate(string username, string password)
        // {
        //     var user = _context.person.FirstOrDefault(u => u.username == username && u.password == password);

        //     return user;
        // }
    }

}
