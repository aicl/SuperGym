using System;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Autenticacion.Interfaz
{
	public class LogoutService:AuthServiceBase<LogoutData>
	{
		protected override object Run(LogoutData request){
			AuthProvider.Logout(request.SessionId);
			
			return new LogoutResponse(){
				ResponseStatus= new ResponseStatus(){
					Message="Hasta pronto...."
				}
			};
		}
	}
}

