using System;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Servicio.Personas.Modelos;

namespace EstadoResultados
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string tipo;
			
			DateTime desde;
			DateTime hasta;
			DateTime hoy = DateTime.Today;
			
			if(args.Length>0) 
				tipo= args[0].ToUpper();
			else
				tipo= "SEMANAL";
			
			
			if(tipo=="SEMANAL"){
				desde= hoy.AddDays(-8);
				hasta= hoy.AddDays(-2);
			}
			
			else if( tipo=="MENSUAL" ){
				int mes = hoy.Month >1 ? hoy.Month-1: 12 ;
				int anio =  hoy.Month >1?  hoy.Year: hoy.Year-1;
				
				desde= new DateTime(anio, mes, 1);
				hasta= new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes) );
			}
			else{
				Console.WriteLine("uso mono EstadoResultados.exe SEMANAL\nuso mono EstadoResultados.exe MENSUAL");
				return;
			}
			
			
			string url = "http://localhost/autenticacion"; 
			string urlServicio = "http://localhost/servicio"; 
			
			using (JsonServiceClient client = new JsonServiceClient(url))
			{
				//var request = new LoginData { UserName="sistema", Password="billazoperrazo" };
				var request = new LoginData { UserName="yadira", Password="ctyd1525" };
				var response = client.Send<LoginResponse>(request);
				
				if(!response.Success){
					Console.WriteLine(response.ResponseStatus.Message);
					return ;
				}
				
				using (JsonServiceClient cl = new JsonServiceClient(urlServicio))
				{
					CajaConsolidadoGet er = new CajaConsolidadoGet()
					{
						Desde= desde,
						Hasta= hasta,
						SessionId= response.Id,
						SendMail=true,
					};
					
					CajaConsolidadoGetResponse r = cl.Send<CajaConsolidadoGetResponse>(er);
					if( ! r.Success ){
						Console.WriteLine(r.ResponseStatus.Message);
					}
					
					LogoutData lo = new LogoutData(){
						SessionId= response.Id
					};
					
					client.Send<LogoutResponse>(lo);
				}
				
			}
			Console.WriteLine("This is The End my friend");
		}
	}
}
