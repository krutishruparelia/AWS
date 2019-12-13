using AWS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AWS.Areas.Admin.Controllers
{
    public class StationController : Controller
    {
        AWSDatabaseContext db = new AWSDatabaseContext();
        clsDatabase dbs = new clsDatabase();
        // GET: Admin/Station
        public ActionResult Index()
        {
            List<stationprofile> lstprof = new List<stationprofile>();
            var query = db.tbl_ProfileMaster.ToList();
            foreach (var item in query)
            {
                lstprof.Add(new stationprofile
                {
                    name = item.Name,
                    value = item.Name
                });
            }
            ViewBag.stprofile = lstprof;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string stationid, String stationname, string latitude, string longnitude, string city, string state, string tahesil, string Block, string village, string address, string bank, string busstand, string airport, string otherinfo, string image, HttpPostedFileBase photo)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            string createQuery = string.Empty;
            string getgain = "";
            string getoffset = "";
            string getserialNumber = "";
            ViewBag.Image = "[No photo]";
            if (photo != null)
            {
                SaveFile(photo);
            }
            tbl_StationMaster stationmaster = new tbl_StationMaster();
            var profile = Session["dropdownvalue"].ToString();
            stationmaster.StationID = stationid;
            stationmaster.Name = stationname;
            stationmaster.Latitude = latitude;
            stationmaster.Longitude = longnitude;
            stationmaster.City = city;
            stationmaster.State = state;
            stationmaster.TehsilTaluk = tahesil;
            stationmaster.Block = Block;
            stationmaster.Village = village;
            stationmaster.Address = address;
            stationmaster.Bank = bank;
            stationmaster.BusStand = busstand;
            stationmaster.Airport = airport;
            stationmaster.OtherInformation = otherinfo;
            stationmaster.Image = ViewBag.Image;
            stationmaster.Profile = profile;
            var sql = db.tbl_StationConfigTemp.Where(x => x.UserID == userid.ToString()).ToList();
            ///Get Gain of all sensors
            foreach (var gainitem in sql)
            {
                if (gainitem.Gain == null)
                {
                    getgain += "null,";
                }
                else
                {
                    getgain += gainitem.Gain + ",";
                }
            }
            var gain = getgain.TrimEnd(',');
            ///Get offset of all sensors
            foreach (var offsetitem in sql)
            {
                if (offsetitem.Offset == null)
                {
                    getoffset += "null,";
                }
                else
                {
                    getoffset += offsetitem.Offset + ",";
                }
            }
            var offset = getoffset.TrimEnd(',');
            ///Get serialnumber of all sensors
            foreach (var serialnumber in sql)
            {
                if (serialnumber.SerialNumber == null)
                {
                    getserialNumber += "null,";
                }
                else
                {
                    getserialNumber += serialnumber.SerialNumber + ",";
                }
            }
            var serialNumber = getserialNumber.TrimEnd(',');
            stationmaster.Gain =gain;
            stationmaster.Offset = offset;
            stationmaster.SerialNumber = serialNumber;
            var modelSation = db.tbl_StationMaster;
            modelSation.Add(stationmaster);
            db.SaveChanges();
            var sensorNamesql = db.tbl_ProfileMaster.Where(x => x.Name == profile).FirstOrDefault();
            string[] splitSensornameSql = sensorNamesql.SensorID.Split(',');
            foreach (var data in splitSensornameSql)
            {
                createQuery += "[" + data + "]" + " varchar(500),";
            }
            var finalParameterquery = createQuery.TrimEnd(',');
            var querySql = "Create table tbl_StationData_" + stationid + "(ID int not null identity(1,1)," + finalParameterquery + ")";
            dbs.CreateTable(querySql, "WEB");
            return View();
        }
        void SaveFile(HttpPostedFileBase file)
        {
            var targetLocation = Server.MapPath("~/Images/");

            try
            {
                var path = Path.Combine(targetLocation, file.FileName);
                ViewBag.Image = path;
                //Uncomment to save the file
                file.SaveAs(path);
            }
            catch
            {
                Response.StatusCode = 400;
            }
        }
        public JsonResult getDrodownvalue(string value)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            var checktemp = db.tbl_StationConfigTemp.Where(x => x.UserID == userid.ToString());
            if (checktemp.Count() == 0)
            {
                var data = "";
                var profile = db.tbl_ProfileMaster.Where(X => X.Name == value).ToList();
                foreach (var item in profile)
                {
                    data = item.SensorID;
                }
                string[] final_data = data.Split(',');
                tbl_StationConfigTemp stTemp = new tbl_StationConfigTemp();
                List<tbl_StationConfigTemp> lstSttiontemp = new List<tbl_StationConfigTemp>();
                for (int i = 0; i <= final_data.Length - 1; i++)
                {
                    stTemp.SensorName = final_data[i];
                    stTemp.Gain = null;
                    stTemp.Offset = null;
                    stTemp.SerialNumber = null;
                    stTemp.UserID = userid.ToString();
                    var model = db.tbl_StationConfigTemp;
                    model.Add(stTemp);
                    db.SaveChanges();
                }

                Session.Add("gridvalue", data);
                Session.Add("dropdownvalue", value);
                return Json(stTemp, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = db.tbl_StationConfigTemp;
                var item1 = model.Where(x => x.UserID == userid.ToString()).ToList();
                foreach (var deleteitem in item1)
                {
                    var getData = model.FirstOrDefault(it => it.ID == deleteitem.ID);
                    model.Remove(getData);
                    db.SaveChanges();
                }
                var data = "";
                var profile = db.tbl_ProfileMaster.Where(X => X.Name == value).ToList();
                foreach (var item in profile)
                {
                    data = item.SensorID;
                }
                string[] final_data = data.Split(',');
                tbl_StationConfigTemp stTemp = new tbl_StationConfigTemp();
                List<tbl_StationConfigTemp> lstSttiontemp = new List<tbl_StationConfigTemp>();
                for (int i = 0; i <= final_data.Length - 1; i++)
                {
                    stTemp.SensorName = final_data[i];
                    stTemp.Gain = null;
                    stTemp.Offset = null;
                    stTemp.SerialNumber = null;
                    stTemp.UserID = userid.ToString();
                    var model1 = db.tbl_StationConfigTemp;
                    model1.Add(stTemp);
                    db.SaveChanges();
                }

                Session.Add("gridvalue", data);
                Session.Add("dropdownvalue", value);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
         
        }
        public ActionResult GridViewPartial()
        {
            int userid = Convert.ToInt32(Session["userid"]);
            List<tbl_StationConfigTemp> lstSttiontemp = new List<tbl_StationConfigTemp>();
            var query = db.tbl_StationConfigTemp.OrderBy(t => t.ID);
            var allButTheLastTwoElements = query.Take(query.Count() - 1);
            var stParam = allButTheLastTwoElements.Skip(3).ToList();
            foreach (var item in stParam)
            {
                lstSttiontemp.Add(new tbl_StationConfigTemp
                {
                    ID = item.ID,
                    SensorName = item.SensorName,
                    Gain = item.Gain,
                    Offset = item.Offset,
                    SerialNumber = item.SerialNumber,
                    UserID = item.UserID
                });
            }
            ViewBag.stparam = lstSttiontemp;
            return PartialView("GridViewPartial");
        }
        public void Update(List<String> newdata, List<String> olddata)
        {
            int userid = Convert.ToInt32(Session["userid"]);
            tbl_StationConfigTemp stTemp = new tbl_StationConfigTemp();
            string newcombindedString = string.Join(",", newdata.ToArray());
            dynamic data = JObject.Parse(newcombindedString);
            string oldcombindedString = string.Join(",", olddata.ToArray());
            dynamic Olddata = JObject.Parse(oldcombindedString);
            stTemp.Gain = data.Gain == null ? Olddata.Gain : data.Gain;
            stTemp.Offset = data.Offset == null ? Olddata.Offset : data.Offset;
            stTemp.SerialNumber = data.SerialNumber == null ? Olddata.SerialNumber : data.SerialNumber;
            stTemp.ID = Olddata.ID;
            stTemp.UserID = userid.ToString();
            stTemp.SensorName = Olddata.SensorName;
            db.Entry(stTemp).State = EntityState.Modified;
            // this.UpdateModel(modelItem);
            db.SaveChanges();
        }
    }
}