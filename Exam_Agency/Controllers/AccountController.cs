using Core.Models;
using Exam_Agency.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam_Agency.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerdto)
        {
            if(!ModelState.IsValid)
            {
                return View(registerdto);
            }
            User user = new User()
            {
                Name = registerdto.Name,
                Surname = registerdto.Surname,
                Email = registerdto.Email,
                UserName = registerdto.Username
            };
            var result= await _userManager.CreateAsync(user,registerdto.Password);
            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }
                return View(registerdto);
            }
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Login");
            
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            if(!ModelState.IsValid)
            {
                return View(logindto);
            }
            var user = await _userManager.FindByEmailAsync(logindto.UsernameOrEmail);
            if(user == null)
            {
                user=await _userManager.FindByNameAsync(logindto.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "Password or Email/Username is not valid");
                    return View();
                }
            }
            var result=  await _signInManager.CheckPasswordSignInAsync(user, logindto.Password, true);
            if(result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try again later");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or Email/Username is not valid");
                return View();
            }
            await _signInManager.SignInAsync(user,logindto.IsRemember);
            return RedirectToAction("Index","Home");

        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role = new IdentityRole("Admin");
            IdentityRole role1 = new IdentityRole("User");
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role);
            return Ok();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
