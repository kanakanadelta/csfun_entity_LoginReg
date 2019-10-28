using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LoginReg.Models;


using System.Linq;
using System.Collections.Generic;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private RegContext DbContext;

        public HomeController(RegContext context)
        {
            DbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(DbContext.Users.Any(u => u.Email == user.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "This Email is already in use!");
                    // return the view
                    return View("Index");
                }    

                if(user.Password != user.Confirm)
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Password", "Password confirmation was not the same password.");
                    // return the view
                    return View("Index");
                }
                // // // // // // // // // //
                // hash the given password:

                // Initialize the hasher object
                var hasher = new PasswordHasher<User>();

                user.Password = hasher.HashPassword(user, user.Password);
                DbContext.Add(user);
                DbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                return View("Index");
            }
        }
    }
}