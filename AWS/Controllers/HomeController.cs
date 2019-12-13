using DevExpress.Web.Mvc;
using AWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWS.Controllers {
    public class HomeController : Controller {
        AWSDatabaseContext db = new AWSDatabaseContext();
        public ActionResult Index() {
           
            return View();
        }

        AWS.Models.AWSDatabaseContext db1 = new AWS.Models.AWSDatabaseContext();

        [ValidateInput(false)]
        public ActionResult demo1()
        {
            var model = db1.tbl_User;
            return PartialView("~/Areas/User/Views/User/_demo1.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult demo1AddNew([ModelBinder(typeof(DevExpressEditorsBinder))] AWS.Models.tbl_User item)
        {
            var model = db1.tbl_User;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Areas/User/Views/User/_demo1.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult demo1Update([ModelBinder(typeof(DevExpressEditorsBinder))] AWS.Models.tbl_User item)
        {
            var model = db1.tbl_User;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID == item.ID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Areas/User/Views/User/_demo1.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult demo1Delete(System.Int32 ID)
        {
            var model = db1.tbl_User;
            if (ID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID == ID);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Areas/User/Views/User/_demo1.cshtml", model.ToList());
        }

        public ActionResult ChartPartial1()
        {
            var model = new object[0];
            return PartialView("~/Areas/User/Views/User/_ChartPartial1.cshtml", model);
        }
    }
}