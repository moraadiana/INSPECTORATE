using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

using iTextSharp.tool.xml;


namespace INSPECTORATEStaff.pages
{
    public partial class TrainingListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            GetTrainingCourses();
            MyTrainingApplications();
            mlvTrainings.ActiveViewIndex = 0;
        }

        private void GetTrainingCourses()
        {
            try
            {
                using (SqlConnection conn = MyComponents.getconnToNAV())
                {
                    string L_ = null;

                    var com = new SqlCommand();
                    L_ = "spGetTrainingCourses";

                    com.CommandText = L_;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Connection = conn;
                    com.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    listCourse.DataSource = dt;
                    listCourse.DataBind();

                    listCourse.UseAccessibleHeader = true;
                    listCourse.HeaderRow.TableSection = TableRowSection.TableHeader;

                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }



        private void MyTrainingApplications()
        {
            try
            {
                using (SqlConnection conn = MyComponents.getconnToNAV())
                {
                    string L_ = null;

                    var com = new SqlCommand();
                    L_ = "spGetMyTrainingApplications";
                    var user = Session["username"].ToString();
                    var userId = "'" + user + "'";

                    com.CommandText = L_;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Connection = conn;
                    com.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    com.Parameters.AddWithValue("@Employee_No", userId);
                    SqlDataAdapter sda = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    myApplications.DataSource = dt;
                    myApplications.DataBind();

                    myApplications.UseAccessibleHeader = true;
                    myApplications.HeaderRow.TableSection = TableRowSection.TableHeader;

                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

        }

        protected void btnViewMyApps_Click(object sender, EventArgs e)
        {

            mlvTrainings.ActiveViewIndex = 1;

        }


        protected void listCourse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "apply")
            {
                int c_row;
                c_row = Convert.ToInt32(e.CommandArgument.ToString());
                var Employee_Num = Session["username"].ToString();
                string course_code = listCourse.Rows[c_row].Cells[0].Text;
                DateTime appDate = DateTime.Today;
                string user_Id = "WEBPORTALS";

                // try
                // {
                //string ReturnMsg = MyComponents.ObjNav.TrainingApplication(Employee_Num, user_Id, course_code, appDate);
                string ReturnMsg = "";
                if (ReturnMsg != string.Empty)
                {
                    if (ReturnMsg == "SUCCESS")
                    {
                        //Response.Redirect("TrainingListing.aspx");
                        //return;
                        Message("Application Submitted Successfully");
                    }
                    else if (ReturnMsg == "APPLIED")
                    {
                        Message("You have already Applied for this Course");
                    }
                    else
                    {
                        Message("Failed to Add Application, Please contact the Administrator");
                    }
                }
                else
                {
                    Message("Empty Return Message");
                }

                // }
                // catch (Exception ex)
                // {
                //     ex.Data.Clear();
                // }
            }

        }

        public void MessageWithRefresh(string strMsg)
        {
            string strScript = null;
            string myPage = "Login.aspx";
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "window.location='" + myPage + "'";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }

        public void Message(string strMsg)
        {
            string strScript = null;
            strScript = "<script>";
            strScript = strScript + "alert('" + strMsg + "');";
            strScript = strScript + "</script>";
            Page.RegisterStartupScript("ClientScript", strScript.ToString());
        }


    }
}