using BC;
using INSPECTORATEStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace INSPECTORATEStaff
{
    public class MyComponents
    {
        public static SqlConnection connToNAV;

        #region Get reportpath

        public static string ReportsPath()
        {
            string currDir = "";
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String Root = Directory.GetCurrentDirectory();
            currDir = Root;

            return currDir;
        }
        #endregion

        public static string Company_Name = "INSPECTORATE";

        public static void SendEmailAlerts(string body, string recepient, string subject)
        {

            try
            {
                var mail = new MailMessage();
                var smtpServer = new SmtpClient("smtp.gmail.com");

                mail.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"), "@gmail.com>"));
                mail.From = new MailAddress("erp@maseno.ac.ke");

                // mail.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"), "@rti.ac.ke>"));
                // mail.From = new MailAddress("training@rti.ac.ke");

                mail.To.Add(recepient);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtpServer.Port = 25;
                smtpServer.Credentials = new System.Net.NetworkCredential("erp", "K9cyGS&p");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("training", "rtitraining");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

            }
            catch (Exception ex2)
            {
                ex2.Data.Clear();
            }
        }

        public static Boolean SendEmailAlert(string body, string recepient, string subject, string attachmentFilename)
        {
            Boolean x = false;

            string a = "";
            try
            {

                string SMTPHost = "smtp.gmail.com";
                // string fromAddress = "info@mwalimusacco.com";
                string fromAddress = "erp@maseno.ac.ke";
                string toAddress = recepient;
                System.Net.Mail.MailMessage mail_ = new System.Net.Mail.MailMessage();
                mail_.To.Add(toAddress);
                mail_.Subject = subject;
                mail_.From = new System.Net.Mail.MailAddress(fromAddress);
                mail_.Body = body;
                mail_.IsBodyHtml = true;
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(SMTPHost, 587);

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("erp", "K9cyGS&p"),
                    EnableSsl = true
                };
                smtp.Send(mail_);

                x = true;
                a = "Sent";

            }
            catch (Exception ex2)
            {
                a = ex2.ToString();
                ex2.Data.Clear();


            }
            //to correct the mail server to be able to send email to the users
            //x = true;
            return x;
        }
        public static void SendMyEmail(string address, string subject, string message)
        {
            try
            {
                string email = "dynamicselfservice@gmail.com";
                string password = "{[*^5~8(_+?Fd";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
                // throw;
            }
        }
        public static string FormatDateWithoutSymbols(DateTime Date2Format)
        {
            string s = "";

            try
            {
                string y, m, d, hr, mn, sc;

                y = Date2Format.Year.ToString();

                m = Date2Format.Month.ToString();
                if (m.Length == 1) m = "0" + m;

                d = Date2Format.Day.ToString();
                if (d.Length == 1) d = "0" + d;

                hr = Date2Format.Hour.ToString();
                if (hr.Length == 1) hr = "0" + hr;

                mn = Date2Format.Minute.ToString();
                if (mn.Length == 1) mn = "0" + mn;

                sc = Date2Format.Second.ToString();
                if (sc.Length == 1) sc = "0" + sc;

                s = y + m + d + " " + hr + mn + sc;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

            return s;
        }


        public static void SendEmailAlert(string body, string recepient)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //SmtpClient SmtpServer = new SmtpClient("mail.rti.ac.ke");

                mail.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"), "@gmail.com>"));
                mail.From = new MailAddress("erp@maseno.ac.ke");

                // mail.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), ".", DateTime.Now.ToString("HHmmss"), "@rti.ac.ke>"));
                // mail.From = new MailAddress("training@rti.ac.ke");

                mail.To.Add(recepient);
                mail.Subject = "";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("erp", "K9cyGS&p");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("training", "rtitraining");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

            }
            catch (Exception ex2)
            {

                ex2.Data.Clear();
            }
        }
        public static string BaseSiteUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
                return baseUrl;
            }
        }
        public static Control FindControlRecursive(Control Root, string Id)
        {

            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);

                if (FoundCtl != null)

                    return FoundCtl;
            }
            return null;
        }

        #region getconnToNAV
        public static SqlConnection getconnToNAV()
        {
            try
            {
                if (connToNAV == null || connToNAV.State == ConnectionState.Closed)
                {
                    connToNAV = new SqlConnection(@"Data Source = 102.213.176.86; Initial Catalog = INSPECTORATE; User ID = webportals; Password = login*4");

                    connToNAV.Open();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return connToNAV;
        }
        #endregion
        public static WebPortals ObjNav
        {
            get
            {
                var ws = new WebPortals();

                try
                {
                    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"],
                        ConfigurationManager.AppSettings["W_PWD"], ConfigurationManager.AppSettings["DOMAIN"]);

                    //ws.UseDefaultCredentials = true;
                    ws.Credentials = credentials;
                    ws.PreAuthenticate = true;

                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return ws;
            }
        }
        public static BC.NAV OdataService
        {
            get
            {
                var serviceRoot = "http://phil:7048/BC180/ODataV4/Company('BOHEMIAN%20FLOWERS%20LTD')/";
                var context = new NAV(new Uri(serviceRoot));

                try
                {
                    context.BuildingRequest += Context_BuildingRequest;

                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return context;
            }
        }
        private static void Context_BuildingRequest(object sender, Microsoft.OData.Client.BuildingRequestEventArgs e)
        {
            //e.RequestUri = new Uri(e.RequestUri.ToString().Replace("V4/", "V4/Company('MASENO%20UNIVERSITY')/"));
            e.Headers.Add("Authorization", "Basic anVtYTpKdW1hQDIwMTk=");
        }

        public static bool ValidNumber(string numberToValidate)
        {
            bool b = false;
            try
            {
                numberToValidate = ValidateNumber(numberToValidate);

                if (numberToValidate.Length > 0)
                {
                    //throw exception if not double number.
                    double d = Convert.ToDouble(numberToValidate);

                    //success/valid double number
                    b = true;
                }
            }
            catch (Exception ex)
            {
                //cSite.SendErrorToDeveloper(ex);
                ex.Data.Clear();
            }
            return b;
        }

        public static string ValidateNumber(string Entry)
        {
            string r = Entry;

            try
            {
                Entry = ValidateEntry(Entry);

                string s = ",()";//sql illegal entry characters

                Entry = Entry.Trim();

                char[] c = s.ToCharArray();

                for (int i = 0; i < c.Length; i++)
                {
                    Entry = Entry.Replace(c[i].ToString(), "");
                }
                r = Entry;
            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }
        public static string ValidateEntry(string Entry)
        {
            string r = Entry;
            try
            {
                if (Entry.Length > 250) Entry = Entry.Substring(0, 250);

                string s = "'";//sql illegal entry characters

                Entry = Entry.Trim();//remove spaces

                char[] c = s.ToCharArray();

                for (int i = 0; i < c.Length; i++)
                    if (Entry.Contains(c[i].ToString()))
                    {
                        //Entry = Entry.Replace(c[i].ToString(), "" );//blank
                        Entry = Entry.Replace(c[i].ToString(), "\'" + c[i].ToString());//escape character
                    }

                s = "--";//sql illegal entry characters

                if (Entry.Contains(s))
                    Entry = Entry.Replace(s, "");//blank

                r = Entry;
            }
            catch (Exception)
            {
                throw;
            }
            return r;
        }
        public static string UserID
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [User ID] FROM [" + MyComponents.Company_Name + "$User Setup$bf65ec43-e187-4491-8d5f-10241a637a81] WHERE [Employee No_]= '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = dr["User ID"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }

        public static string Customer
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Name] FROM [" + MyComponents.Company_Name + "$Customer$437dbf0e-84ff-417a-965d-ed2bb9650972] WHERE [No_]= '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = dr["Name"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }
        public static string Department
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Department Code] FROM [" + MyComponents.Company_Name + "$HRM-Employee C$9f74d069-84a8-407d-a2e1-68f3423ff5fb] WHERE [No_] = '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = dr["Department Code"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }
        public static string AppRoverPF(string userID)
        {
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Staff No] FROM [" + MyComponents.Company_Name + "$User Setup$437dbf0e-84ff-417a-965d-ed2bb9650972] WHERE [User ID] = '" + userID + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = dr["Staff No"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }
        public static string EmployeeGender
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Gender] FROM [" + MyComponents.Company_Name + "$HRM-Employee C$9f74d069-84a8-407d-a2e1-68f3423ff5fb] WHERE No_ = '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = (Convert.ToInt32(dr["Gender"])).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }
        public static string EmployeeGrade
        {
            get
            {
                string s = "";

                try
                {
                    string strSQL = String.Format("SELECT [Grade] FROM [" + MyComponents.Company_Name + "$HRM-Employee C$9f74d069-84a8-407d-a2e1-68f3423ff5fb] WHERE No_ = '" + HttpContext.Current.Session["username"].ToString() + "'");
                    SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            s = (Convert.ToInt32(dr["Grade"])).ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return s;
            }
        }       
        public static bool IsNumeric(string no)
        {
            double result;
            if (double.TryParse(no, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
