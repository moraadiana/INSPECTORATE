using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appkings.pages
{
    public partial class ImprestSurrenderList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string LoadImprests()
        {
            string userID = Session["username"].ToString();
            var htmlStr = string.Empty;
            try
            {
                using (SqlConnection connToNav = MyComponents.getconnToNAV())
                {
                    string q = null;
                    SqlCommand cmd = new SqlCommand();
                    q = "spGetMyImprestSurrender";
                    cmd.CommandText = q;
                    cmd.CommandType = CommandType.StoredProceFdure;
                    cmd.Connection = connToNav;
                    cmd.Parameters.AddWithValue("@Company_Name", MyComponents.Company_Name);
                    cmd.Parameters.AddWithValue("@userID", "'" + userID + "'");
                    cmd.Parameters.AddWithValue("@userID2", "'" + MyComponents.UserID + "'");
                    using (SqlDataReader drL = cmd.ExecuteReader())
                    {
                        if (drL.HasRows)
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
                                    case "Cheque Printing":
                                        statusCls = "success"; break;
                                    case "Cancelled":
                                        statusCls = "danger"; break;
                                    case "Checking":
                                        statusCls = "info"; break;
                                }
                                htmlStr += string.Format(@"<tr  class='text-primary small'>
                                                            <td>{0}</td>
                                                            <td>{2}</td>
                                                            <td>{4}</td>
                                                            <td>{5}</td>
                                                     </tr>",
                                    drL["No"],
                                    drL["Type"],
                                    drL["Account No_"],
                                    drL["Account Name"],
                                    drL["Amount"],
                                    drL["MyStatus"],
                                    statusCls
                                );
                            }
                    }
                    connToNav.Close();
                }
               // return htmlStr;
            }
            catch (Exception ex)
            {
                //cSite.SendErrorToDeveloper(ex);
                ex.Data.Clear();
            }

            return htmlStr;
        }

    }
}