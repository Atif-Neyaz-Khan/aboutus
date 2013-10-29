using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class aboutus_content : System.Web.UI.Page
{
    protected string tempCountry = "";
    protected bool flag = false;
    protected bool helper = true;
    

    protected void Page_PreRender(object sender, EventArgs e)
    {
            if (flag)
            {
                List<String> list = new List<string>();
                List<String> values = new List<string>();
				
				List<Attachment> collection = new List<Attachment>();
                                
                HttpPostedFile flagfile = Request.Files["coverLetter"];
                if (flagfile != null && flagfile.ContentLength > 0)
                {
                    collection.Add(new Attachment(flagfile.InputStream, flagfile.FileName));
                }
								
                list.Add("$MainHeading$");
                values.Add("Masterkey Career");

                list.Add("$JobTitle$");
                if (!String.IsNullOrEmpty(Request["JobTitle"]))
                    values.Add(Request["JobTitle"]);
                else
                    values.Add(String.Empty);

                list.Add("$JobCategory$");
                if (!String.IsNullOrEmpty(Request["JobCategory"]))
                    values.Add(Request["JobCategory"]);
                else
                    values.Add(String.Empty);

                list.Add("$JobLocation$");
                if (!String.IsNullOrEmpty(Request["JobLocation"]))
                    values.Add(Request["JobLocation"]);
                else
                    values.Add(String.Empty);

                list.Add("$DatePosted$");
                if (!String.IsNullOrEmpty(Request["DatePosted"]))
                    values.Add(Request["DatePosted"]);
                else
                    values.Add(String.Empty);

                list.Add("$Title$");
                if (!String.IsNullOrEmpty(Request["ddlTitle"]))
                    values.Add(Request["ddlTitle"]);
                else
                    values.Add(String.Empty);

                list.Add("$FirstName$");
                if (!String.IsNullOrEmpty(Request["txtFirstName"]))
                    values.Add(Request["txtFirstName"]);
                else
                    values.Add(String.Empty);

                list.Add("$LastName$");
                if (!String.IsNullOrEmpty(Request["txtLastName"]))
                    values.Add(Request["txtLastNAme"]);
                else
                    values.Add(String.Empty);

                list.Add("$Country$");
                if (!String.IsNullOrEmpty(Request["ddlCountry"]))
                    values.Add(Request["ddlCountry"]);
                else
                    values.Add(String.Empty);

                list.Add("$txtContactNo$");
                if (!String.IsNullOrEmpty(Request["txtContactNo"]))
                    values.Add(Request["txtContactNo"]);
                else
                    values.Add(String.Empty);


                list.Add("$EmailAddress$");
                if (!String.IsNullOrEmpty(Request["txtEmail"]))
                    values.Add(Request["txtEmail"]);
                else
                    values.Add(String.Empty);


                list.Add("$HowLong$");
                if (!String.IsNullOrEmpty(Request["txtHowLong"]))
                    values.Add(Request["txtHowLong"]);
                else
                    values.Add(String.Empty);

                HttpPostedFile flagfile1 = Request.Files["ResumeLetter"];
                if (flagfile1 != null && flagfile1.ContentLength > 0)
                {
                    collection.Add(new Attachment(flagfile1.InputStream, flagfile1.FileName));
                }
                
                string email = getFile(Server.MapPath("/Emails/Content.html"), list, values);
                SendMail(Request["txtEmail"], "careers@gomasterkey.com", "Careers Master Key", email.ToString(),collection);
            }
    }

    public string getFile(string path, List<String> list, List<String> values)
    {
        StreamReader reader = new StreamReader(path);
        string temp = reader.ReadToEnd();
        for (int i = 0; i < list.Count; i++)
            temp = temp.Replace(list[i], values[i]);
        reader.Close();
        reader.Dispose();
        return temp;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        tempCountry = Request["ddlCountry"];
        if (Request.RequestType.ToUpper() == "POST")
		{
            if (this.Session["CaptchaText"] != null && !String.IsNullOrEmpty(Request["txtCaptch"]))
            {
                if (this.Session["CaptchaText"].ToString().ToLower() == Request["txtCaptch"].ToLower())
                    flag = true;
                else
                {
                    string Link = "";
                    if (Request.UrlReferrer.AbsolutePath != "/aboutus/content.aspx")
                    {
                        string[] nc = Request.Form.AllKeys;
                        for (int i = 0; i < nc.Length; i++)
                        {
                            Link += "&" + nc[i] + "=" + Request.Form.GetValues(i)[0];
                        }
                        Response.Redirect(Request.UrlReferrer.ToString() + "?helper=true" + Link);
                    }
                    else
                    {
                        helper = false;
                        flag = false;
                    }
                }
            }
            else
            {
                string Link = "";
                if (Request.UrlReferrer.AbsolutePath != "/aboutus/content.aspx")
                {
                    string[] nc = Request.Form.AllKeys;
                    for (int i = 0; i < nc.Length; i++)
                    {
                        Link += "&" + nc[i] + "=" + Request.Form.GetValues(i)[0];
                    }
                    Response.Redirect(Request.UrlReferrer.ToString() + "?helper=true" + Link);
                }
                else
                {
                    helper = false;
                    flag = false;
                }
            }
		}
    }

    public void SendMail(string from, string to, string subject, string body, List<Attachment> att)
    {
        try
        {
            new Utility().SendEmailwithAttachments(from, to, subject, body, att);
            Response.Redirect("/aboutus/thanks.aspx");
        }
        catch (Exception exx)
        {
            Response.Write(exx.Message);
        }
    }
}





