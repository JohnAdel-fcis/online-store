
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using market.Models;
using market.Models.help;

namespace market.Controllers
{
    public class HomeController : Controller
    {
       public DataBase db = new DataBase();
        // GET: Home
        public ActionResult Index( int ?id )
        {
            var us = GlobalVariables.LoginUser;
            if (id !=0 && id!= null)
            {
                var currentUser = db.Users.First(item => item.Id == id);
                ViewBag.name = currentUser.Name;
                ViewBag.userId = currentUser.Id;
            }
            else
            {
                ViewBag.name = "Login";
                ViewBag.userId = 0;
            }
            
           
            var itemList = db.Items.ToList();

            return View(itemList);
        }
        [HttpPost]
        public ActionResult Index(string name,int? id ,string searchString)
        {
            var items = (from m in db.Items
                select m).ToList();


            List<Item> myDb = db.Items.ToList();

            IEnumerable<Item> selected ;
            ViewBag.name = name ?? "Login";
            if (id !=0 &&id!=null)
            {
                ViewBag.id = id;
            }
            if (!String.IsNullOrEmpty(searchString) && searchString != "Search")
            {
                 selected =   items.Where(s => s.ItemName.Contains(searchString)) ;
                
            }
            else
            {
                selected=items;
            }
            return View(selected);
        }

    }
}