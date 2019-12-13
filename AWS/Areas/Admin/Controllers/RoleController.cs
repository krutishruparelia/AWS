using AWS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;

namespace AWS.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        AWSDatabaseContext db = new AWSDatabaseContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            if (Session["userid"] != null)
            {
                List<RoleData> lstRole = new List<RoleData>();
                var sqlQuery = db.tbl_Role.Where(x => x.IsDeleted == false).ToList();
                foreach (var item in sqlQuery)
                {
                    lstRole.Add(new RoleData
                    {
                        ID = item.ID,
                        RoleName = item.RoleName,
                        Code = item.Code
                    });

                }
                ViewBag.RoleData = lstRole;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult Update(List<String> newdata, List<String> olddata,tbl_Role role1)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_Role role = new tbl_Role();
            string newcombindedString = string.Join(",", newdata.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            string oldcombindedString = string.Join(",", olddata.ToArray());
            dynamic Olddata = JObject.Parse(oldcombindedString);
            if(newcombindedString.Contains("RoleName"))
            {
                role.RoleName = data.RoleName;
            }
            else
            {
                role.RoleName = Olddata.RoleName;
            }
            if(newcombindedString.Contains("Code"))
            {
                role.Code = data.Code;
            }
            else
            {
                role.Code = Olddata.Code;
            }
            role.Code = data.Code;
            role.ID = Olddata.ID;
            role.RoleName = data.RoleName;
            role.CreatedBy = userid;
            role.IsDeleted = false;
            role.UpdatedBy = userid;
            role.CreatedDate = DateTime.Now;
            role.UpdatedDate = DateTime.Now;
            db.Entry(role).State= EntityState.Modified;

            // this.UpdateModel(modelItem);
            db.SaveChanges();
            return View();
        }
        public void Insert(List<String> value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_Role role = new tbl_Role();
            string newcombindedString = string.Join(",", value.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            role.Code = data.Code;
            role.RoleName = data.RoleName;
            role.CreatedBy = userid;
            role.IsDeleted = false;
            role.UpdatedBy = userid;
            role.CreatedDate = DateTime.Now;
            role.UpdatedDate = DateTime.Now;
            var model = db.tbl_Role;
            model.Add(role);
            db.SaveChanges();
        }
        public void Remove(int ID)
        {
            var model = db.tbl_Role;
            var item1 = model.FirstOrDefault(it => it.ID == ID);
            model.Remove(item1);
            db.SaveChanges();
        }
    }
}