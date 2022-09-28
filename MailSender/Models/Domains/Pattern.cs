using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MailSender.Models.Domains
{
    public class Pattern
    {
        public int Id { get; set; }
        [Display(Name = "Nadawca:")]
        public string SenderName { get; set; }
        public string UserId { get; set; }
    }
}