using System.Net.Mail;
using System.Net;
using EmailSenderSMTP;
using Diplom2.Db;
using System;
using Diplom2.DTO;
using Microsoft.EntityFrameworkCore;

namespace Diplom2
{
    public class Mail
    {
        
        SenderSMTP client = new SenderSMTP(MailService.MailRu, "privalenkomaria4@mail.ru", "ErFJEhsr6Hynwt5iKqwd", "zalupa");

        //ErFJEhsr6Hynwt5iKqwd


        RegistUser regist = new RegistUser();



        private static Random random = new Random();  
        public string CurrentCode { get; private set; } 

        

        public string RandomizeCode()
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            CurrentCode /*regist.kod*/ = code; 
            return CurrentCode; 
        }

        private DiplomContext _context;
     


        internal async Task Send(RegistUser user)
        {
            string name = "zalupa";
            string subject = "галя шлюха";
            
            string bodyText = RandomizeCode();
            user.kod = bodyText;
            //_context.Update(user);
            //await _context.SaveChangesAsync();



            MailMessage message = client.CreateMailMessageBodyIsText(user.EmailUser, name, subject, bodyText );
            client.SendMail(message);




        }
       

        //SmtpClient emailService = new SmtpClient();
        //string hostmail = "ilchenkor1135@suz-ppk.ru";
        //public Mail()
        //{
        //    emailService.Host = "smtp.beget.com";
        //    emailService.Port = 25;
        //    //465 - обычное защищенное соединение
        //    //587 - антиспам для почт(чтобы не закинуло в спам сообщения) защищенное соединение
        //    emailService.Credentials = new NetworkCredential(hostmail, "MBx&QIF9");
        //    emailService.EnableSsl = false;
        //}

        //internal async Task Send(string? mail, string v2, string v3)
        //{
        //    emailService.Send(hostmail, mail, v2, v3);
        //}
    }
}
