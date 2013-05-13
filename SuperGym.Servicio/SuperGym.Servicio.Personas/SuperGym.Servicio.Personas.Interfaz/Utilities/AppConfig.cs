using System;
using System.Collections.Generic;
using System.IO;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class AppConfig
	{
		public AppConfig ()
		{
		}
		
		public AppConfig(IResourceManager resources)
		{
			RootDirectory = resources.GetString("RootDirectory");
			PhotoDirectory = resources.GetString("PhotoDirectory");
			MetaFilesDirectory = resources.GetString("MetaFilesDirectory");
			PrintOutDirectory = resources.GetString("PrintOutDirectory");
			ReportConnection = resources.GetString("ReportConnection");
			
			MailFrom = resources.GetString("MailFrom");
			MailServerUrl = resources.GetString("MailServerUrl");
			MailServerUser = resources.GetString("MailServerUser");
			MailServerPassword = resources.GetString("MailServerPassword");
			MailServerPort= resources.Get<int>("MailServerPort",587); 
			MailServerEnableSsl= resources.Get<bool>("MailServerEnableSsl",true);

			GymName = resources.Get<string> ("GymName", "GymName");

			LongitudFactura=resources.Get<int>("LongitudFactura",9);
			
			string dow = resources.Get<string>("DiaDeCierre", "SABADO").ToUpper();
			
			switch(dow){
			case "1":
			case "LUNES":
				DiaDeCierre= DayOfWeek.Monday;
				break;
			case "2":
			case "MARTES":
				DiaDeCierre= DayOfWeek.Tuesday;
				break;	
			case "3":
			case "MIERCOLES":
				DiaDeCierre= DayOfWeek.Wednesday;
				break;	
			case "4":
			case "JUEVES":
				DiaDeCierre= DayOfWeek.Thursday;
				break;	
			case "5":
			case "VIERNES":
				DiaDeCierre= DayOfWeek.Friday;
				break;	
			case "6":
			case "SABADO":
				DiaDeCierre= DayOfWeek.Saturday;
				break;	
			case "0":
			case "7":
			case "DOMINGO":
				DiaDeCierre= DayOfWeek.Sunday;
				break;	
			default:
				DiaDeCierre= DayOfWeek.Saturday;
				break;
			}
			
		}

		public string RootDirectory { get; set; }
		public string PhotoDirectory { get; set; }
		public string MetaFilesDirectory { get; set; }
		public string PrintOutDirectory { get; set; }
		public string ReportConnection { get; set; }
		
		public string MailFrom { get; set;}
		public string MailServerUrl { get; set;}
		public string MailServerUser { get; set;}
		public string MailServerPassword { get; set;}
		public int MailServerPort { get; set;}
		public bool  MailServerEnableSsl { get; set;}
		public int LongitudFactura {get; set;}
		public DayOfWeek DiaDeCierre { get; set;}

		public string GymName { get; set; }
		
	}
}



