using MailSender.Models;
using MailSender.Models.Domains;
using MailSender.Models.Repositories;
using MailSender.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace MailSender.Controllers
{
    public class HomeController : Controller
    {
        private ReceiverRepository _receiverRepository = new ReceiverRepository();
        private EmailRepository _emailRepository = new EmailRepository();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Serwis E-mail.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Strona kontaktowa.";

            return View();
        }

        public ActionResult CreateEmail()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = PrepareCreateEmailViewModel(userId);
            return View(viewModel);
        }

        private CreateEmailViewModel PrepareCreateEmailViewModel(string userId)
        {
            Email email = new Email()
            {
                UserId = userId,
                Receiver = new Receiver() { UserId = userId }
            };

            return new CreateEmailViewModel()
            {
                Email = email,
                Receivers = _receiverRepository.GetReceivers(userId),
                Pattern = _emailRepository.GetPattern(userId)
            };
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEmail(CreateEmailViewModel vm)
        {
            var email = vm.Email;
            byte[] uploadedFile = null;
            if (vm.File != null)
            {
                uploadedFile = new byte[vm.File.InputStream.Length];
                vm.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
            }
            var userId = User.Identity.GetUserId();
            email.UserId = userId;

            if (!ModelState.IsValid)
            {
                vm = PrepareCreateEmailViewModel(userId);
                return View("CreateEmail", vm);
            }
            await _emailRepository.SendEmail(vm,uploadedFile);
            _emailRepository.AddEmail(vm, uploadedFile);
            TempData["MailSentMessage"] = "Wiadomość wysłana";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SetTempData(string value)
        {
            TempData["MailSentMessage"] = value;
            return new EmptyResult();
        }
    }
}