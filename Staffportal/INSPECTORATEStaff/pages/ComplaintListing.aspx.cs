using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
	public partial class ComplaintListing : System.Web.UI.Page
	{
       // Staffportall webportals = Components.ObjNav;
        WebPortals webportals = MyComponents.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
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
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string ComplaintList = webportals.GetMyComplaints(username);
                if (!string.IsNullOrEmpty(ComplaintList))
                {
                    string[] ComplaintListArr = ComplaintList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < ComplaintListArr.Length; i++)
                    {
                        string[] responseArr = ComplaintListArr[i].Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length >= 4)
                        {
                            // Extract fields
                            string ID = responseArr[0];
                            string Date = responseArr[1];
                            string Description = responseArr[2];
                            string status = responseArr[3];


                            // Determine status class "Open","In Progress","Resolved","Closed";
                            var statusCls = "secondary";
                            switch (status)
                            {
                                case "Open":
                                    statusCls = "warning"; break;
                                case "In Progress":
                                    statusCls = "warning"; break;
                                case "Resolved":
                                    statusCls = "default"; break;
                                case "Closed":
                                    statusCls = "success"; break;
                               
                            }

                            // Generate table row
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>";
                            htmlStr += $"<td>{ID}</td>";
                            htmlStr += $"<td>{Date}</td>";
                            htmlStr += $"<td>{Description}</td>";
                            htmlStr += $"<td><span class='label label-{statusCls}'>{status}</span></td>";

                            htmlStr += "<td>";
                            htmlStr += "  <div class='dropdown'>";
                            htmlStr += "    <button class='btn btn-sm btn-primary dropdown-toggle' type='button' data-toggle='dropdown'>Actions <span class='caret'></span></button>";
                            htmlStr += "    <ul class='dropdown-menu'>";
                            htmlStr += $"      <li><a class='dropdown-item' href='Complaint.aspx?requestNo={ID}&query=old&status={status}'><i class='fa fa-info-circle text-success'></i> Details</a></li>";
                            
                            htmlStr += "    </ul>";
                            htmlStr += "  </div>";
                            htmlStr += "</td>";

                            htmlStr += "</tr>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }
    }
}