using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class LeaveStatements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadP9();
            }
        }
        protected void LoadP9()
        {
            try
            {
                var filename = Session["username"].ToString().Replace(@"/", @"");
                try
                {
                    MyComponents.ObjNav.GenerateLeaveStatement(Session["username"].ToString(), String.Format("LvSttmnts{0}.pdf", filename));
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("LvSttmnts{0}.pdf", filename)));
                }
                catch (Exception exception)
                {
                    exception.Data.Clear();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}