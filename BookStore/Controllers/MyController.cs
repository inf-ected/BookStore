using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class MyController : Controller
    {
        // GET: My
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public string Index()
        {
            return "INDEX of My controller";
        }

      //  [Route("123/")]
        public string Test() { return "test"; }
    }
}