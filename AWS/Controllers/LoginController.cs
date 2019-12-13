using AWS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AWS.Controllers
{
    public class LoginController : Controller
    {
        AWSDatabaseContext db = new AWSDatabaseContext();
        // GET: Login
        public ActionResult Index()
        {
            List<map> lstmap = new List<map>();
            var query = db.tbl_StationMaster.ToList();
            foreach (var item in query)
            {
                lstmap.Add(new map
                {
                    location = item.Latitude + "," + item.Longitude,
                    tooltip = item.Name
                });

            }
            ViewBag.MapData = lstmap;
            return View();
        }
        [HttpPost]
        public ActionResult Index(tbl_User userClass)
        {
            var username = userClass.Username;
            var password = userClass.Password;
            var userDetails = db.tbl_User.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (userDetails != null)
            {
                bool? checkAdmin = userDetails.IsAdmin;
                if (checkAdmin == true)
                {
                    var getUserid = db.tbl_User.Where(x => x.Username == username).FirstOrDefault();
                    Session.Add("userid", getUserid.ID);
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    var getUserid = db.tbl_User.Where(x => x.Username == username).FirstOrDefault();
                    Session.Add("name", getUserid.FirstName + " " + getUserid.LastName);
                    
                    Session.Add("U_userid", getUserid.ID);
                    return RedirectToAction("Index", "Station", new { area = "User" });
                }
            }
            else
            {
                return View();
            }
           
        }
        public ActionResult Logout()
        {
            Session.Remove("userid");
            return RedirectToAction("Index");
        }
        public ActionResult UserLogout()
        {
            Session.Remove("U_userid");
            return RedirectToAction("Index");
        }
    }
}