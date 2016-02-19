using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShopv2.Models;
using System.Web.Script.Serialization;

namespace OnlineShopv2.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[Common.CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult DeleteAll()
        {
            Session[Common.CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessioncart = (List<CartItem>)Session[Common.CommonConstants.CartSession];
            sessioncart.RemoveAll(x => x.Product.ID == id);
            Session[Common.CommonConstants.CartSession] = sessioncart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[Common.CommonConstants.CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[Common.CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().GetByID(productId);
            var cart = Session[Common.CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tao moi 1 doi tuong CartItem
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                Session[Common.CommonConstants.CartSession] = list;
            }
            else
            {
                //tao moi 1 doi tuong CartItem
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gan vao session
                Session[Common.CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[Common.CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName,string address,string phoneNumber,string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = phoneNumber;
            order.ShipName = shipName;
            order.ShipEmail = email;

            

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[Common.CommonConstants.CartSession];
                var detailDao = new OrderDetailDao();
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                }
                
            }
            catch (Exception ex)
            {
                return Redirect("/loi-gui-don-hang");
            }
            return Redirect("/hoan-thanh");
            
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}