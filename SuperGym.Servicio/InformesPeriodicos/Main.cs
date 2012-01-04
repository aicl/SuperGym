using System;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

using SuperGym.Servicio.Autenticacion.Modelos;

namespace InformesPeriodicos
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string url = "http://localhost/autenticacion"; 
			
			using (JsonServiceClient client = new JsonServiceClient(url))
			{
				//var request = new LoginData { UserName="sistema", Password="billazoperrazo" };
				var request = new LoginData { UserName="yadira", Password="ctyd1525" };
				var response = client.Send<LoginResponse>(request);
				Console.WriteLine(response.Success);
				
				
			}
			
		}
	}
}
