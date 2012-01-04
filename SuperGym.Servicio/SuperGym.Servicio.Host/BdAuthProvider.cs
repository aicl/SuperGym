using System;
using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Tablas;

namespace SuperGym.Servicio.Host
{
	public class BdAuthProvider:IAuthProvider
	{
		public BdAuthProvider ()
		{
		}
		
		public IDbConnectionFactory DbFactory{get; set;}
	
		
		private Usuario Validate(LoginData userData ){
			return DbFactory.Usuario(userData.UserName, userData.Password);
			
			
		}	
		#region IAuthProvider implementation
		public Session CreateSession (LoginData userData, string ipAddress, string userAgent)
		{
			var response = Validate(userData);
			if( response==default(Usuario))
				throw new UnauthenticatedException("Usuario No Autenticado");
			
			if( !response.Activo )
				throw new UnauthenticatedException("Usuario se encuentra inactivo");
			
			return AuthUserSession.Add(
			 new UserBase(){
				Name=response.Nombre,
				UserId=response.Id.ToString()
			 },
			 ipAddress,userAgent);
		}

		public Session GetValidSession(Guid sessionId)
		{
			if (sessionId == default(Guid))
				throw new UnauthorizedException("Se requiere Identificador de Sesion");
						
			Session authSession = AuthUserSession[sessionId];
			
			if ( authSession != default(Session) )
			{
				Console.WriteLine("ServiceAuthProvider sessionId {0}",authSession.Id);
				AuthUserSession.Refresh(authSession);
				return authSession;
			}

			throw new UnauthorizedException("No Existe Sesion con Id: " + sessionId);
		}
		
		
		public void RefreshSession(Guid sessionId){
			 AuthUserSession.Refresh(sessionId);
		}
		
		
		public void Logout(Guid sessionId){
			if (sessionId == default(Guid))
				throw new UnauthorizedException("Se requiere Identificador de Sesion");
			
			AuthUserSession.Remove(sessionId);
		}

		public IAuthUserSession AuthUserSession {
			get;set;
		}
		#endregion
}
}
;
