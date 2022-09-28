using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailSender.Models.Domains
{
    public class EmailAttachment
    {
        public int Id { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
    }
}