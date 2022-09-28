using EmailSender.Extensions;
using MailSender.Core;
using MailSender.Models.Domains;
using MailSender.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;

namespace MailSender.Models.Repositories
{
    public class EmailRepository
    {        
        internal void AddEmail(CreateEmailViewModel vm,byte[] uploadedFile)
        {
            using (var context=new ApplicationDbContext())
            {                
                var email = vm.Email;
                email.Receiver.Email = email.Receiver.Email.ToLower();
                var receiver = context.Receivers.Where(x => x.Email == email.Receiver.Email && x.UserId==email.UserId).FirstOrDefault();
                if (receiver != null)
                    email.Receiver = receiver;
                context.Patterns.First(x => x.UserId == email.UserId).SenderName = vm.Pattern.SenderName;
                if (uploadedFile != null)
                {
                    var attachmentId = SaveAttachmentToDatabase(uploadedFile, vm.File.FileName);
                    email.AttachmentId = attachmentId;
                }
                context.Emails.Add(email);
                context.SaveChanges();
            }
        }
        private int SaveAttachmentToDatabase(byte[]uploadedFile,string fileName)
        {
            using (var context = new ApplicationDbContext())
            {
                var attachment = new EmailAttachment() { Attachment = uploadedFile, FileName = fileName };
                context.Attachments.Add(attachment);
                context.SaveChanges();
                int attachmentId = attachment.Id;
                return attachmentId;
            }
        }

        internal Pattern GetPattern(string userId)
        {
            using (var context=new ApplicationDbContext())
            {                
               var pattern= context.Patterns.SingleOrDefault(x => x.UserId == userId);

                if (pattern != null)
                    return pattern;

                pattern = new Pattern() { UserId = userId };
                context.Patterns.Add(pattern);
                context.SaveChanges();
                return pattern;
            }
        }

        public async Task SendEmail(CreateEmailViewModel vm,byte[] uploadedFile)
        {
            var body = GenerateHtml(vm.Email.Message, vm.Pattern.SenderName);
            Attachment attachment = null;
            var emailParams = new MailSender.Core.Email(new EmailParams
            {
                HostSmtp = WebConfigurationManager.AppSettings["HostSmtp"],
                Port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]),
                EnableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["EnableSsl"]),
                SenderName = vm.Pattern.SenderName,
                SenderEmail = WebConfigurationManager.AppSettings["SenderEmail"],
                SenderEmailPassword = GetPassword()
            });
            if (uploadedFile != null)
            {
                MemoryStream memoryStream = new MemoryStream(uploadedFile);
                attachment = new Attachment(memoryStream, vm.File.FileName);
            }
            await emailParams.Send(vm.Email.Title, body, vm.Email.Receiver.Email,attachment);
        }
        
        private string GetPassword()
        {
            Cipher.StringCipher stringCipher = new Cipher.StringCipher("60AF001A-12A5-44E1-B081-E4594FF62167");
            var password = ConfigurationManager.AppSettings["SenderEmailPassword"];
            if (password.StartsWith("encrypt:"))
            {
                password = password.Replace("encrypt:", "");
                password = stringCipher.Encrypt(password);
                var configFile = WebConfigurationManager.OpenWebConfiguration("~");
                configFile.AppSettings.Settings["SenderEmailPassword"].Value = password;
                configFile.Save();
            }
            password = stringCipher.Decrypt(password);
            return password;
        }

        public string GenerateHtml(string message,string senderName)
        {
            return $"<p>{message}</p><br /><h4>{senderName}</h4>";
        }
    }
}