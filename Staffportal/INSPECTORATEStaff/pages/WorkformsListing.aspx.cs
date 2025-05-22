using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
	public partial class WorkformsListing : System.Web.UI.Page
	{
        WebPortals webportals = MyComponents.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

        }
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string WorkformsList = webportals.GetQAWorkforms();
                if (!string.IsNullOrEmpty(WorkformsList))
                {
                    string[] WorkformsListArr = WorkformsList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < WorkformsListArr.Length; i++)
                    {
                        string[] responseArr = WorkformsListArr[i].Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length >= 4)
                        {
                            // Extract fields
                            string DocNo = responseArr[0];
                            string DocTitle = responseArr[1];
                            string Date = responseArr[3];
                            string status = responseArr[2];


                            // Determine status class "Open","In Progress","Resolved","Closed";
                            var statusCls = "secondary";
                            switch (status)
                            {
                                case "InActive":
                                    statusCls = "default"; break;
                                case "Active":
                                    statusCls = "success"; break;

                            }

                            // Generate table row
                            htmlStr += "<tr class='text-primary small'>";
                            htmlStr += $"<td>{i + 1}</td>";
                            htmlStr += $"<td>{DocNo}</td>";
                            htmlStr += $"<td>{DocTitle}</td>";
                            htmlStr += $"<td>{Date}</td>";
                            htmlStr += $"<td><span class='label label-{statusCls}'>{status}</span></td>";

                            htmlStr += "<td>";
                            //htmlStr += $"<td><a href='#' class='btn btn-sm btn-info' onclick=\"viewAttachments('{DocNo}')\"><i class='fa fa-paperclip'></i> View Documents</a></td>";
                            htmlStr += $"<td><a href='WorkformAttachments.aspx?requestNo={DocNo}' class='btn btn-sm btn-info'><i class='fa fa-paperclip'></i> View Documents</a></td>";
                          

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