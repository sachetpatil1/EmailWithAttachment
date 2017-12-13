[HttpPost]
public ActionResult SendEmail()
{
    try
    {
        HttpFileCollectionBase files = Request.Files;
        
        MailMessage _objMail = new MailMessage();

        // Set properties needed for the email
        _objMail.From = new MailAddress(EmailFrom);

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
            HttpPostedFileBase file = files[i];

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

            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(buffer, file.FileName);
            _objMail.Attachments.Add(attach);
        }

        if (EmailAttachments != null)
        {
            foreach (EmailAttachment ma in EmailAttachments)
            {
                _objMail.Attachments.Add(ma.File);
            }
        }
        // Set the mail object's smtpserver property
        SmtpClient smtp = GetSmtp();
        smtp.Send(_objMail);
        return Json("Mail sent Successfully!");
    }
    catch (Exception ex)
    {
        return Json("Error While sending the mail!");
    }
}
