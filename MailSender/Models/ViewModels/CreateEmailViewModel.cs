using MailSender.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MailSender.Models.ViewModels
{
    public class CreateEmailViewModel
    {
        public Email Email { get; set; }
        public Pattern Pattern { get; set; }
        public List<Receiver> Receivers { get; set; }

        [Display(Name = "Załącznik:")]
        public HttpPostedFileBase File { get; set; }
    }
}
