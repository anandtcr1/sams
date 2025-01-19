using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamsSite.Models;

namespace SamsSite.Views
{
    public class PropertiesController : Controller
    {
        // GET: Properties
        public ActionResult Index()
        {
            List<PropertyItem> propertyItemList = new List<PropertyItem>
            {
                new PropertyItem{ PropertyItemId=1, PropertyListingId="L001" },
                new PropertyItem{ PropertyItemId=2, PropertyListingId="L002" }
            };
            ViewData.Model = propertyItemList;
            return View();
        }

        public ActionResult SurplusProperties()
        {
            List<PropertyItem> propertyItemList = new List<PropertyItem>
            {
                new PropertyItem{ PropertyItemId=1, PropertyListingId="L001" },
                new PropertyItem{ PropertyItemId=2, PropertyListingId="L002" }
            };
            ViewData.Model = propertyItemList;
            return View();
        }

        public ActionResult NetLeaseProperties()
        {
            List<PropertyItem> propertyItemList = new List<PropertyItem>
            {
                new PropertyItem{ PropertyItemId=1, PropertyListingId="L001" },
                new PropertyItem{ PropertyItemId=2, PropertyListingId="L002" }
            };
            ViewData.Model = propertyItemList;
            return View();
        }

        public ActionResult C_StoreList()
        {
            List<PropertyItem> propertyItemList = new List<PropertyItem>
            {
                new PropertyItem{ PropertyItemId=1, PropertyListingId="L001" },
                new PropertyItem{ PropertyItemId=2, PropertyListingId="L002" }
            };
            ViewData.Model = propertyItemList;
            return View();
        }

        public ActionResult SubmitSite()
        {
            List<PropertyItem> propertyItemList = new List<PropertyItem>
            {
                new PropertyItem{ PropertyItemId=1, PropertyListingId="L001" },
                new PropertyItem{ PropertyItemId=2, PropertyListingId="L002" }
            };
            ViewData.Model = propertyItemList;
            return View();
        }

        // GET: Properties/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Properties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Properties/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Properties/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Properties/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}