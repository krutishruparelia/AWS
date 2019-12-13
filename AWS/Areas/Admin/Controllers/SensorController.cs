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
    public class SensorController : Controller
    {
        // GET: Admin/Sensor
        AWSDatabaseContext db = new AWSDatabaseContext();
        public ActionResult Index()
        {
            List<tbl_SensorMaster> lstSensor = new List<tbl_SensorMaster>();
            var sqlQuery = db.tbl_SensorMaster.Where(x => x.IsDeleted == false).ToList();
            
            foreach (var item in sqlQuery)
            {
                lstSensor.Add(new tbl_SensorMaster
                {
                    ID = item.ID,
                    Name = item.Name,
                    Type = item.Type,
                    Make = item.Make,
                    Model = item.Model
                });
            }
            ViewBag.Sensordata = lstSensor;
            return View();
        }
        public ActionResult Update(List<String> newdata, List<String> olddata, tbl_SensorMaster sensor1)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_SensorMaster sensor = new tbl_SensorMaster();
            string newcombindedString = string.Join(",", newdata.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            string oldcombindedString = string.Join(",", olddata.ToArray());
            dynamic Olddata = JObject.Parse(oldcombindedString);
            sensor.ID = Olddata.ID;
            sensor.Name = data.Name==null?Olddata.Name: data.Name;
            sensor.Type = data.Type == null ? Olddata.Type : data.Type;
            sensor.Make = data.Make == null ? Olddata.Make : data.Make;
            sensor.Model = data.Model == null ? Olddata.Model : data.Model;
            sensor.CreatedDate = DateTime.Now;
            sensor.CreatedBy = userid;
            sensor.UpdatedDate = DateTime.Now;
            sensor.UpdatedBy = userid;
            sensor.IsDeleted = false;
            db.Entry(sensor).State = EntityState.Modified;

            // this.UpdateModel(modelItem);
            db.SaveChanges();
            return View();
        }
        public void Insert(List<String> value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_SensorMaster sensor = new tbl_SensorMaster();
            string newcombindedString = string.Join(",", value.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            sensor.Name = data.Name;
            sensor.Type = data.Type;
            sensor.CreatedBy = userid;
            sensor.Make = data.Make;
            sensor.Model = data.Model;

            sensor.IsDeleted = false;
            sensor.UpdatedBy = userid;
            sensor.CreatedDate = DateTime.Now;
            sensor.UpdatedDate = DateTime.Now;
            var model = db.tbl_SensorMaster;
            model.Add(sensor);
            db.SaveChanges();
        }
        public void Remove(int ID)
        {
            var model = db.tbl_SensorMaster;
            var item1 = model.FirstOrDefault(it => it.ID == ID);
            model.Remove(item1);
            db.SaveChanges();
        }
    }
}