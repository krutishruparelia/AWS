using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWS.Areas.Admin.Controllers
{
    public class SiteSurveyController : Controller
    {
        // GET: Admin/SiteSurvey
        public ActionResult Index()
        {
            return View();
        }
    }
}