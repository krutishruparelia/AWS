using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWS.Models;
using Newtonsoft.Json.Linq;
using System.Data.Entity;

namespace AWS.Areas.Admin.Controllers
{
    public class ParameterController : Controller
    {
        // GET: Admin/Parameter
        AWSDatabaseContext db = new AWSDatabaseContext();
        public ActionResult Index()
        {
            List<tbl_ParameterMaster> lstParameter = new List<tbl_ParameterMaster>();
            List<tbl_SensorMaster> lstsensor = new List<tbl_SensorMaster>();

            var sqlQuery = db.tbl_ParameterMaster.Where(x => x.IsDeleted == false).ToList();
            var sensorqry = db.tbl_SensorMaster.ToList();
            foreach (var item in sqlQuery)
            {
                lstParameter.Add(new tbl_ParameterMaster
                {
                    ID = item.ID,
                    Name = item.Name,
                    SensorID = item.SensorID,
                    MinimumRange = item.MinimumRange,
                    MaximumRange=item.MaximumRange,
                    Unit=item.Unit
                });
            }
            foreach (var item1 in sensorqry)
            {
                lstsensor.Add(new tbl_SensorMaster
                {
                    ID = item1.ID,
                    Name = item1.Name,
                });
            }
            ViewBag.Parameterdata = lstParameter;
            ViewBag.Sensordata = lstsensor;

            return View();
        }
        public ActionResult Update(List<String> newdata, List<String> olddata, tbl_SensorMaster parameter1)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_ParameterMaster parameter = new tbl_ParameterMaster();
            string newcombindedString = string.Join(",", newdata.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            string oldcombindedString = string.Join(",", olddata.ToArray());
            dynamic Olddata = JObject.Parse(oldcombindedString);
            parameter.ID = Olddata.ID;
            parameter.Name = data.Name == null ? Olddata.Name : data.Name;
            parameter.SensorID = data.SensorID == null ? Olddata.SensorID : data.SensorID;
            parameter.MinimumRange = data.MinimumRange == null ? Olddata.MinimumRange : data.MinimumRange;
            parameter.MaximumRange = data.MaximumRange == null ? Olddata.MaximumRange : data.MaximumRange;
            parameter.Unit = data.Unit == null ? Olddata.Unit : data.Unit;
            parameter.CreatedDate = DateTime.Now;
            parameter.CreatedBy = userid;
            parameter.UpdatedDate = DateTime.Now;
            parameter.UpdatedBy = userid;
            parameter.IsDeleted = false;
            db.Entry(parameter).State = EntityState.Modified;
            
            // this.UpdateModel(modelItem);
            db.SaveChanges();
            return View();
        }
        public void Insert(List<String> value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_ParameterMaster parameter = new tbl_ParameterMaster();
            string newcombindedString = string.Join(",", value.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            parameter.Name = data.Name;
            parameter.SensorID = data.SensorID;
            parameter.MaximumRange = data.MaximumRange;
            parameter.MinimumRange = data.MinimumRange;
            parameter.Unit = data.Unit;
            parameter.CreatedBy = userid;
            parameter.IsDeleted = false;
            parameter.UpdatedBy = userid;
            parameter.CreatedDate = DateTime.Now;
            parameter.UpdatedDate = DateTime.Now;
            var model = db.tbl_ParameterMaster;
            model.Add(parameter);
            db.SaveChanges();
        }
        public void Remove(int ID)
        {
            var model = db.tbl_ParameterMaster;
            var item1 = model.FirstOrDefault(it => it.ID == ID);
            model.Remove(item1);
            db.SaveChanges();
        }
    }
}