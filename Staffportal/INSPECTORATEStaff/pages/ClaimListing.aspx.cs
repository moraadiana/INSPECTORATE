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
    public partial class ClaimListing : System.Web.UI.Page
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
        }
        protected string Jobs()
        {
            string userID = Session["username"].ToString();
            var htmlStr = string.Empty;

            using (var conn = MyComponents.getconnToNAV())
            {
                string L_ = null;
                L_ = "spGetMyClaims";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = L_;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                using (SqlDataReader drL = cmd.ExecuteReader())
                {
                    if (drL.HasRows)
                    {
                        while (drL.Read())
                        {
                            var statusCls = "default";
                            string status = drL["MyStatus"].ToString();
                            switch (status)
                            {
                                case "Pending":
                                    statusCls = "warning"; break;
                                case "1st Approval":
                                    statusCls = "default"; break;
                                case "2nd Approval":
                                    statusCls = "primary"; break;
                                case "Pending Approval":
                                    statusCls = "info"; break;
                                case "Approved":
                                    statusCls = "success"; break;
                                case "Cheque Printing":
                                    statusCls = "success"; break;
                                case "Cancelled":
                                    statusCls = "danger"; break;
                                case "Checking":
                                    statusCls = "info"; break;
                            }
                            htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td>{0}</td>
                                                            <td>{1}</td>
                                                            <td>{2}</td>
                                                            <td><span class='label label-{4}'>{3}</span></td>
                                                            <td class='small'>
                                                               <div class='options btn-group' >
					                                                      <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                                                    <ul class='dropdown-menu'>
                                                                            <li><a href='ClaimLines.aspx?An={0}&Tp=old'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                                                        </ul>				                                                  
                                                                </div>
                                                            </td>
                                                     </tr>",
                                drL["No_"],
                                drL["Purpose"],
                                drL["Payee"],
                                drL["MyStatus"],
                                statusCls
                            );
                        }
                    }
                }
            }
            return htmlStr;
        }
    }
}