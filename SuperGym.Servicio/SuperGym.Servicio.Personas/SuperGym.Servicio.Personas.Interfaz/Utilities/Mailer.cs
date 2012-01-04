using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class Mailer
	{
		
				
		public MailMessage Message {get;set;}
		
		private SmtpClient SmtpServer {get ;set;}
		
		public Mailer ( AppConfig config )
		{
			Message = new MailMessage();
			Message.From = new MailAddress( config.MailFrom);
						
			SmtpServer = new SmtpClient(config.MailServerUrl);
			SmtpServer.Port = 587;
			SmtpServer.Credentials = 
				new System.Net.NetworkCredential(config.MailServerUser, config.MailServerPassword);
			SmtpServer.EnableSsl = config.MailServerEnableSsl;
			ServicePointManager.ServerCertificateValidationCallback =
				delegate(object s, X509Certificate certificate,
				X509Chain chain, SslPolicyErrors sslPolicyErrors)
				{ return true; };
			
		}
		
		
		public void Send(){
			SmtpServer.Send(Message);
		}
		
		
		
	}
}

