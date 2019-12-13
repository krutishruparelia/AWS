using AWS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AWS.Areas.User.Controllers
{
    public class StationController : Controller
    {
        private clsDatabase adDB = new clsDatabase();
        AWSDatabaseContext db = new AWSDatabaseContext();
        private string json = "";
        private string yesjson = "";
        private string weeklyjson = "";
        private string monthlyjson = "";

        // GET: User/StationView
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StationView(string id)
        {
            Session.Add("StationID", id);
          
            List<ColumnNames> lstcol = new List<ColumnNames>();
            var tableName = "tbl_StationData_" + id;
            ///Fetching CoulmnName to display on grid header
            DataSet columnDataset = adDB.FetchData_SP_columnName("getColumnNameForGrid", tableName, "WEB");
            DataTable columnDataTable = new DataTable();
            string columnnames = "";
            if (columnDataset.Tables.Count != 0)
            {
                columnDataTable = columnDataset.Tables[0];
                for (int i = 0; i < columnDataTable.Rows.Count - 1; i++)
                {

                    columnnames += "\"" + columnDataTable.Rows[i][0] + "\"" + ",";
                    

                }

            }
            var colnames = "[" + columnnames.TrimEnd(',') + "]";
            var trimStart = colnames.TrimStart('\"');
            var trimEnd = trimStart.TrimEnd('\"');
            ///Fetching Today's Data to display in grid
            DataSet TodayData = adDB.FetchData_SP_columnName(Sp_list.FetchTodayData.ToString(), tableName, "WEB");
            DataTable tdata = new DataTable();
            if (TodayData.Tables.Count!=0)
            {
                tdata = TodayData.Tables[0];
                json = JsonConvert.SerializeObject(tdata);
            }
            ///Fetching Yesterday's Data to display in grid
            DataSet yesterdayDataset = adDB.FetchData_SP_columnName(Sp_list.FetchYesterDayData.ToString(), tableName, "WEB");
            DataTable yesdata = new DataTable();
            if (yesterdayDataset.Tables[0].Rows.Count != 0)
            {
                yesdata = yesterdayDataset.Tables[0];
                yesjson = JsonConvert.SerializeObject(yesdata);
            }
            ///Fetch Latitude and Longitude for map
            List<map> lstmap = new List<map>();
            var query = db.tbl_StationMaster.Where(x=>x.StationID==id).ToList();
            foreach (var item in query)
            {
                lstmap.Add(new map
                {
                    location = item.Latitude + "," + item.Longitude,
                    tooltip = item.Name
                });

            }
            ViewBag.MapData = lstmap;
            ViewBag.StationGrid = json;
            ViewBag.YesterdayGrid = yesjson;
            ViewBag.ColumnNamesGrid = trimEnd;
            return View();
        }
        public ActionResult TodayView()
        {
            var id = Session["StationID"].ToString();
            var tableName = "tbl_StationData_" + id;
            DataSet columnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTable = new DataTable();
            if (columnDataset.Tables.Count != 0)
            {
                columnDataTable = columnDataset.Tables[0];
                ViewBag.ColumnNames = columnDataTable;
            }

            return View();
        }
        public ActionResult TodayGraph(string data, string Position)
        {
            var id = Session["StationID"].ToString();

            var tableName = "tbl_StationData_" + id;
            ViewBag.GraphName = data;
            List<graph> lstgraph = new List<graph>();
            DataSet TodayData = adDB.FetchData_SP_columnName(Sp_list.FetchTodayData.ToString(), tableName, "WEB");
            DataTable tdata = new DataTable();
            int pos = Convert.ToInt32(Position);
            if (TodayData.Tables.Count != 0)
            {
                tdata = TodayData.Tables[0];
                for (int i = 0; i < tdata.Rows.Count - 1; i++)
                {
                    lstgraph.Add(new graph
                    {
                        Time = tdata.Rows[i][3].ToString(),
                        ColumnName = data,
                        value = tdata.Rows[i][pos].ToString()
                    });

                }

            }

            ViewBag.TodayData = lstgraph;
            return View();
        }
        public ActionResult YesterdayView()
        {
            var id = Session["StationID"].ToString();
            var tableName = "tbl_StationData_" + id;
            DataSet columnDatasetyesterday = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTableyesterday = new DataTable();
            if (columnDatasetyesterday.Tables.Count != 0)
            {
                columnDataTableyesterday = columnDatasetyesterday.Tables[0];
                ViewBag.YesterdayColumnNames = columnDataTableyesterday;
            }
            return View();
        }
        public ActionResult YesterdayGraph(string data, string Position)
        {
            var id = Session["StationID"].ToString();

            var tableName = "tbl_StationData_" + id;
            ViewBag.YesterdayGraphName = data;
            List<yesterdaygraph> lstyesterdaygraph = new List<yesterdaygraph>();
            DataSet yesterdayData = adDB.FetchData_SP_columnName(Sp_list.FetchYesterDayData.ToString(), tableName, "WEB");
            DataTable ydata = new DataTable();
            int pos = Convert.ToInt32(Position);
            if (yesterdayData.Tables.Count != 0)
            {
                ydata = yesterdayData.Tables[0];
                for (int i = 0; i < ydata.Rows.Count - 1; i++)
                {
                    lstyesterdaygraph.Add(new yesterdaygraph
                    {
                        Time = ydata.Rows[i][3].ToString(),
                        ColumnName = data,
                        value = ydata.Rows[i][pos].ToString()
                    });
                }
            }

            ViewBag.yesData = lstyesterdaygraph;
            return View();
        }
        public ActionResult WeeklyGrid()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            //Fetch Column Name
            DataSet columnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTable = new DataTable();
            if (columnDataset.Tables.Count != 0)
            {
                columnDataTable = columnDataset.Tables[0];
                ViewBag.ColumnNames = columnDataTable;
            }
            ///Create Weekly column name
            DataSet weeklycolumnanedataset = adDB.FetchData_SP_columnName("getColumnName", tableName, "WEB");
            DataTable weeklycolumndatatable = new DataTable();
            if (weeklycolumnanedataset.Tables.Count != 0)
            {
                weeklycolumndatatable = weeklycolumnanedataset.Tables[0];
                for (int i = 0; i < weeklycolumndatatable.Rows.Count - 1; i++)
                {
                    dynamiccol += "min([" + weeklycolumndatatable.Rows[i][0].ToString() + "]) as [Min " + weeklycolumndatatable.Rows[i][0].ToString() + "],max([" + weeklycolumndatatable.Rows[i][0].ToString() + "]) as [Max " + weeklycolumndatatable.Rows[i][0].ToString() + "],CEILING(avg(cast([" + weeklycolumndatatable.Rows[i][0].ToString() + "] as float))) as [avg " + weeklycolumndatatable.Rows[i][0].ToString() + "],";
                }
            }
            var trimDynamicCol = dynamiccol.TrimEnd(',');
            ///Fetching Weekly Data to display in grid
            DataSet weeklyDataset = adDB.FetchData_SP_MonthlyWeeklyData("getWeeklyMonthlyData", tableName, trimDynamicCol, "WEB");
            DataTable wdata = new DataTable();
            if (weeklyDataset.Tables[0].Rows.Count != 0)
            {
                wdata = weeklyDataset.Tables[0];
                weeklyjson = JsonConvert.SerializeObject(wdata);
            }
            ViewBag.week = wdata;
            return View();
        }
        public ActionResult TodayOverviewView()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            DataSet columnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTable = new DataTable();
            if (columnDataset.Tables.Count != 0)
            {
                columnDataTable = columnDataset.Tables[0];
                ViewBag.ColumnNames = columnDataTable;
                for (int i = 0; i < columnDataTable.Rows.Count - 1; i++)
                {
                    dynamiccol += "[" + columnDataTable.Rows[i][0].ToString() + "]" + ",";
                }
                var trimDynamicCol = dynamiccol.TrimEnd(',');
                DataSet ds2 = adDB.FetchData_SP_MonthlyWeeklyData(Sp_list.getTodayOverview.ToString(), tableName, trimDynamicCol, "WEB");
                DataTable dt1 = new DataTable();
                if (ds2.Tables[0].Rows.Count != 0)
                {
                    dt1 = ds2.Tables[0];
                }
                ViewBag.TodayOverview = dt1;
            }
            return View();
        }
        public ActionResult CurrentViewGauge()
        {
            var id = Session["StationID"].ToString();
            var tableName = "tbl_StationData_" + id;
            DataSet getMaxBatteryVoltageDataset = adDB.FetchData_SP_columnName("getMaxbatteryVoltage", tableName, "WEB");
            DataTable getMaxBatteryVoltagDataTable = new DataTable();
            if (getMaxBatteryVoltageDataset.Tables[0].Rows.Count != 0)
            {
                getMaxBatteryVoltagDataTable = getMaxBatteryVoltageDataset.Tables[0];
                ViewBag.MaxBattery = getMaxBatteryVoltagDataTable.Rows[0][0].ToString();
            }
            return View();
        }
        public ActionResult YesterdayOverviewView()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            DataSet yesterdaycolumnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable yesterdaycolumnDataTable = new DataTable();
            if (yesterdaycolumnDataset.Tables.Count != 0)
            {
                yesterdaycolumnDataTable = yesterdaycolumnDataset.Tables[0];
                ViewBag.ColumnNames = yesterdaycolumnDataTable;
                for (int i = 0; i < yesterdaycolumnDataTable.Rows.Count - 1; i++)
                {
                    dynamiccol += "[" + yesterdaycolumnDataTable.Rows[i][0].ToString() + "]" + ",";
                }
                var trimDynamicCol = dynamiccol.TrimEnd(',');
                DataSet yesterdaydataset = adDB.FetchData_SP_MonthlyWeeklyData("getYesterdayOverview", tableName, trimDynamicCol, "WEB");
                DataTable yesterdaydatatable = new DataTable();
                if (yesterdaydataset.Tables[0].Rows.Count != 0)
                {
                    yesterdaydatatable = yesterdaydataset.Tables[0];
                }
                ViewBag.yesterdayOverview = yesterdaydatatable;
            }
            return View();
        }
        public ActionResult WeeklyOverviewView()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            DataSet WeeklycolumnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable WeeklycolumnDataTable = new DataTable();
            if (WeeklycolumnDataset.Tables.Count != 0)
            {
                WeeklycolumnDataTable = WeeklycolumnDataset.Tables[0];
                ViewBag.ColumnNames = WeeklycolumnDataTable;
                for (int i = 0; i < WeeklycolumnDataTable.Rows.Count - 1; i++)
                {
                    dynamiccol += "[" + WeeklycolumnDataTable.Rows[i][0].ToString() + "]" + ",";
                }
                var trimDynamicCol = dynamiccol.TrimEnd(',');
                DataSet Weeklydataset = adDB.FetchData_SP_MonthlyWeeklyData("getWeeklyOverview", tableName, trimDynamicCol, "WEB");
                DataTable Weeklydatatable = new DataTable();
                if (Weeklydataset.Tables.Count != 0)
                {
                    Weeklydatatable = Weeklydataset.Tables[0];
                }
                ViewBag.weeklyOverview = Weeklydatatable;
            }
            return View();
        }
        public ActionResult WeeklyView()
        {
            var id = Session["StationID"].ToString();
            var tableName = "tbl_StationData_" + id;
            DataSet columnDatasetWeekly = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTableweekly = new DataTable();
            if (columnDatasetWeekly.Tables.Count != 0)
            {
                columnDataTableweekly = columnDatasetWeekly.Tables[0];
                ViewBag.weeklyColumnNames = columnDataTableweekly;
            }
            return View();
        }
        public ActionResult WeeklyGraph(string data, string Position)
        {
            var id = Session["StationID"].ToString();

            var tableName = "tbl_StationData_" + id;
            ViewBag.WeeklyGraphName = data;
            string dynamiccol = "";
            DataSet ds1 = adDB.FetchData_SP_columnName("getColumnName", tableName, "WEB");
            DataTable dt = new DataTable();
            List<Weeklygraph> lstweklygraph = new List<Weeklygraph>();
            int pos = Convert.ToInt32(Position);
            dt = ds1.Tables[0];

            ViewBag.weeklycolumnName = dt;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {

                dynamiccol += "min([" + dt.Rows[i][0].ToString() + "]) as [Min " + dt.Rows[i][0].ToString() + "],max([" + dt.Rows[i][0].ToString() + "]) as [Max " + dt.Rows[i][0].ToString() + "],CEILING(avg(cast([" + dt.Rows[i][0].ToString() + "] as float))) as [avg " + dt.Rows[i][0].ToString() + "],";
            }
            var trimDynamicCol = dynamiccol.TrimEnd(',');
            DataSet ds2 = adDB.FetchData_SP_MonthlyWeeklyData("getWeeklyMonthlyData", tableName, trimDynamicCol, "WEB");
            DataTable dt1 = new DataTable();
            if (ds2.Tables.Count != 0)
            {
                dt1 = ds2.Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    
                    lstweklygraph.Add(new Weeklygraph
                    {
                        Time = dt1.Rows[i][0].ToString(),
                        ColumnName = data,
                        min = dt1.Rows[i][pos].ToString(),
                        max = dt1.Rows[i][pos+1].ToString(),
                        avg = dt1.Rows[i][pos+2].ToString(),

                    });
                }
            }
            ViewBag.weekData = lstweklygraph;
            return View();
        }
        public ActionResult MonthlyOverviewView()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            DataSet MonthlycolumnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable MonthlycolumnDataTable = new DataTable();
            if (MonthlycolumnDataset.Tables.Count != 0)
            {
                MonthlycolumnDataTable = MonthlycolumnDataset.Tables[0];
                ViewBag.ColumnNames = MonthlycolumnDataTable;
                for (int i = 0; i < MonthlycolumnDataTable.Rows.Count - 1; i++)
                {
                    dynamiccol += "[" + MonthlycolumnDataTable.Rows[i][0].ToString() + "]" + ",";
                }
                var trimDynamicCol = dynamiccol.TrimEnd(',');
                DataSet Monthlydataset = adDB.FetchData_SP_MonthlyWeeklyData("getMonthlyOverview", tableName, trimDynamicCol, "WEB");
                DataTable Monthlydatatable = new DataTable();
                if (Monthlydataset.Tables.Count != 0)
                {
                    Monthlydatatable = Monthlydataset.Tables[0];
                }
                ViewBag.monthlyOverview = Monthlydatatable;
            }
            return View();
        }
        public ActionResult MonthlyView()
        {
            var id = Session["StationID"].ToString();
            var tableName = "tbl_StationData_" + id;
            DataSet columnDatasetMonthly = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTableMonthly = new DataTable();
            if (columnDatasetMonthly.Tables.Count != 0)
            {
                columnDataTableMonthly = columnDatasetMonthly.Tables[0];
                ViewBag.MonthlyColumnNames = columnDataTableMonthly;
            }
            return View();
        }
        public ActionResult MonthlyGraph(string data, string Position)
        {
            var id = Session["StationID"].ToString();

            var tableName = "tbl_StationData_" + id;
            ViewBag.MonthlyGraphName = data;
            string dynamiccol = "";
            DataSet ds1 = adDB.FetchData_SP_columnName("getColumnName", tableName, "WEB");
            DataTable dt = new DataTable();
            List<monthlygraph> lstmonthlygraph = new List<monthlygraph>();
            int pos = Convert.ToInt32(Position);
            dt = ds1.Tables[0];

            ViewBag.monthlycolumnName = dt;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {

                dynamiccol += "min([" + dt.Rows[i][0].ToString() + "]) as [Min " + dt.Rows[i][0].ToString() + "],max([" + dt.Rows[i][0].ToString() + "]) as [Max " + dt.Rows[i][0].ToString() + "],CEILING(avg(cast([" + dt.Rows[i][0].ToString() + "] as float))) as [avg " + dt.Rows[i][0].ToString() + "],";
            }
            var trimDynamicCol = dynamiccol.TrimEnd(',');
            DataSet ds2 = adDB.FetchData_SP_MonthlyWeeklyData("getMonthlyData", tableName, trimDynamicCol, "WEB");
            DataTable dt1 = new DataTable();
            if (ds2.Tables.Count != 0)
            {
                dt1 = ds2.Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {

                    lstmonthlygraph.Add(new monthlygraph
                    {
                        Time = dt1.Rows[i][0].ToString(),
                        ColumnName = data,
                        min = dt1.Rows[i][pos].ToString(),
                        max = dt1.Rows[i][pos + 1].ToString(),
                        avg = dt1.Rows[i][pos + 2].ToString(),

                    });
                }
            }
            ViewBag.montlyData = lstmonthlygraph;
            return View();
        }
        public ActionResult MonthlyGrid()
        {
            var id = Session["StationID"].ToString();
            string dynamiccol = "";
            var tableName = "tbl_StationData_" + id;
            //Fetch Column Name
            DataSet columnDataset = adDB.FetchData_SP_columnName(Sp_list.getColumnName.ToString(), tableName, "WEB");
            DataTable columnDataTable = new DataTable();
            if (columnDataset.Tables.Count != 0)
            {
                columnDataTable = columnDataset.Tables[0];
                ViewBag.ColumnNames = columnDataTable;
            }
            ///Create Weekly column name
            DataSet Monthlycolumnanedataset = adDB.FetchData_SP_columnName("getColumnName", tableName, "WEB");
            DataTable Monthlycolumndatatable = new DataTable();
            if (Monthlycolumnanedataset.Tables.Count != 0)
            {
                Monthlycolumndatatable = Monthlycolumnanedataset.Tables[0];
                for (int i = 0; i < Monthlycolumndatatable.Rows.Count - 1; i++)
                {
                    dynamiccol += "min([" + Monthlycolumndatatable.Rows[i][0].ToString() + "]) as [Min " + Monthlycolumndatatable.Rows[i][0].ToString() + "],max([" + Monthlycolumndatatable.Rows[i][0].ToString() + "]) as [Max " + Monthlycolumndatatable.Rows[i][0].ToString() + "],CEILING(avg(cast([" + Monthlycolumndatatable.Rows[i][0].ToString() + "] as float))) as [avg " + Monthlycolumndatatable.Rows[i][0].ToString() + "],";
                }
            }
            var trimDynamicCol = dynamiccol.TrimEnd(',');
            ///Fetching Weekly Data to display in grid
            DataSet MonthlyDataset = adDB.FetchData_SP_MonthlyWeeklyData("getMonthlyData", tableName, trimDynamicCol, "WEB");
            DataTable mdata = new DataTable();
            if (MonthlyDataset.Tables[0].Rows.Count != 0)
            {
                mdata = MonthlyDataset.Tables[0];
                monthlyjson = JsonConvert.SerializeObject(mdata);
            }
            ViewBag.month = mdata;
            return View();
        }

    }
}