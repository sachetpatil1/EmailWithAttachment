using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;

[HttpPost]
public ActionResult SendEmail()
{
    try
    {
        System.Web.HttpFileCollectionBase files = Request.Files;

        System.Net.Mail.MailMessage _objMail = new System.Net.Mail.MailMessage();

        string FromId = "SendFromEmail@xxx.com";

        // Set properties needed for the email
        _objMail.From = new MailAddress(FromId);

        string[] ToId = Request.Params["EmailTo"].Split(';');

        foreach (string ToEmail in ToId)
        {
            _objMail.To.Add(new MailAddress(ToEmail)); //Adding Multiple To email Id
        }

        if (Request.Params["EmailCC"] != null && Request.Params["EmailCC"] != "")
        {
            string[] CCId = Request.Params["EmailCC"].Split(';');

            foreach (string CcEmail in CCId)
            {
                _objMail.CC.Add(new MailAddress(CcEmail)); //Adding Multiple CC email Id
            }
        }

        if (Request.Params["EmailBCC"] != null && Request.Params["EmailBCC"] != "")
        {
            string[] CCId = Request.Params["EmailBCC"].Split(';');

            foreach (string EmailBccd in CCId)
            {
                _objMail.Bcc.Add(new MailAddress(EmailBccd)); //Adding Multiple CC email Id
            }
        }

        _objMail.Subject = Request.Params["EmailSubject"];
        _objMail.SubjectEncoding = Encoding.UTF8;
        _objMail.Body = Request.Params["EmailMessage"];
        _objMail.IsBodyHtml = true;
        _objMail.Priority = MailPriority.Normal;
        _objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

        for (int i = 0; i < files.Count; i++)
        {
            System.Web.HttpPostedFileBase file = files[i];

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = file.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                buffer = ms.ToArray();
            }

            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(new MemoryStream(buffer),file.FileName);
            _objMail.Attachments.Add(attach);
        }
        // Set the mail object's smtpserver property
        using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
        {
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(FromId, "password");

            smtpClient.Send(_objMail);
        }
        return Json("Mail sent Successfully!");
    }
    catch (Exception ex)
    {
        return Json("Error While sending the mail!");
    }
}