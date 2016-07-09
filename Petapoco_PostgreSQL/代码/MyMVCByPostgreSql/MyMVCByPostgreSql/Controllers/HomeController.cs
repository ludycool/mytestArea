using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMVCByPostgreSql.Controllers
{
    public class HomeController : Controller
    {

        Database db = new PetaPoco.Database("Postgresql");


        public ActionResult Index()
        {
            ViewData.Model = db.Query<UserInfo>("select * from userinfo");
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            ViewData.Model = db.SingleOrDefault<UserInfo>("where id = @0", id);
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(UserInfo userinfo)
        {
            try
            {
                // TODO: Add insert logic here
                db.Insert(userinfo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            ViewData.Model = db.SingleOrDefault<UserInfo>("where id=@0",id);
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(UserInfo userinfo)
        {
            try
            {
                // TODO: Add update logic here
                db.Update(userinfo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            ViewData.Model = db.SingleOrDefault<UserInfo>("where id=@0",id);
            return View();
        }

        //
        // POST: /Home/Delete/5

        [HttpPost]
        public ActionResult Delete(UserInfo userinfo)
        {
            try
            {
                // TODO: Add delete logic here
                db.Delete(userinfo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
