using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using PruebaUsuarios.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PruebaUsuarios.Data.Repositorios
{
    public class CorreoRepository : ICorreo
    {
        public string Servidor { get; set; }
        public int Puerto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NombreCorreo { get; set; }
        public bool EnableSSL { get; set; }

        private Configuraciones Configuraciones;
        public CorreoRepository(IOptions<Configuraciones> conf)
        {
            Configuraciones = conf.Value;
            //this.Servidor = Configuraciones.Emailing.Servidor;
            //this.Email = Configuraciones.Emailing.Email;
            //this.Password = Configuraciones.Emailing.Password;
            //this.Puerto = Configuraciones.Emailing.Puerto;
            //this.EnableSSL = Configuraciones.Emailing.EnableSSL;
            //this.NombreCorreo = Configuraciones.Emailing.NombreCorreo;
        }

        public bool EnviarCorreo(string subject, string CuerpoMensaje, string toEmail, string ccEmail)
        {
            if (Configuraciones != null)
            {
                string servidor = Configuraciones.Emailing.Servidor;
                int puerto = Configuraciones.Emailing.Puerto;
                string fromemail = Configuraciones.Emailing.Email;
                string pass = Configuraciones.Emailing.Password;
                string nomCorreo = Configuraciones.Emailing.NombreCorreo;
                bool enableSSL = Configuraciones.Emailing.EnableSSL;

                string[] CorreosMultiples = toEmail.Split(';');

                var message = new MailMessage
                {
                    From = new MailAddress(nomCorreo + " <" + fromemail + ">"),
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    HeadersEncoding = Encoding.UTF8,
                    Priority = MailPriority.High,
                    //Body = CuerpoMensaje,
                    //To = { new MailAddress(toEmail) },
                    Subject = subject
                };

                int i = 0;

                foreach (string email in CorreosMultiples)
                {
                    i = i + 1;

                    if (i < 50)
                        message.To.Add(new MailAddress(email));
                    else
                        message.CC.Add(new MailAddress(email));
                }

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(CuerpoMensaje, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html);
                //htmlView.LinkedResources.Add(img);
                message.AlternateViews.Add(htmlView);

                SmtpClient cliente = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(fromemail, pass),
                    Port = puerto,
                    Host = servidor,
                    //UseDefaultCredentials = false,
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = enableSSL
                };

                try
                {
                    cliente.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else { 
                return false; 
            }
        }

        public bool EnviarCorreo(string subject, string CuerpoMensaje, string toEmail, string ccEmail, MemoryStream ms, string namedata)
        {
            if (Configuraciones != null)
            {

                string servidor = Configuraciones.Emailing.Servidor;
                int puerto = Configuraciones.Emailing.Puerto;
                string fromemail = Configuraciones.Emailing.Email;
                string pass = Configuraciones.Emailing.Password;
                string nomCorreo = Configuraciones.Emailing.NombreCorreo;
                bool enableSSL = Configuraciones.Emailing.EnableSSL;

                string[] CorreosMultiples = toEmail.Split(';');
                string[] CorreosccMultiples = ccEmail.Split(';');


                var message = new MailMessage
                {
                    From = new MailAddress(nomCorreo + " <" + fromemail + ">"),
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    HeadersEncoding = Encoding.UTF8,
                    Priority = MailPriority.High,
                    //Body = CuerpoMensaje,
                    //To = { new MailAddress(toEmail) },
                    Subject = subject

                };

                foreach (string ccemail in CorreosccMultiples)
                {
                    message.CC.Add(new MailAddress(ccemail));
                }

                int i = 0;
                foreach (string email in CorreosMultiples)
                {
                    i = i + 1;

                    if (i < 50)
                        message.To.Add(new MailAddress(email));
                    else
                        message.CC.Add(new MailAddress(email));
                }

                //LinkedResource img = new LinkedResource(rutaimg, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //img.ContentId = "imagen";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(CuerpoMensaje, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html);
                //htmlView.LinkedResources.Add(img);
                message.AlternateViews.Add(htmlView);
                message.Attachments.Add(new Attachment(ms, namedata + ".pdf", "application/pdf"));

                SmtpClient cliente = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(fromemail, pass),
                    Port = puerto,
                    Host = servidor,
                    //UseDefaultCredentials = false,
                    //DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = enableSSL

                };

                try
                {
                    cliente.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else { return false; }
        }
    }
}
