
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using market.Models;
using System.Data.SqlClient;

namespace market.Controllers
{
    public class AdminController : Controller
    {
        public DataBase db = new  DataBase();
        // GET: Admin
        public class myViewModel
        {
            public User adminUser { get; set; }
            public List<Item> AllItems { get; set; }
        }



        public ActionResult Index(int? id)
        {
                ViewBag.id = id;
                
                var user = db.Users.First(item => item.Id == id);
                
                myViewModel viewModel = new myViewModel
                {
                    AllItems = db.Items.ToList(),
                    adminUser = user
                };
                return View(viewModel);
            
           
        }


        public ActionResult AddItem(User user)
        {
            return View(user);
        }



        [HttpPost]
        public ActionResult AddItem(Item item , String Id, HttpPostedFileBase file)
        {
            var intId = Int32.Parse(Id);
            
            var theAdmin =db.Users.FirstOrDefault(num => num.Id == intId);
            


            
            if (item.ItemName == null || item.Price == null || item.quantity==null)
            {
                ViewBag.itemError = "Please enter all fields !!";
                return View(theAdmin);
            }
            else
            {

                if (file != null)
                {
                    item.img = new byte[file.ContentLength];
                    file.InputStream.Read(item.img, 0, file.ContentLength);
                }
                if (db.Items.ToList().FirstOrDefault(num => num.ItemName == item.ItemName) != null) 
                {
                    ViewBag.itemError = "this name is already exist";
                    return View(theAdmin);
                }
                else
                {
                    var q2 = "INSERT INTO Items(img,Price,quantity,ItemName) VALUES (@img,@price,@quantity,@itemName)";
                    SqlCommand cmd = new SqlCommand(q2);
                    
                    if (item.img == null)
                    {
                        cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("img", item.img);
                    }
                    
                    cmd.Parameters.AddWithValue("price", item.Price);
                    
                    cmd.Parameters.AddWithValue("quantity", item.quantity);
                    cmd.Parameters.AddWithValue("itemName", item.ItemName);
                    SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-0B4I9QK\SQLEXPRESS;Initial Catalog=Market;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
                    try
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //var q2 = "INSERT INTO Items(img,Price,quantity,ItemName) VALUES ("+@item.img.ToString()+"," + item.Price + "," + item.quantity  + ","+"N'" + item.ItemName + "')";
                    //var q = "INSERT INTO Items(ItemName,Price,quantity,img) VALUES (N'"+ item.ItemName+"',"+item.Price+","+item.quantity+","+item.img+")";
                    db.SaveChanges();
                    
                    return RedirectToAction("Index", new RouteValueDictionary(
                        new { controller = "Admin", action = "Index", id = intId }));
                }
            }
           
           
        }


        public ActionResult DeleteItem(int id , int userID)
        {
            var deletedItem= Find(id);
           
            db.Entry(deletedItem).State = EntityState.Deleted;
            db.Items.Remove(deletedItem);
            //db.Items.ToList().Remove(deletedItem);
            db.SaveChanges();
            return   RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "Admin", action = "Index", id = userID }));
        }

        public ActionResult Edit( int id )
        {
            var editItem =db.Items.FirstOrDefault(item => item.ItemId == id);
            if (editItem != null)
            {
                TempData["itemId"] = id;
                TempData["image"] = editItem.img;
                TempData.Keep();
                return View(editItem);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Item item,HttpPostedFileBase file)
        {
            Int32 id = (int)TempData["itemId"];
            if (file != null)
            {
                item.img = new byte[file.ContentLength];
                file.InputStream.Read(item.img, 0, file.ContentLength);
            }
            else
            {
                item.img = (byte[]) TempData["image"];
            }
            
            var q2 = "UPDATE Items SET ItemName = @name, Price = @price, quantity=@quantity, img=@img  WHERE ItemId = @id ";

            SqlCommand cmd = new SqlCommand(q2);
            cmd.Parameters.AddWithValue("name", item.ItemName);
            cmd.Parameters.AddWithValue("price", item.Price);
            cmd.Parameters.AddWithValue("quantity", item.quantity);
            if (item.img == null)
            {
                cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.AddWithValue("img", item.img);
            }
            cmd.Parameters.AddWithValue("id", id);
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-0B4I9QK\SQLEXPRESS;Initial Catalog=Market;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return View();
            }
            db.SaveChanges();

            return RedirectToAction("Index", new RouteValueDictionary(
                new {controller = "Admin", action = "Index", id = 15}));
            
        }


        public ActionResult OrderList()
        {
            var dbOrders = db.orders.Where(i => i.respond == false);
            /*
            List<order> orders = new List<order>();
            order order = new order();
            
            var dbOrders = db.orders.Where(i => i.respond == false);
            
            foreach (var VARIABLE in dbOrders)
            {
                foreach (var VAR in VARIABLE.orderItems)
                {
                   var items = db.Items.FirstOrDefault(i => i.ItemId == VAR.ItemId);
                   VAR.Item = items;
                }
                
            }
            */
            return View(dbOrders);
        }
        public Item Find (int id)
        {
            return db.Items.FirstOrDefault(num => num.ItemId == id);
        }
         

    }

    
}