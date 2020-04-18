using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using coursework02.Models;
using coursework02.DAL;

namespace coursework02.Controllers
{
    public class LoanTypesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: LoanTypes
        public ActionResult Index()
        {
            return View(db.LoanTypes.ToList());
        }

        // GET: LoanTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanType loanType = db.LoanTypes.Find(id);
            if (loanType == null)
            {
                return HttpNotFound();
            }
            return View(loanType);
        }

        // GET: LoanTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( LoanType loanType)
        {
            if (ModelState.IsValid)
            {
                db.LoanTypes.Add(loanType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loanType);
        }

        // GET: LoanTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanType loanType = db.LoanTypes.Find(id);
            if (loanType == null)
            {
                return HttpNotFound();
            }
            return View(loanType);
        }

        // POST: LoanTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] LoanType loanType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loanType);
        }

        // GET: LoanTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanType loanType = db.LoanTypes.Find(id);
            if (loanType == null)
            {
                return HttpNotFound();
            }
            return View(loanType);
        }

        // POST: LoanTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanType loanType = db.LoanTypes.Find(id);
            db.LoanTypes.Remove(loanType);
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
