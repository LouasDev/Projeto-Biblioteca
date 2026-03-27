using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.IO;

namespace BibliotecaApp.Services
{
    public static class EmailService
    {
        private static readonly string remetente = "biblioteca.gastaovalle@gmail.com"; // coloque o seu
        private static readonly string senha = "kbvv piip qtrs wpmm"; // senha de app do Gmail


        public static void Enviar(string destinatario, string assunto, string mensagem, string caminhoAnexo = null)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(remetente, senha);
                    smtp.EnableSsl = true;

                    var mail = new MailMessage(remetente, destinatario, assunto, mensagem)
                    {
                        IsBodyHtml = true
                    };

                    if (!string.IsNullOrEmpty(caminhoAnexo) && File.Exists(caminhoAnexo))
                        mail.Attachments.Add(new Attachment(caminhoAnexo));

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar e-mail: " + ex.Message);
            }
        }

    }
}