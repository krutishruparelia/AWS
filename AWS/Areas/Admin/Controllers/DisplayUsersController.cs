using AWS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AWS.Areas.Admin.Controllers
{
    public class DisplayUsersController : Controller
    {
        AWSDatabaseContext db = new AWSDatabaseContext();
        // GET: Admin/DisplayUsers
        public ActionResult Index()
        {

            List<tbl_User> lstuser = new List<tbl_User>();
            List<tbl_Role> lstRole = new List<tbl_Role>();
            var sqlQuery = db.tbl_User.Where(x => x.IsDeleted == false).ToList();
            var getRole = db.tbl_Role.ToList();
            foreach (var roleitemn in getRole)
            {
                lstRole.Add(new tbl_Role
                {
                    ID = roleitemn.ID,
                    RoleName = roleitemn.RoleName

                });
            }
            foreach (var item in sqlQuery)
            {
                lstuser.Add(new tbl_User
                {
                    ID = item.ID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    EmailID = item.EmailID,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    Username = item.Username,
                    Password = item.Password,
                    IsActive = item.IsActive,
                    IsAdmin = item.IsAdmin,
                    RoleID = item.RoleID,
                    RoleName = item.RoleName
                });

            }
            ViewBag.UserData = lstuser;
            ViewBag.lkpRole = lstRole;
            return View();

        }

        public void Insert(List<String> value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_User user = new tbl_User();
            string newcombindedString = string.Join(",", value.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.EmailID = data.EmailID;
            user.PhoneNumber = data.PhoneNumber;
            user.Address = data.Address;
            user.Username = data.Username;
            user.Password = data.Password;
            user.IsActive = data.IsActive;
            user.IsAdmin = data.IsAdmin;
            user.RoleID = data.RoleID;
            user.CreatedBy = userid;
            user.IsDeleted = false;
            user.UpdatedBy = userid;
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            var model = db.tbl_User;
            model.Add(user);
            db.SaveChanges();

        }
        public ActionResult Update(List<String> newdata, List<String> olddata)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_User user = new tbl_User();
            string newcombindedString = string.Join(",", newdata.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            string oldcombindedString = string.Join(",", olddata.ToArray());
            dynamic Olddata = JObject.Parse(oldcombindedString);
            user.ID = Olddata.ID;
            user.FirstName = data.FirstName == null ? Olddata.FirstName : data.FirstName;
            user.LastName = data.LastName == null ? Olddata.LastName : data.LastName;
            user.EmailID = data.EmailID == null ? Olddata.EmailID : data.EmailID;
            user.PhoneNumber = data.PhoneNumber == null ? Olddata.PhoneNumber : data.PhoneNumber;
            user.Address = data.Address == null ? Olddata.Address : data.Address;
            user.Username = data.Username == null ? Olddata.Username : data.Username;
            user.Password = data.Password == null ? Olddata.Password : data.Password;
            user.IsActive = data.IsActive == null ? Olddata.IsActive : data.IsActive;
            user.IsAdmin = data.IsAdmin == null ? Olddata.IsAdmin : data.IsAdmin;
            user.RoleID = data.RoleID == null ? Olddata.RoleID : data.RoleID;
            user.CreatedBy = userid;
            user.IsDeleted = false;
            user.UpdatedBy = userid;
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;

            // this.UpdateModel(modelItem);
            db.SaveChanges();
            return View();
        }
    }
}