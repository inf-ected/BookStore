﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        public ActionResult Index()
        {
            return View();
        }


        public string Test() { return "test"; }
    }
}