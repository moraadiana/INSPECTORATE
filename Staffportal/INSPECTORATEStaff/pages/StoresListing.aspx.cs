using INSPECTORATEStaff.NAVWS;
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
    public partial class StoresListing : System.Web.UI.Page
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
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected string Jobsz()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string storereqList = MyComponents.ObjNav.GetMyStoreRequisitions(username);
                if (!string.IsNullOrEmpty(storereqList))
                {
                    int counter = 0;
                    string[] storereqListArr = storereqList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string storelist in storereqListArr)
                    {
                        counter++;
                        string[] responseArr = storelist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[3];
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
                        htmlStr += String.Format(@"
                     <tr>
                         <td>{0}</td>
                         <td>{1}</td>
                         <td>{2}</td>
                         <td>{3}</td>    
                 
                         <td><span class='label label-{5}'>{4}</span></td>
                         
                     </tr>

                     ",
                          counter,
                          responseArr[0],
                          responseArr[1],
                          responseArr[2],
                          responseArr[3],
                          // responseArr[4],


                          statusCls
                          );
                        ////to be returned need be to view details
                        /*  < td class='small'>
                               <div class='options btn-group' >
                                      <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
                                      <ul class='dropdown-menu'>

                                       <li><a href = 'StoresLines.aspx?An={0}&Tp=old' >< i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>

                                   </ul>	
                               </div>
                           </td>*/
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }
        protected string Jobsz1()
        {
            //string userID = MyComponents.UserID;
            string userID = Session["username"].ToString();
            var htmlStr = string.Empty;

            using (var conn = MyComponents.getconnToNAV())
            {
                string L_ = null;
                L_ = "spGetMyStoresReq";
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
                                                            <td><span class='label label-{4}'>{3}</span></td>
                                                            <td class='small'>
                                                               <div class='options btn-group' >
					                                                      <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                                                    <ul class='dropdown-menu'>
                                                                            <li><a href='StoresLines.aspx?An={0}&Tp=old'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                                                        </ul>				                                                  
                                                                </div>
                                                            </td>
                                                     </tr>",
                                drL["No_"],
                                drL["Request Description"],
                                drL["Request date"],
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