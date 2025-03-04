using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace INSPECTORATEStaff.pages
{
    public partial class PurchaseReqListing : System.Web.UI.Page
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
        protected string Jobsz()
        {
            string userID = Session["username"].ToString();
            var htmlStr = string.Empty;

            using (var conn = MyComponents.getconnToNAV())
            {
                string L_ = null;
                L_ = "spGetMyPurchaseReq";
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
                                case "Open":
                                    statusCls = "warning"; break;
                                case "Released":
                                    statusCls = "default"; break;
                                case "Pending Approval":
                                    statusCls = "primary"; break;
                                case "Cheque Printing":
                                    statusCls = "success"; break;
                                case "Pending Prepayment":
                                    statusCls = "danger"; break;
                                case "Canceled":
                                    statusCls = "info"; break;
                            }
                            htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td>{0}</td>
                                                            <td>{1}</td>
                                                            <td>{2}</td>
                                                            <td>{3}</td>
                                                            <td><span class='label label-{5}'>{4}</span></td>
                                                            <td class='small'>
                                                               <div class='options btn-group' >
					                                                      <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                                                    <ul class='dropdown-menu'>
                                                                            <li><a href='PurchaseReqLines.aspx?An={0}&Tp=old'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                                                        </ul>				                                                  
                                                                </div>
                                                            </td>
                                                     </tr>",
                                drL["No_"],
                                drL["MyDocType"],
                                Convert.ToDateTime(drL["Order Date"]).ToLongDateString(),
                                Convert.ToDateTime(drL["Expected Receipt Date"]).ToLongDateString(),
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