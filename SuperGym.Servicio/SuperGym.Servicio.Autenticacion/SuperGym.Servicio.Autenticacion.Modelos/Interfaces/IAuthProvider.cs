

using System;


namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public interface IAuthProvider
	{
						
		Session CreateSession(LoginData userData, string ipAddress, string userAgent);

		Session GetValidSession(Guid sessionId);
		
		IAuthUserSession AuthUserSession{ get; set; }
		
		void RefreshSession(Guid sessionId);
		
		void Logout(Guid sessionId);
	}

	
}

