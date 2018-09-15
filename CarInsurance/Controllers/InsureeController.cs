using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                //Setting initial quote to a base of 50.
                insuree.Quote = 50.00m;

                //Age Logic (younger than 18 add 100, younger than 25 add 25, older than 100 add 25)
                int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
                if (age < 18)
                {
                    insuree.Quote = insuree.Quote + 100m;
                }
                else if (age < 25 || age > 100)
                {
                    insuree.Quote = insuree.Quote + 25m;
                }

                //Car Year Logic (older than 2000 add 25, newer than 2015 add 25)
                if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
                {
                    insuree.Quote = insuree.Quote + 25m;
                }

                //Make/Model check (if make is porsche add 25 and if model a 911 Carrera add 25)
                if (insuree.CarMake.ToLower() == "porsche")
                {
                    insuree.Quote = insuree.Quote + 25m;
                    if (insuree.CarModel.ToLower() =="911 carrera")
                    {
                        insuree.Quote = insuree.Quote + 25m;
                    }
                }

                //Speeding Tickets (For ever speeding ticket add 10)
                
                for (int i = 0; i < insuree.SpeedingTickets; i++)
                {
                    insuree.Quote = insuree.Quote + 10m;
                }

                //DUI (if history of DUI[bool] add 25%)

                if (insuree.DUI == true)
                {
                    insuree.Quote = (.25m * insuree.Quote) + insuree.Quote;
                }

                //If full coverage [bool] add %50
                if (insuree.CoverageType == true)
                {
                    insuree.Quote = (.50m * insuree.Quote) + insuree.Quote;
                }



                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
