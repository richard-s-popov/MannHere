using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using MannHere.Models;

namespace MannHere.Controllers.Home
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendOrder(Order model)
        {
            var email = new EmailMessage
                {
                    DisplayNameFrom = "Заказ тренинга",
                    From = "ordermann@rbprofit.ru",
                    To = "ordermann@rbprofit.ru",
                    Subject = string.Format("Заявка на треннинг \"{0}\"", model.Training),
                    Message = string.Format("Заявка на получение: <b>{0}</b><br/>" +
                                            "Имя: <b>{1}</b><br/>" +
                                            "Телефон: <b>{2}</b><br/>" +
                                            "Email: <b>{3}</b><br/>" +
                                            "Комментарий: <b>{4}</b>", 
                                            model.Training, 
                                            model.Name, 
                                            model.Phone, 
                                            model.Email, 
                                            model.Comment)
                };

            SendMessage(email, "ordermann@rbprofit.ru", "123456aaa111", "smtp.yandex.ru", 587, true);

            return Json(true);
        }

        private void SendMessage(EmailMessage message, string username, string password, string host, int port, bool enableSsl)
        {
            var from = new MailAddress(message.From, message.DisplayNameFrom);
            var to = new MailAddress(message.To);

            var mm = new MailMessage(from, to)
            {
                Subject = message.Subject,
                Body = message.Message,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };

            var credentials = new NetworkCredential(username, password);
            var sc = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = credentials
            };

            sc.Send(mm);
        }
    }
}
