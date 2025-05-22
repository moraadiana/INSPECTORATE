using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INSPECTORATEStaff.pages
{
	public partial class Complaint : System.Web.UI.Page
	{
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
                    return;
                }
                // LoadStaffDepartmentDetails();
                loadUsers();

                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                string query = Request.QueryString["query"];
                if (query == "New")
                {
                   
                }
                else if (query == "old")
                {
                    string requestNo = Request.QueryString["requestNo"];
                    string response = webportals.GetComplaintsDetails(requestNo);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                        
                        txtDescription.Text = responseArr[2];
                        txtNotes.Text = responseArr[4];
                        ddlHandledBy.Text = responseArr[5];
                        txtFeedback.Text = responseArr[7];
                        DateTime resDate;
                        if (DateTime.TryParseExact(
                                responseArr[6],     
                                "MM/dd/yy",          
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out resDate))
                        {
                            txtDate.Text = resDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtDate.Text = "Error";
                        }



                    }
                    else
                    {
                        Message("An error occured while loading details. Please try again later.");
                        return;
                    }

                }

                if (approvalStatus == "Open" || approvalStatus == "Pending" || approvalStatus == "New")
                {
                    lbtnSubmit.Visible = true;

                }

                else
                {
                    lbtnSubmit.Visible = false;

                }
            }
        }
        private void loadUsers()
        {
            try
            {
                ddlHandledBy.Items.Clear();
                ddlHandledBy.Items.Add("--Select--");
                string response = webportals.GetUsers();
                if (!string.IsNullOrEmpty(response))
                {
                    string[] usersArr = response.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string users in usersArr)
                    {
                        ddlHandledBy.Items.Add(new ListItem(users));
                    }
                }
                else
                {
                    ddlHandledBy.Items.Add(new ListItem("No users found"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ddlHandledBy.Items.Add(new ListItem("Error loading users"));
            }

        }
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                // string grevanceId = ddlLeaveType.SelectedValue;
                string description = txtDescription.Text;
                string reslnNote = txtNotes.Text;
                string handledBy = ddlHandledBy.SelectedValue;
                string date = txtDate.Text;
                string feedback = txtFeedback.Text;


               

                if (string.IsNullOrEmpty(description))
                {
                    Message("Description cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(reslnNote))
                {
                    Message("Resolution Note cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(handledBy))
                {
                    Message("Handled By cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(date))
                {
                    Message("Date cannot be null");
                   // txtAppliedDays.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(feedback))
                {
                    Message("Feedback cannot be null");
                    return;
                }
               

                

                DateTime rslnDate = Convert.ToDateTime(txtDate.Text);
                // procedure CreateComplaint(EmployeeNo: Text; description: Text; resolutionNotes:text; handledBy: text; resolutionDate:Date; resolutionFeedback: Text) 
                string response = webportals.CreateComplaint(username, description, reslnNote, handledBy, rslnDate, feedback);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string GrievanceId = responseArr[1];
                       
                        //TODO: Sent email to hr
                        //string hrEmail = "annenamwaya001@gmail.com";
                        /*
                        string hrEmail = "annenamwaya001@gmail.com";
                        string subject = "Portal - Complaint/ Greivance Application";
                            string body = $"Dear HR Team," +
                                $"<br/><br/>" +
                                $"A new grievance has been submitted in the system. Please find the details below:" +
                                $"<br/><br/>" +
                                $"Grievance ID: {GrievanceId}" +
                                $"<br/>" +
                                $"Employee Name:{username} " +
                                $"<br/>" +
                                $"Date Submitted:{}" +
                                $"<br/>" +
                                $"Kindly review and take the necessary action at your earliest convenience." +
                                $"<br/>" +
                                $"Regards" +
                                $"<br/>" ;
                            MyComponents.SentEmailAlerts(hrEmail, subject, body);*/
                            SuccessMessage($"Complaint {GrievanceId} has been created/submitted successfully.");
                            return;
                        
                    }
                    else
                    {
                        Message("An error occured while applying for the leave. Please try again later!");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {

                Message("An error occurred: " + ex.Message);
            }
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "ComplaintListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

    }
}