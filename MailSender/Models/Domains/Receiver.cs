using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MailSender.Models.Domains
{
    public class Receiver
    {
        public Receiver()
        {
            Emails = new Collection<Email>();
        }
        public int Id { get; set; }

        
        [Required(ErrorMessage = "Proszę wpisać adres e-mail odbiorcy.")]
        [Display(Name = "E-Mail odbiorcy:")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Email> Emails { get; set; }
    }
}