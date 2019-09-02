using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FIlmApp.Models;

namespace FIlmApp.Controllers
{
    public class filmsController : Controller
    {
        private FilmsDBEntities db = new FilmsDBEntities();

        // GET: films
        public ActionResult Index()
        {
            return View(db.films.ToList());
        }

        // GET: films/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            film film = db.films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // GET: films/Create
        public ActionResult Create()
        {
            return View();
        }

       
        
        // POST: films/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FilmName,Desctiption,Director,Creator,Year,ImageUrl")] film film, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                // получаем имя файла
               
                string fileName = System.IO.Path.GetFileName(Image.FileName);
                fileName = String.Format(@"{0}", System.Guid.NewGuid()) + Path.GetExtension(Image.FileName);

                // сохраняем файл в папку Files в проекте
                Image.SaveAs(Server.MapPath("~/Files/" + fileName));
                film.ImageUrl = "~/Files/" + fileName;
            }
            if (ModelState.IsValid)
            {
                
                db.films.Add(film);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(film);
        }

        // GET: films/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            film film = db.films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: films/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FilmName,Desctiption,Director,Creator,Year,ImageUrl")] film film)
        {   
            if (ModelState.IsValid)
            {
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        // GET: films/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            film film = db.films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            film film = db.films.Find(id);
            db.films.Remove(film);
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
