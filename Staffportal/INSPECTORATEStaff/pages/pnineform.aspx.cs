using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class pnineform : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadYears();
                LoadP9();
            }
        }
        protected void LoadYears()
        {
            string username = Session["username"].ToString();
            try
            {
                using (SqlConnection connToNAV = MyComponents.getconnToNAV())
                {
                    string sqlStmt = null;
                    sqlStmt = "spGetP9Years";
                    SqlCommand cmdProgStage = new SqlCommand();
                    cmdProgStage.CommandText = sqlStmt;
                    cmdProgStage.Connection = connToNAV;
                    cmdProgStage.CommandType = CommandType.StoredProcedure;
                    cmdProgStage.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmdProgStage.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader sqlReaderStages = cmdProgStage.ExecuteReader())
                    {
                        if (sqlReaderStages.HasRows)
                        {
                            ddlYear.DataSource = sqlReaderStages;
                            ddlYear.DataTextField = "Period Year";
                            ddlYear.DataValueField = "Period Year";
                            ddlYear.DataBind();
                        }
                    }
                    connToNAV.Close();
                }
            }
            catch (Exception ex)
            {

                ex.Data.Clear();
            }
        }
        protected void LoadP9()
        {
           try
            {
                var filename = Session["username"].ToString().Replace(@"/", @"");
                var employee =Session["username"].ToString();
                int period = Convert.ToInt32(ddlYear.SelectedValue);
                //var s =Convert.ToDateTime(period.ToString("M/dd/yyyy", CultureInfo.InvariantCulture));
                try
                {
                    MyComponents.ObjNav.Generatep9Report(period, employee, String.Format("p9Form{0}.pdf", filename));
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("p9Form{0}.pdf", filename)));

                }
            catch (Exception exception)
            {
                exception.Data.Clear();
               //     HttpContext.Current.Response.Write(exception);
            }
        }
           catch (Exception ex)
           {
                //ex.Data.Clear();
                //HttpContext.Current.Response.Write(ex);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadP9();
        }
    }
}