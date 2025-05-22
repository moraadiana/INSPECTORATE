using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
    public partial class WorkFormAttachments : System.Web.UI.Page
    {
        WebPortals webportals = MyComponents.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
            string documentNo = Request.QueryString["requestNo"].ToString();
            BindAttachedDocuments(documentNo);
        }
        private void BindAttachedDocuments(string documentNo)
        {
            try
            {
                string docLines = webportals.GetDocumentlines(documentNo);

                if (!string.IsNullOrEmpty(docLines) && docLines != "No document lines")
                {
                    string[] lineItems = docLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Document No");
                    dt.Columns.Add("File Name");
                    // dt.Columns.Add("$systemCreatedAt");
                    dt.Columns.Add("SystemId");


                    foreach (string item in lineItems)
                    {
                        string[] fields = item.Split(strLimiters, StringSplitOptions.None);

                        if (fields.Length == 4)
                        {
                            DataRow row = dt.NewRow();
                            row["Document No"] = fields[0];
                            row["File Name"] = fields[1];
                            // row["$systemCreatedAt"] = fields[2];
                            row["SystemId"] = fields[3];
                            dt.Rows.Add(row);
                        }
                    }

                    gvAttachments.DataSource = dt;
                    gvAttachments.DataBind();
                }
                else
                {
                    gvAttachments.DataSource = null;
                    gvAttachments.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or show an error message as needed)
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        protected void lbtnDownload_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            string fileName = lbtn.CommandArgument + ".pdf" ;

            // Assume all uploaded files are stored in "~/Uploads/"
            string filePath = Server.MapPath("~/Uploads/" + fileName);

            if (System.IO.File.Exists(filePath))
            {
                Response.Clear();
                Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                Response.WriteFile(filePath);
                Response.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File not found.');", true);
            }
        }

        protected void lbtnDownload_Click1(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = "application/pdf";
           // Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));

           // Response.WriteFile(filePath);
            Response.End();
        }

            /*
            protected void lbtnDownload_Click(object sender, EventArgs e)
            {
                LinkButton lbtn = (LinkButton)sender;
                GridViewRow row = (GridViewRow)lbtn.NamingContainer;

                // Get the Document No from the CommandArgument
                string documentNo = lbtn.CommandArgument;

                // Example: Assuming file name and path are stored in your database
                // Replace this with actual retrieval logic
               // string fileName = GetFileNameByDocumentNo(documentNo); // e.g., "Report.pdf"
                string filePath = Server.MapPath("~/Uploads/" + fileName);

                if (System.IO.File.Exists(filePath))
                {
                    Response.Clear();
                    Response.ContentType = MimeMapping.GetMimeMapping(fileName);
                    Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                    Response.WriteFile(filePath);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    // Handle file not found case
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File not found.');", true);
                }
            }*/

        }
}