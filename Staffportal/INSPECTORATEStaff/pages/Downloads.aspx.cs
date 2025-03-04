using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class Downloads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string mysession = Session["username"].ToString();
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                try
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Uploads/"));
                    List<ListItem> files = new List<ListItem>();
                    foreach (string filePath in filePaths)
                    {
                        files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    }
                    GridView1.DataSource = files;
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));

            Response.WriteFile(filePath);
            Response.End();
        }
    }
}