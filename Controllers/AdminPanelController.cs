﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        public ActionResult Home()
        {
            return View();
        }

        // GET: AdminPanel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminPanel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminPanel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminPanel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminPanel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminPanel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}