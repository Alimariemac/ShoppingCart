﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalShoppingCart.Models;
using Microsoft.AspNet.Identity;

namespace FinalShoppingCart.Controllers
{
    public class OrdersController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }


        public ActionResult Completed(int? id)
        {
            if (id != null)

            {
                var userid = User.Identity.GetUserId();
                var shoppingcarts = db.ShoppingCarts.Where(s => s.CustomerId == userid);
                var completeOrder = db.Orders.Find(id);
                var orderDetail = completeOrder.OrderDetails.ToList();
                db.Orders.Remove(completeOrder);

                if (shoppingcarts != null)
                {
                    foreach (var i in shoppingcarts)
                    {
                        db.ShoppingCarts.Remove(i);
                    }
                    
                }

                db.SaveChanges();
            }

            return View();
        }

        /*private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }*/

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Address,City,State,Zipcode,Country,Phone")] Order order)

        {

                var user = db.Users.Find(User.Identity.GetUserId());
                var shoppingcart = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
                decimal totalAmt = 0;
                if (shoppingcart.Count != 0)

{

                    if (ModelState.IsValid)
                    {
                    foreach (var product in shoppingcart)
                    {
                    OrderDetail orderdetail = new OrderDetail();
                    orderdetail.ItemId = product.ItemId;
                    orderdetail.OrderId = order.Id;
                    orderdetail.Quantity += product.Count;
                    orderdetail.UnitPrice = product.Item.Price;
                    totalAmt += (product.Count* product.Item.Price);

                    db.OrderDetails.Add(orderdetail);
                    }
                    order.Total = totalAmt;
                    order.Completed = false;
                    order.OrderDate = DateTime.Now;
                    order.CustomerId = user.Id;
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    }
                }

                ViewBag.NoItem = "There's no item to order";;
                return View(order);

}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Completed,Address,City,State,Zipcode,Country,Phone,OrderDate,Total,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }*/

         /*GET: Orders/Edit/5*/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Completed,Address,City,State,Zipcode,Country,Phone,OrderDate,Total,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
            
        {
            var userid = User.Identity.GetUserId();
            var shoppingcarts = db.ShoppingCarts.Where(s => s.CustomerId == userid);
            foreach (var i in shoppingcarts) {
                db.ShoppingCarts.Remove(i);
            }

            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
