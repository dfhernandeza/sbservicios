using AppWebSantaBeatriz.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.Funciones;

namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class usersController : Controller
    {
        
        // GET: users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (User.IsInRole("Admin"))
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
        
        
        
        
        }
        
        

    }
}