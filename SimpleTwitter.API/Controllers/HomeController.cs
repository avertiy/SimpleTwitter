using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleTwitter.API.WcfCommandServiceReference;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.ReadSide.Data;
using StackExchange.Redis;

namespace SimpleTwitter.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult SeedData(bool flush=false,bool seedanyway = false)
        {
            try
            {
                SeedHelper.Seed(flush,seedanyway);
                return Content("Seed cuccess");
            }
            catch (Exception ex)
            {
                return Content(ex.InnerException.Message);
            }
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult List()
        {
            return PartialView();
        }
        public ActionResult Edit()
        {
            return PartialView();
        }
        public ActionResult Detail()
        {
            return PartialView();
        }
	}
}