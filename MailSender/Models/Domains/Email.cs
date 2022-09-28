using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MailSender.Models.Domains
{
    public class Email
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Proszę podać tytuł.")]
        [Display(Name = "Tytuł:")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Należy wpisać treść.")]
        [Display(Name = "Treść wiadomości:")]
        public string Message { get; set; }

        [Display(Name = "Odbiorca:")]        
        public int ReceiverId { get; set; }
        public Receiver Receiver { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int AttachmentId { get; set; }
        public EmailAttachment Attachment { get; set; }
    }
}