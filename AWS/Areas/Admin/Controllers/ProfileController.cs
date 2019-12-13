using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWS.Models;
using Newtonsoft.Json.Linq;

namespace AWS.Areas.Admin.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Admin/Profile
        AWSDatabaseContext db = new AWSDatabaseContext();
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["userid"]);
            List<tbl_ParameterMaster> lstParameter = new List<tbl_ParameterMaster>();
            List<tbl_SensorMaster> lstSensor = new List<tbl_SensorMaster>();
            List<tbl_SensorMaster> lstLookup = new List<tbl_SensorMaster>();
            var sqlQuery = db.tbl_ParameterMaster.Where(x => x.IsDeleted == false).ToList();
            var sqlSensor = db.tbl_ProfileTemp.Where(x => x.UserID == id).ToList();
            var sqlSensorloookup = db.tbl_SensorMaster.ToList();
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
            foreach (var item in sqlSensor)
            {
                lstSensor.Add(new tbl_SensorMaster
                {
                    ID = item.ID,
                    Name = item.Name,
                    Type = item.Make,
                    Model = item.Model
                });
            }
            foreach (var item1 in sqlSensorloookup)
            {
                lstLookup.Add(new tbl_SensorMaster
                {
                    ID = item1.ID,
                    Name = item1.Name,
                    Type = item1.Make,
                    Model = item1.Model
                });
            }
            ViewBag.Parameterdata = lstParameter;
            ViewBag.Sensordata = lstSensor;
            ViewBag.SensordataLookup = lstLookup;
            return View();
        }
        public void InsertTemp(List<String> value)
        {

            //int userid = Convert.ToInt32(Session["userid"]);
            //tbl_ProfileTemp profileTemp = new tbl_ProfileTemp();

            //string newcombindedString = string.Join(",", value.ToArray());
            //dynamic data = JObject.Parse(newcombindedString);
            //int sensorID = Convert.ToInt32(data.Name);
            //var getsensortdata = db.tbl_SensorMaster.Where(j => j.ID == sensorID).FirstOrDefault();
            //profileTemp.Name = getsensortdata.Name;
            ////profileTemp.UserID = userid;
            ////profile.Name = data.Name;
            ////profile.SensorID = data.Name;
            //profileTemp.Delimiter = data.Delimiter;
            //profileTemp.DateFormat = data.DateFormat;
            //profileTemp.CreatedBy = userid;
            //profileTemp.IsDeleted = false;
            //profileTemp.UpdatedBy = userid;
            //profileTemp.CreatedDate = DateTime.Now;
            //profileTemp.UpdatedDate = DateTime.Now;
            //var model = db.tbl_ProfileTemp;
            //model.Add(profileTemp);
            //db.SaveChanges();
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_ProfileTemp profiletemp = new tbl_ProfileTemp();
            string newcombindedString = string.Join(",", value.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            profiletemp.Name = data.Name;
            profiletemp.Type = data.Type;
            profiletemp.Make = data.Make;
            profiletemp.Model = data.model;
            profiletemp.Description = data.Description;
            profiletemp.UserID = userid;

            profiletemp.CreatedBy = userid;
            profiletemp.IsDeleted = false;
            profiletemp.UpdatedBy = userid;
            profiletemp.CreatedDate = DateTime.Now;
            profiletemp.UpdatedDate = DateTime.Now;
            var model = db.tbl_ProfileTemp;
            model.Add(profiletemp);
            db.SaveChanges();

        }
        public void Insert(string value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_ProfileMaster profile = new tbl_ProfileMaster();
            var getData = db.tbl_ProfileMaster.Where(x => x.CreatedBy == userid).ToList();
            // string name = string.Join(",", getData.SensorID);
            //var name = getData.SensorID;  var model = db.tbl_ProfileMaster;
            var model = db.tbl_ProfileMaster;
            foreach (var item in getData)
            {
                //profile.SensorID = getData.SensorID + ",";
                profile.SensorID += item.SensorID+ ",";
            }
            profile.SensorID.TrimEnd(',');
            profile.CreatedBy = userid;
            profile.IsDeleted = false;
            profile.UpdatedBy = userid;
            profile.CreatedDate = DateTime.Now;
            profile.UpdatedDate = DateTime.Now;
            model = db.tbl_ProfileMaster;
            model.Add(profile);
            db.SaveChanges();
            //model = db.tbl_ProfileMaster;
            var item1 = model.FirstOrDefault(it => it.UpdatedBy == userid);
            model.Remove(item1);
            db.SaveChanges();
        }
        public ActionResult insertProfileName(string profilename, string deli, string dateformat)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            var model = db.tbl_ProfileMaster;
            tbl_ProfileMaster profile = new tbl_ProfileMaster();
            var getsensorNamesql = db.tbl_ProfileTemp.Where(x => x.UserID == userid).ToList();
            string datajoin = "";
            string firstStr = "StationID,Date,Time,";
            string lstStr = "PeripheralStatus";
            datajoin = firstStr;
            foreach (var item in getsensorNamesql)
            {
                datajoin += item.Name + ",";
            }
            datajoin += lstStr;
            profile.SensorID = datajoin.TrimEnd(',');
            profile.Name = profilename;
            profile.Delimiter = deli;
            profile.IsDeleted = false;
            profile.DateFormat = dateformat;
            profile.CreatedDate = DateTime.Now;
            profile.CreatedBy = userid;
            profile.UpdatedDate = DateTime.Now;
            profile.UpdatedBy = userid;
            model.Add(profile);
            var deleteModel = db.tbl_ProfileTemp;
            
            var item1 = deleteModel.Where(it => it.UserID == userid).ToList();
            foreach (var item2 in item1)
            {
                deleteModel.Remove(item2);
                db.SaveChanges();
            }
            
            TempData["Added"] = "done";
            return Json("True", JsonRequestBehavior.AllowGet);
        }
        public void Remove(int ID)
        {
            var model = db.tbl_ProfileTemp;
            var item1 = model.FirstOrDefault(it => it.ID ==  ID);
            model.Remove(item1);
            db.SaveChanges();
        }
    }
}