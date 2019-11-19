using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPractiseOne.Models;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.IO;
using System.Xml;

namespace MyPractiseOne.Controllers
{
    public class MasterController : Controller
    {
        public class FileData
        {
            public string Name { get; set; }
            public HttpPostedFileWrapper ImageFile { get; set; }
            public string Data { get; set; }
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString);
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CountryMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CountryInsertUpdateData(string CountryID, string CountryCode, string CountryName, string IsActive)
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {

                if (CountryCode == "")
                {
                    Dic["Result"] = "Please Enter First Name..!";
                    Dic["Focus"] = "txtCountryCode";
                }
                else if (CountryName == "")
                {
                    Dic["Result"] = "Please Enter Mobile No..!";
                    Dic["Focus"] = "txtCountryName";
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_CountryMaster_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryID", CountryID);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    cmd.Parameters.AddWithValue("@CountryName", CountryName);
                    cmd.Parameters.AddWithValue("@IsActive", Convert.ToByte(IsActive));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Dic["Result"] = dt.Rows[0]["Result"].ToString();
                        Dic["Status"] = dt.Rows[0]["Status"].ToString();
                        GetCountryDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                Dic["Result"] = ex.Message;
            }
            return Json(Dic);
        }
        [HttpGet]
        public ActionResult StateMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult StateInsertUpdateData(string StateID, string StateCode, string StateName, string CountryCode, string IsActive)
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {
                if (StateCode == "")
                {
                    Dic["Result"] = "Please Enter State Code..!";
                    Dic["Focus"] = "txtStateCode";
                }
                else if (StateName == "")
                {
                    Dic["Result"] = "Please Enter State Name..!";
                    Dic["Focus"] = "txtStateName";
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_StateMaster_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StateID", StateID);
                    cmd.Parameters.AddWithValue("@StateCode", StateCode);
                    cmd.Parameters.AddWithValue("@StateName", StateName);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    cmd.Parameters.AddWithValue("@IsActive", Convert.ToByte(IsActive));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Dic["Result"] = dt.Rows[0]["Result"].ToString();
                        Dic["Status"] = dt.Rows[0]["Status"].ToString();
                        GetStateDetails();
                    }

                }
            }
            catch (Exception ex)
            {

                Dic["Result"] = ex.Message;
            }
            return Json(Dic);
        }

        [HttpPost]
        public JsonResult GetCountryRecord()
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CountryMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetStateRecord()
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_StateMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCityRecord()
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetALL", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCountryCode()
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_StateMaster_GetCountryCode", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStateCode(string CountryCode)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetStateCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CountryEditRecord(string CountryID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CountryMaster_Edit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CountryDataDelete(string CountryID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CountryMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StateDataDelete(string StateID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_StateMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CityDataDelete(string CityID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CityMaster()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CityInsertUpdateData(string CityID, string CityName, string CountryCode, string StateCode, string IsActive)
        {

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {

                if (CityName == "")
                {
                    Dic["Result"] = "Please Enter CityName..!";
                    Dic["Focus"] = "txtStateName";
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_CityMaster_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CityID", CityID);
                    cmd.Parameters.AddWithValue("@CityName", CityName);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    cmd.Parameters.AddWithValue("@StateCode", StateCode);
                    cmd.Parameters.AddWithValue("@IsActive", Convert.ToByte(IsActive));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Dic["Result"] = dt.Rows[0]["Result"].ToString();
                        Dic["Status"] = dt.Rows[0]["Status"].ToString();
                        GetCityDetails();
                        GetCityDetails2();
                    }
                }
            }
            catch (Exception ex)
            {
                Dic["Result"] = ex.Message;
            }
            return Json(Dic);
        }

        public JsonResult GetCountryDetails()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CountryMaster_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder StrBulider = new StringBuilder();
            StrBulider.Append("<table class=table table - responsive table - stripped >");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    StrBulider.Append("<tr>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Columns[i].ColumnName.ToString());
                    StrBulider.Append("</td>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Rows[0][i].ToString());
                    StrBulider.Append("</td>");
                }
                StrBulider.Append("</tr></table>");
                Dic["Grid"] = StrBulider.ToString();
            }
            else
            {
                Dic["Grid"] = "Recod Not Found";
            }

            return Json(Dic);
        }

        public JsonResult GetStateDetails()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_StateMaster_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder StrBulider = new StringBuilder();
            StrBulider.Append("<table class=table table - responsive table - stripped >");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    StrBulider.Append("<tr>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Columns[i].ColumnName.ToString());
                    StrBulider.Append("</td>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Rows[0][i].ToString());
                    StrBulider.Append("</td>");
                }
                StrBulider.Append("</tr></table>");
                Dic["Grid"] = StrBulider.ToString();
            }
            else
            {
                Dic["Grid"] = "Recod Not Found";
            }

            return Json(Dic);
        }
        public JsonResult GetCityDetails()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder StrBulider = new StringBuilder();
            StrBulider.Append("<table class=table table - responsive table - stripped >");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    StrBulider.Append("<tr>");
                    StrBulider.Append("<td style='font-weight:bold'>");
                    StrBulider.Append(dt.Columns[i].ColumnName.ToString());
                    StrBulider.Append("</td>");
                    StrBulider.Append("<td>:</td>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Rows[0][i].ToString());
                    StrBulider.Append("</td>");
                }
                StrBulider.Append("</tr></table>");
                Dic["Grid"] = StrBulider.ToString();
            }
            else
            {
                Dic["Grid"] = "Recod Not Found";
            }

            return Json(Dic);
        }

        public JsonResult GetCityDetails2()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");
            StringBuilder st = new StringBuilder();
            st.Append("<table id='mytable' class='table' style='border:1px solid black; white-space:nowrap'></> ");

            st.Append("<tr style='background:maroon;color:white;text-transform:uppercase'>");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                st.Append("<td style='font-weight:bold'>Delete</td>");
                st.Append("<td style='font-weight:bold'>Edit</td>");
                st.Append("<td style='font-weight:bold'>DETAILS</td>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    string ColumnName = dt.Columns[i].ColumnName.ToString();

                    st.Append("<th>");
                    st.Append(ColumnName.ToString());
                    st.Append("</th>");


                }


                for (int j = 0; j < dt.Rows.Count; j++)
                {


                    st.Append("<tr>");
                    st.Append("<td><button id='btnDelete' style='background:#381818;color:white;' onclick=\"CityDataDelete('" + dt.Rows[j]["CityID"].ToString() + "')\"><i class='fa fa-trash' aria-hidden='true'></i></button></td>");
                    st.Append("<td><button id='btnEdit' style='background:#29a318;color:white;' onclick=\"EditRecord('" + dt.Rows[j]["CityID"].ToString() + "')\"><i class='fa fa-pencil' aria-hidden='true'></i></button></td>");

                    st.Append("<td><a onclick=\"GetCityByID('" + dt.Rows[j]["CityID"].ToString() + "')\" data-toggle='modal' data-target='#myModal'>" + dt.Rows[j]["CityID"].ToString() + "</a></td>");
                    //st.Append("<td><a onclick=\"GetCityByID('" + dt.Rows[j]["CityID"].ToString() + "')\" data-toggle='modal' data-target='#myModal'>View</a></td>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        st.Append("<td scope=col>");

                        st.Append(dt.Rows[j][i].ToString());
                        st.Append("</td>");

                    }
                    st.Append("</tr>");

                }

                st.Append("</table>");
                Dic["Grid"] = st.ToString();
                Dic["RowCount"] = dt.Rows.Count.ToString();
            }


            else
            {
                Dic["Grid"] = "Recod Not Found";
            }
            return Json(Dic);
        }


        public JsonResult GetCityByID(int CityID)
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder StrBulider = new StringBuilder();
            StrBulider.Append("<table class=table table - responsive table - stripped >");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    StrBulider.Append("<tr>");
                    StrBulider.Append("<td style='font-weight:bold'>");
                    StrBulider.Append(dt.Columns[i].ColumnName.ToString());
                    StrBulider.Append("</td>");
                    StrBulider.Append("<td style='font-weight:bold'>:</td>");
                    StrBulider.Append("<td>");
                    StrBulider.Append(dt.Rows[0][i].ToString());
                    StrBulider.Append("</td>");
                }
                StrBulider.Append("</tr></table>");
                Dic["Grid"] = StrBulider.ToString();
            }
            else
            {
                Dic["Grid"] = "Recod Not Found";
            }

            return Json(Dic);
        }
        [HttpPost]
        public JsonResult EditRecord(string CityID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_Edit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImageMaster()
        {

            return View();
        }


        [HttpPost]
        public JsonResult SaveImage(string byteData, string imageName, string contentType)
        {
            byte[] bytes = Convert.FromBase64String(byteData);
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_Image_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", imageName);
            cmd.Parameters.AddWithValue("@ContentType", contentType);
            cmd.Parameters.AddWithValue("@Data", bytes);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            return Json("Uploaded Successfully!", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImage()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");


            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_Image_GetALL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div><table>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<th>");
                sb.Append(dt.Rows[i]["Name"].ToString());
                sb.Append("</th>");
                sb.Append("<td>");
                sb.Append("<img src='data:image/jpg.base64',('" + dt.Rows[i]["Data"].ToString() + "'/>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table></div>");
            Dic["Grid"] = sb.ToString();
            return Json(Dic);
        }
        public ActionResult PacketMaster()
        {
            Session["dtPacket"] = null;
            return View();
        }
        [HttpPost]
        public JsonResult GetCityCode(string StateCode)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetCityCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StateCode", StateCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetConsigneeCountryCode()
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_StateMaster_GetCountryCode", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetConsigneeStateCode(string CountryCode)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetStateCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetConsigneeCityCode(string StateCode)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_CityMaster_GetCityCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StateCode", StateCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPacketDetails(int DTID, string PacketName, string PacketType, string Weight, string Length, string Width, string Height)
        {
            DataTable dtPacket = new DataTable();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            if (Session["dtPacket"] != null)
            {
                dtPacket = (DataTable)Session["dtPacket"];
            }
            else
            {
                dtPacket.Columns.Add("DTID", typeof(int));
                dtPacket.Columns.Add("PacketName", typeof(String));
                dtPacket.Columns.Add("PacketType", typeof(String));
                dtPacket.Columns.Add("Weight", typeof(string));
                dtPacket.Columns.Add("Length", typeof(String));
                dtPacket.Columns.Add("Width", typeof(String));
                dtPacket.Columns.Add("Height", typeof(String));
            }
            if (DTID <= 0)
            {
                DataRow dtRow = dtPacket.NewRow();
                dtRow["DTID"] = dtPacket.Rows.Count + 1;
                dtRow["PacketName"] = PacketName;
                dtRow["PacketType"] = PacketType;
                dtRow["Weight"] = Weight;
                dtRow["Length"] = Length;
                dtRow["Width"] = Width;
                dtRow["Height"] = Height;
                dtPacket.Rows.Add(dtRow);
            }
            else
            {
                for (int i = 0; i < dtPacket.Rows.Count; i++)
                {
                    if (DTID == Convert.ToInt32(dtPacket.Rows[i]["DTID"].ToString()))
                    {
                        dtPacket.Rows[i]["PacketName"] = PacketName;
                        dtPacket.Rows[i]["PacketType"] = PacketType;
                        dtPacket.Rows[i]["Weight"] = Weight;
                        dtPacket.Rows[i]["Length"] = Length;
                        dtPacket.Rows[i]["Width"] = Width;
                        dtPacket.Rows[i]["Height"] = Height;

                    }
                }

            }
            Session["dtPacket"] = dtPacket;
            Dic["Grid"] = dtPacket.ToString();

            return Json(Dic);

        }

        public JsonResult ShowPackageDetails()
        {
            DataTable dtPacket = new DataTable();

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");
            if (Session["dtPacket"] != null)
            {
                dtPacket = (DataTable)Session["dtPacket"];
            }
            StringBuilder st = new StringBuilder();
            st.Append("<table id='mytable' class='table' style='border:1px solid black; white-space:nowrap'></> ");

            st.Append("<tr style='background:maroon;color:white;text-transform:uppercase'>");



            if (dtPacket.Rows.Count > 0)
            {
                st.Append("<td style='font-weight:bold'>Delete</td>");
                st.Append("<td style='font-weight:bold'>Edit</td>");

                for (int i = 0; i < dtPacket.Columns.Count; i++)
                {

                    string ColumnName = dtPacket.Columns[i].ColumnName.ToString();

                    st.Append("<th>");
                    st.Append(ColumnName.ToString());
                    st.Append("</th>");


                }


                for (int j = 0; j < dtPacket.Rows.Count; j++)
                {


                    st.Append("<tr>");
                    st.Append("<td><button id='btnDelete' style='background:#381818;color:white;' onclick=\"PacketDataDelete('" + dtPacket.Rows[j]["DTID"].ToString() + "')\"><i class='fa fa-trash' aria-hidden='true'></i></button></td>");
                    st.Append("<td><button id='btnEdit' style='background:#29a318;color:white;' onclick=\"EditPacketRecord('" + dtPacket.Rows[j]["DTID"].ToString() + "')\"><i class='fa fa-pencil' aria-hidden='true'></i></button></td>");


                    for (int i = 0; i < dtPacket.Columns.Count; i++)
                    {
                        st.Append("<td scope=col>");

                        st.Append(dtPacket.Rows[j][i].ToString());
                        st.Append("</td>");

                    }
                    st.Append("</tr>");

                }

                st.Append("</table>");
                Dic["Grid"] = st.ToString();
                Dic["RowCount"] = dtPacket.Rows.Count.ToString();
            }


            else
            {
                Dic["Grid"] = "Recod Not Found";
            }
            return Json(Dic);
        }

        [HttpPost]
        public JsonResult EditPacketRecord(int DTID)
        {
            DataTable dtPacket = new DataTable();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");

            if (Session["dtPacket"] != null)
            {
                dtPacket = (DataTable)Session["dtPacket"];
                DataRow[] strArr = dtPacket.Select("DTID='" + DTID + "'");
                foreach (var row in strArr)
                {
                    Dic["DTID"] = row["DTID"].ToString();
                    Dic["PacketName"] = row["PacketName"].ToString();
                    Dic["PacketType"] = row["PacketType"].ToString();
                    Dic["Weight"] = row["Weight"].ToString();
                    Dic["Length"] = row["Length"].ToString();
                    Dic["Width"] = row["Width"].ToString();
                    Dic["Height"] = row["Height"].ToString();
                    Dic["Grid"] = "";
                }

            }

            return Json(Dic);

        }
        [HttpPost]
        public JsonResult PacketDataDelete(int DTID)
        {
            DataTable dtPacket = new DataTable();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");

            if (Session["dtPacket"] != null)
            {
                dtPacket = (DataTable)Session["dtPacket"];
                DataRow[] strArr = dtPacket.Select("DTID='" + DTID + "'");
                foreach (DataRow row in strArr)
                {
                    dtPacket.Rows.Remove(row);
                }

            }

            return Json(Dic);
        }

        [HttpPost]
        public JsonResult PacketInsertUpdateData(int PacketID, string AWBNo, string ConsigerName, string ConsigerAddress,
       string ConsigerCountryCode, string ConsigerStateCode, string ConsigerCityName, string ConsigerPincode,
       string ConsigerPhoneNo, string ConsigerGSTNo, string ConsigerFaxNo, string ConsigneeName,
       string ConsigneeAddress, string ConsigneeCountryCode, string ConsigneeStateCode, string ConsigneeCityName,
       string ConsigneePincode, string ConsigneePhoneNo, string ConsigneeGSTNo, string ConsigneeFaxNo, int PacketDetailsID,
       string PacketName, string PacketType, string PacketWeight, string PacketLength, string PacketWidth, string PacketHeight)

        {
            DataTable dtPacket = new DataTable();
            string PacketDetail = "";
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {

                if (AWBNo == "")
                {
                    Dic["Result"] = "Please Enter AWBNO..!";
                    Dic["Focus"] = "txtawbno";
                }
                else
                {
                    if (Session["dtPacket"] != null)
                    {
                        dtPacket = (DataTable)Session["dtPacket"];
                        dtPacket.TableName = "PacketDetails";
                        StringWriter sw = new StringWriter();
                        dtPacket.WriteXml(sw);
                        PacketDetail = sw.ToString();
                    }

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_Packet_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PacketID", PacketID);
                    cmd.Parameters.AddWithValue("@AWBNo", AWBNo);
                    cmd.Parameters.AddWithValue("@ConsigerName", ConsigerName);
                    cmd.Parameters.AddWithValue("@ConsigerAddress", ConsigerAddress);
                    cmd.Parameters.AddWithValue("@ConsigerCountryCode", ConsigerCountryCode);
                    cmd.Parameters.AddWithValue("@ConsigerStateCode", ConsigerStateCode);
                    cmd.Parameters.AddWithValue("@ConsigerCityName", ConsigerCityName);
                    cmd.Parameters.AddWithValue("@ConsigerPincode", ConsigerPincode);
                    cmd.Parameters.AddWithValue("@ConsigerPhoneNo", ConsigerPhoneNo);
                    cmd.Parameters.AddWithValue("@ConsigerGSTNo", ConsigerGSTNo);
                    cmd.Parameters.AddWithValue("@ConsigerFaxNo", ConsigerFaxNo);
                    cmd.Parameters.AddWithValue("@ConsigneeName", ConsigneeName);
                    cmd.Parameters.AddWithValue("@ConsigneeAddress", ConsigneeAddress);
                    cmd.Parameters.AddWithValue("@ConsigneeCountryCode", ConsigneeCountryCode);
                    cmd.Parameters.AddWithValue("@ConsigneeStateCode", ConsigneeStateCode);
                    cmd.Parameters.AddWithValue("@ConsigneeCityName", ConsigneeCityName);
                    cmd.Parameters.AddWithValue("@ConsigneePincode", ConsigneePincode);
                    cmd.Parameters.AddWithValue("@ConsigneePhoneNo", ConsigneePhoneNo);
                    cmd.Parameters.AddWithValue("@ConsigneeGSTNo", ConsigneeGSTNo);
                    cmd.Parameters.AddWithValue("@ConsigneeFaxNo", ConsigneeFaxNo);
                    cmd.Parameters.AddWithValue("@PacketDetails", PacketDetail.Trim());
                    //save xml file in folder also
                    //if (Session["dtPacket"] != null)
                    //{

                    //    dtPacket = (DataTable)Session["dtPacket"];
                    //    dtPacket.TableName = "MyTable";

                    //    //DataSet ds = new DataSet();
                    //    //dtPacket = ds.Tables["dtPacket"];
                    //    //XmlDocument xdoc = new XmlDocument();
                    //    //string propertyFile = @"E:\Training\MyPractiseOne\MyPractiseOne\UploadImage\PacketDetailSs.xml";
                    //    //string propertyFolder = propertyFile.Substring(0, propertyFile.LastIndexOf("\\") + 1);
                    //    //string newXML = propertyFolder + "PacketDetails.xml";
                    //    //xdoc.Save(newXML);​\----------
                    //    dtPacket.WriteXml(@"E:\Training\MyPractiseOne\MyPractiseOne\UploadImage\pp.xml",true);
                    //}
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    Dic["Result"] = dt.Rows[0]["Result"].ToString();
                    Dic["Status"] = dt.Rows[0]["Status"].ToString();
                }
            }

            catch (Exception ex)
            {
                Dic["Result"] = ex.Message;
            }
          
            return Json(Dic);
        }

        public JsonResult AddPacketDetailControls(string NoOfControls)
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            StringBuilder strControls = new StringBuilder();
            Dic["Grid"] = "";
            for (int i = 0; i < Convert.ToInt32(NoOfControls); i++)
            {
                strControls.Append("<br/>");
                strControls.Append("<div class='col-lg-12 col-md-12 col-sm-12'>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<input type='hidden' id='hfDTID' value='0' />");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>Name<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append(" <div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<input type='text' id='txtPacketName' class='form-control' autocomplete='off' placeholder='Name'/>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>P&nbsp;Type<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append(" <div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<select id='ddlPacketType' class='form-control'>");
                strControls.Append("<option value='0'>Select</option>");
                strControls.Append("<option value='Doc'>Doc</option>");
                strControls.Append("<option value='Non Doc'>Non Doc</option>");
                strControls.Append("</select>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>Weight<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append("<div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<input type='text' id='txtPacketWeight' class='form-control' autocomplete='off' placeholder='Packet Weight' />");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>Length<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append("<div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<input type='text' id='txtLength' class='form-control'  autocomplete='off' placeholder='Length'/>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>Width<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append("<div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<input type='text' id='txtWidth' class='form-control' autocomplete='off' placeholder='Width'/>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<div class='col-lg-2 col-md-2 col-sm-2'>");
                strControls.Append("<div class='form-group'>");
                strControls.Append("<label class='col-sm-4 col-lg-4 col-md-4'>Height<span style='color:red;font-size:20px;'>*</span></label>");
                strControls.Append("<div class='col-sm-8 col-lg-8 col-md-8'>");
                strControls.Append("<input type='text' id='txtHeight'  class='form-control' autocomplete='off' placeholder='Height'/>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("</div>");
                strControls.Append("<br/>");
                strControls.Append("<br/>");
                Dic["Grid"] = strControls.ToString();
            }

            return Json(Dic);
        }

        public JsonResult GetPacketRecord()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");
            StringBuilder st = new StringBuilder();
            st.Append("<table class='table table-bordered table-responsive table-dark' id='MyTable'>");
  
            st.Append("<tr style='background:maroon;color:white;text-transform:uppercase'>");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PacketAllDetails_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                st.Append("<td style='font-weight:bold'>Delete</td>");
                st.Append("<td style='font-weight:bold'>Edit</td>");
           
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    string ColumnName = dt.Columns[i].ColumnName.ToString();

                    st.Append("<th>");
                    st.Append(ColumnName.ToString());
                    st.Append("</th>");


                }


                for (int j = 0; j < dt.Rows.Count; j++)
                {


                    st.Append("<tr>");
                    st.Append("<td><button id='btnDelete' style='background:#381818;color:white;' onclick=\"CityDataDelete('" + dt.Rows[j]["Packet ID"].ToString() + "')\"><i class='fa fa-trash' aria-hidden='true'></i></button></td>");
                    st.Append("<td><button id='btnEdit' style='background:#29a318;color:white;' onclick=\"EditRecord('" + dt.Rows[j]["Packet ID"].ToString() + "')\"><i class='fa fa-pencil' aria-hidden='true'></i></button></td>");


                    //st.Append("<td><a onclick=\"GetCityByID('" + dt.Rows[j]["CityID"].ToString() + "')\" data-toggle='modal' data-target='#myModal'>View</a></td>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        st.Append("<td scope=col>");

                        st.Append(dt.Rows[j][i].ToString());
                        st.Append("</td>");

                    }
                    st.Append("</tr>");

                }

                st.Append("</table>");
                Dic["Grid"] = st.ToString();
                Dic["RowCount"] = dt.Rows.Count.ToString();
            }


            else
            {
                Dic["Grid"] = "Recod Not Found";
            }
            return Json(Dic);
        }

        public ActionResult PacketStatusMaster()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetPacketStatusMasterRecord()
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PacketStatusMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetPacketStatusMasterRecord()
        //{
        //    Dictionary<string, string> Dic = new Dictionary<string, string>();
        //    Dic.Add("Grid", "");
        //    Dic.Add("RowCount", "");
        //    StringBuilder st = new StringBuilder();
        //    st.Append("<table class='table table-bordered table-responsive table-dark' id='MyTable'>");

        //    st.Append("<tr style='background:maroon;color:white;text-transform:uppercase'>");

        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("Sp_PacketStatusMaster_Get", con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        st.Append("<td style='font-weight:bold'>Delete</td>");
        //        st.Append("<td style='font-weight:bold'>Edit</td>");

        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {

        //            string ColumnName = dt.Columns[i].ColumnName.ToString();

        //            st.Append("<th>");
        //            st.Append(ColumnName.ToString());
        //            st.Append("</th>");


        //        }


        //        for (int j = 0; j < dt.Rows.Count; j++)
        //        {


        //            st.Append("<tr>");
        //            st.Append("<td><button id='btnDelete' style='background:#381818;color:white;' onclick=\"CityDataDelete('" + dt.Rows[j]["PacketStatusID"].ToString() + "')\"><i class='fa fa-trash' aria-hidden='true'></i></button></td>");
        //            st.Append("<td><button id='btnEdit' style='background:#29a318;color:white;' onclick=\"EditRecord('" + dt.Rows[j]["PacketStatusID"].ToString() + "')\"><i class='fa fa-pencil' aria-hidden='true'></i></button></td>");
        //            for (int i = 0; i < dt.Columns.Count; i++)
        //            {
        //                st.Append("<td scope=col>");

        //                st.Append(dt.Rows[j][i].ToString());
        //                st.Append("</td>");

        //            }
        //            st.Append("</tr>");

        //        }

        //        st.Append("</table>");
        //        Dic["Grid"] = st.ToString();
        //        Dic["RowCount"] = dt.Rows.Count.ToString();
        //    }


        //    else
        //    {
        //        Dic["Grid"] = "Recod Not Found";
        //    }
        //    return Json(Dic);
        //}


        [HttpPost]
        public JsonResult PacketStatusMasterEditRecord(string PacketStatusID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PacketStatusMaster_Edit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PacketStatusID", PacketStatusID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PacketStatusMasterDataDelete(string PacketStatusID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PacketStatusMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PacketStatusID", PacketStatusID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PacketStatusMasterDataInsertUpdate(string PacketStatusID, string PacketID, string StatusDate, string StatusTime, string Location, string PacketStatus, string CreteadBy,string CreatedDate,string ModifiedBy, string ModifiedDate)
        {
            
            Dictionary <string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {

                if (PacketID == "")
                {
                    Dic["Result"] = "Please Enter First Name..!";
                    Dic["Focus"] = "txtPacketID";
                }
             
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_PacketStatusMaster_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PacketStatusID", PacketStatusID);
                    cmd.Parameters.AddWithValue("@PacketID", PacketID);
                    cmd.Parameters.AddWithValue("@StatusDate", StatusDate);
                    cmd.Parameters.AddWithValue("@StatusTime", StatusTime);
                    cmd.Parameters.AddWithValue("@Location", Location);
                    cmd.Parameters.AddWithValue("@PacketStatus", PacketStatus);
                    cmd.Parameters.AddWithValue("@CreteadBy", CreteadBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                    cmd.Parameters.AddWithValue("@ModifiedBy", ModifiedBy);
                    cmd.Parameters.AddWithValue("@ModifiedDate", ModifiedDate);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Dic["Result"] = dt.Rows[0]["Result"].ToString();
                        Dic["Status"] = dt.Rows[0]["Status"].ToString();
                        GetCountryDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                Dic["Result"] = ex.Message;
            }
            return Json(Dic);
        }
        public ActionResult PacketStatusMasterDataByID()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPacketStatusMasterRecordByID(int PacketID)
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PacketStatusMasterGetAllRecordViaPacketID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PacketID", PacketID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PinCodeMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PincodeInsertUpdateData(string PincodeID, string PinCode, string CityID, string CountryCode, string StateCode, string IsActive)
        {

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Result", "");
            Dic.Add("Status", "0");
            Dic.Add("Focus", "");
            try
            {

                if (PinCode == "")
                {
                    Dic["Result"] = "Please Enter Pin Code..!";
                    Dic["Focus"] = "txtPinCode";
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_InsertUpdate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PincodeID", PincodeID);
                    cmd.Parameters.AddWithValue("@PinCode", PinCode);
                    cmd.Parameters.AddWithValue("@CityID", CityID);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    cmd.Parameters.AddWithValue("@StateCode", StateCode);
                    cmd.Parameters.AddWithValue("@IsActive", Convert.ToByte(IsActive));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Dic["Result"] = dt.Rows[0]["Result"].ToString();
                        Dic["Status"] = dt.Rows[0]["Status"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Dic["Result"] = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return Json(Dic);
        }

        public ActionResult PinCodeUse()
        {
            return View();
        }

        public JsonResult GetCountryCodeByUsingPincode(string PinCode)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_AddressByPincode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PinCode", PinCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPincodeRecord()
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPincodeDetails()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Grid", "");
            Dic.Add("RowCount", "");
            StringBuilder st = new StringBuilder();
            st.Append("<table id='mytable' class='table' style='border:1px solid black; white-space:nowrap'></> ");

            st.Append("<tr style='background:maroon;color:white;text-transform:uppercase'>");

            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_Get", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                st.Append("<td style='font-weight:bold'>Delete</td>");
                st.Append("<td style='font-weight:bold'>Edit</td>");

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    string ColumnName = dt.Columns[i].ColumnName.ToString();

                    st.Append("<th>");
                    st.Append(ColumnName.ToString());
                    st.Append("</th>");


                }


                for (int j = 0; j < dt.Rows.Count; j++)
                {


                    st.Append("<tr>");
                    st.Append("<td><button id='btnDelete' style='background:#381818;color:white;' onclick=\"PincodeDataDelete('" + dt.Rows[j]["PincodeID"].ToString() + "')\"><i class='fa fa-trash' aria-hidden='true'></i></button></td>");
                    st.Append("<td><button id='btnEdit' style='background:#29a318;color:white;' onclick=\"PincodeDataEdit('" + dt.Rows[j]["PincodeID"].ToString() + "')\"><i class='fa fa-pencil' aria-hidden='true'></i></button></td>");


                    //st.Append("<td><a onclick=\"GetCityByID('" + dt.Rows[j]["CityID"].ToString() + "')\" data-toggle='modal' data-target='#myModal'>View</a></td>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        st.Append("<td scope=col>");

                        st.Append(dt.Rows[j][i].ToString());
                        st.Append("</td>");

                    }
                    st.Append("</tr>");

                }

                st.Append("</table>");
                Dic["Grid"] = st.ToString();
                Dic["RowCount"] = dt.Rows.Count.ToString();
            }


            else
            {
                Dic["Grid"] = "Recod Not Found";
            }
            return Json(Dic);
        }

        [HttpPost]
        public JsonResult PincodeDataEdit(string PincodeID)
        {

            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_Edit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PincodeID", PincodeID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult PincodeDataDelete(string PincodeID)
        {
            string data = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_PincodeMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PincodeID", PincodeID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            data = JsonConvert.SerializeObject(dt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}














