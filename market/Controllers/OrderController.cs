using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using market.Models;
using market.Models.help;

namespace market.Controllers
{
    public class OrderController : Controller
    {
        private DataBase db = new DataBase();
        // GET: Order
        public ActionResult Index( int? itemId)
        {
            var user = GlobalVariables.LoginUser;
            if (user==null)
            {
                return RedirectToAction("RealLogin", "Login");
            }
            else
            {
                var item = db.Items.First(i => i.ItemId == itemId);
                ViewBag.name = user.Name;
                ViewBag.userId = user.Id;
                return View(item);
            }
            
        }

        [HttpPost]
        public ActionResult Index(int Quantity1 , Item itm)
        {
            BasketElement item = new BasketElement {Item = itm, Quantity = Quantity1};
            if (GlobalVariables.counter == 0)
            {
                List<BasketElement>basketElements = new List<BasketElement>();
                basketElements.Add(item);
                Basket basket = new Basket {BasketIElements = basketElements};
                GlobalVariables.MyBasket = basket;
            }
            else
            {
                var basketElements = GlobalVariables.MyBasket.BasketIElements;
                basketElements.Add(item);
                GlobalVariables.MyBasket.BasketIElements = basketElements;
            }
            GlobalVariables.counter = GlobalVariables.counter + 1;
            return RedirectToAction("Index", "Home", new {id = GlobalVariables.LoginUser.Id});
        }


        public ActionResult Basket()
        {
            return View();
        }

        public ActionResult ConfirmOrder()
        {
            List<orderItem> lista = new List<orderItem>();
            foreach (var VARIABLE in GlobalVariables.MyBasket.BasketIElements)
            {
                orderItem ordItem = new orderItem();

                ordItem.ItemId = VARIABLE.Item.ItemId;
                ordItem.quantity = VARIABLE.Quantity;
                lista.Add(ordItem);
            }

            var ord = db.orders.Create();
            var user = new User
            {
                Address = GlobalVariables.LoginUser.Address,
                Email = GlobalVariables.LoginUser.Email,
                Name = GlobalVariables.LoginUser.Name,
                Id = GlobalVariables.LoginUser.Id,
                Password = GlobalVariables.LoginUser.Password,
                Phone = GlobalVariables.LoginUser.Phone
            };
            
            ord.Id = user.Id;
            ord.orderItems = lista;
            db.orders.Add(ord);
            
            db.SaveChanges();
            return View();
        }
    }
}