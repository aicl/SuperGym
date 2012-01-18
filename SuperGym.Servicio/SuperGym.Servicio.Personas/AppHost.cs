using System;
using System.Configuration;

using System.IO;
using Funq;
using ServiceStack.Common.Utils;
using ServiceStack.Configuration;
using ServiceStack.DataAccess;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.WebHost.Endpoints;

using ServiceStack.ServiceInterface;

using ServiceStack.Redis;
using ServiceStack.CacheAccess;

using ServiceStack.OrmSimple;
using ServiceStack.OrmSimple.Firebird;


using SuperGym.Servicio.Host;
using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Servicio.Personas.Interfaz;
using ServiceStack.Logging.Log4Net ;



namespace SuperGym.Servicio.Personas
{
	public class AppHost:AppHostBase
	{
		private static ILog log;

		public AppHost()
			: base("SuperGym - Servicio", typeof(PersonaGetService).Assembly)
		{
			log4net.Config.XmlConfigurator.Configure(
				new System.IO.FileInfo(ConfigurationManager.AppSettings.Get("Log4NetConfigFile")));
			LogManager.LogFactory = new  Log4NetFactory();
			log = LogManager.GetLogger(typeof (AppHost));
		}
		
		public override void Configure(Container container)
		{
			string sc = ConfigurationManager.AppSettings.Get("DbConnection");
			
			string rchost= ConfigurationManager.AppSettings.Get("SessionRedisHost");
			rchost= (string.IsNullOrEmpty(rchost))? "localhost:6379": rchost;
			
			
			string rcpassword= ConfigurationManager.AppSettings.Get("SessionRedisPassword");
			
			string rcdbs=  ConfigurationManager.AppSettings.Get("SessionRedisDb");
			int rcdb;
			if(! int.TryParse(rcdbs, out rcdb) ) rcdb= 10; 
			
			string sstout = ConfigurationManager.AppSettings.Get("SessionTimeout");
			int sessionTimeout;
			if(! int.TryParse(sstout, out sessionTimeout) ) sessionTimeout= 60*8;
			
			var cacheClient= new  BasicRedisClientManager( new string[]{rchost}, new string[]{rchost}, rcdb);
			cacheClient.GetClient().Password= rcpassword;
						
			container.Register<IAuthProvider>( new BdAuthProvider()
			{
				DbFactory=new ConnectionFactory(sc,  FirebirdDialectProvider.Instance),
				AuthUserSession= new UserSession()
				{
					CacheClient= cacheClient,
					TimeOut=sessionTimeout,
				}
			});
		
			
			string phost= ConfigurationManager.AppSettings.Get("CacheHost");
			phost = (string.IsNullOrEmpty(phost))?"localhost:6379":phost;	
			
			string pdbs= ConfigurationManager.AppSettings.Get("CacheDb");				
			int pdb ;
			if(! int.TryParse(pdbs, out pdb) ) pdb= 9; 
			
			
			string ppassword= ConfigurationManager.AppSettings.Get("CachePassword");
						
			var p = new PooledRedisClientManager(new string[]{phost}, new string[]{phost}, pdb); 
			p.GetClient().Password= ppassword;
			container.Register<ICacheClient>( p);
			container.Register<IDbConnectionFactory>(
				new ConnectionFactory(sc,  FirebirdDialectProvider.Instance)
			);
			
			var config = new AppConfig(new ConfigurationResourceManager());
			
			container.Register(config);

			if (!Directory.Exists(config.PhotoDirectory))
			{
				Directory.CreateDirectory(config.PhotoDirectory);
			}
			
			
			//Mailer mailer = new Mailer(config);
			//container.Register(mailer);	
			//HtmlReport.Register(this);
				
			
			//Permit modern browsers (e.g. Firefox) to allow sending of any REST HTTP Method
			base.SetConfig(new EndpointHostConfig
			{
				GlobalResponseHeaders =
					{
						{ "Access-Control-Allow-Origin", "*" },
                    	{ "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
						{ "Access-Control-Allow-Headers","X-Requested-With"}
					},
			});
			
			log.InfoFormat("AppHost Configured: " + DateTime.Now);
		}
	}
}

