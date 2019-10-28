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

        [HttpGet("login")]
        public IActionResult Login()
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

        [HttpPost("loginconfirm")]
        public IActionResult LoginConfirm(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = DbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Login");
                }
                else
                {
                    return RedirectToAction("Success");
                }
                
            }
            else
            {
                return View("Index");
            }
        }
    }
}