using coretest.dbdata;
using coretest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace coretest.Controllers
{
    public class HomeController : Controller
    {

        [Authorize]
        public IActionResult Index()
        {

            mytestContext database = new mytestContext();
            List<MyModel> mm = new List<MyModel>();
            var res = database.Mytables.ToList();

            foreach (var item in res)
            {
                mm.Add(new MyModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    City = item.City,
                });


            }
            return View(mm);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(MyModel obj)
        {

            mytestContext database = new mytestContext();
            //  MyModel mm = new MyModel();
            Mytable tt = new Mytable();
            tt.Id = obj.Id;
            tt.Name = obj.Name;
            tt.Email = obj.Email;
            tt.City = obj.City;

            if (obj.Id == 0)
            {
                database.Mytables.Add(tt);
                database.SaveChanges();
            }
            else
            {
                database.Entry(tt).State = EntityState.Modified;
                database.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Del(int id)
        {
            mytestContext database = new mytestContext();
            //Mytable tt = new Mytable();

            var de = database.Mytables.Where(i => i.Id == id).FirstOrDefault();
            database.Mytables.Remove(de);
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            mytestContext database = new mytestContext();
            //  Mytable tt = new Mytable();
            MyModel tt = new MyModel();
            var editing = database.Mytables.Where(e => e.Id == id).FirstOrDefault();
            tt.Id = editing.Id;
            tt.Name = editing.Name;
            tt.Email = editing.Email;
            tt.City = editing.City;



            return View("Add", tt);   //<method>, <obj of the model>
        }

        //=================================Login============================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModel obj)
        {


            mytestContext database = new mytestContext();

            var res = database.Logintbls.Where(a => a.Email == obj.Email).FirstOrDefault();



            if (res == null)
            {

                TempData["Invalid"] = "Email is not found";
            }

            else
            {
                if (res.Email == obj.Email && res.Pass == obj.Pass)
                {

                    var claims = new[] {/* new Claim(ClaimTypes.Pass, res.Pass),*/
                                     new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                     CookieAuthenticationDefaults.AuthenticationScheme,
                     new ClaimsPrincipal(identity),
                   authProperties);


                    //   HttpContext.Session.SetString("Name", obj.Email);

                    return RedirectToAction("Index", "Home");

                }

                else
                {

                    ViewBag.Inv = "Wrong Email Id or password";

                    return View("Login");
                }


            }


            return View("Login");
        }

        //================================Logout===================================

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return View("Login");
        }


        //==========================Add Data in Register==============================

        [HttpGet]
      
        public IActionResult Addred()
        {
            return View();
        }

        [HttpPost]
      
        public IActionResult Addred(LoginModel obj)
        {
            mytestContext database = new mytestContext();
            Logintbl tt = new Logintbl();

            tt.Id = obj.Id;
            tt.Email=obj.Email;
            tt.Pass= obj.Pass;

            database.Logintbls.Add(tt);
            database.SaveChanges();

            return View("Login");
        }
    }
}
