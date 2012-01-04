using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	//[CustomAuth( Action="", ClientType="" )]
	public class LogoutData:IRequireAuthentication
	{
		public LogoutData ()
		{
		}
		
		#region IRequireAuthentication implementation
		public Guid SessionId {	get ; set;}
		
		#endregion
	}
}

