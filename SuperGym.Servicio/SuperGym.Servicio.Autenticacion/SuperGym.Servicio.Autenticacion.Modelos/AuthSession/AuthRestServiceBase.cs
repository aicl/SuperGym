using System;
using System.Linq;
using System.Collections.Generic;

using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using ServiceStack.CacheAccess;
using ServiceStack.OrmSimple;
using ServiceStack.Redis;


namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class AuthRestServiceBase<TRequest> : RestServiceBase<TRequest>
	{
		public IAuthProvider AuthProvider { get; set; }

		public Session Session{get; protected set; }
		
		public ICacheClient CacheClient { get; set; }
		
		public IDbConnectionFactory DbFactory {get;set;}
		
		
		protected override void OnBeforeExecute (TRequest request)
		{
			Validate(request);
		} 

		private void Validate(TRequest request)
		{
			var requiresAuth = request as IRequireAuthentication;
			if (requiresAuth == null) return;
			
			Session = AuthProvider.GetValidSession(requiresAuth.SessionId);
			
						
			var attrs = request.GetType().GetCustomAttributes(typeof (CustomAuthAttribute), true);
			if (attrs.Length > 0)
			{
				var customAuth = (CustomAuthAttribute)attrs[0];

				if ( !customAuth.ClientType.IsNullOrEmpty() &&
				    (Session.ClientType != customAuth.ClientType ))
					throw new UnauthorizedException("Unauthorized. Requires ClientType: " + customAuth.ClientType);
			}
        }
		
		
		protected void ValidateUserAction(string userAction){
							
			if( !userAction.IsNullOrEmpty() ){
				var du = Session.GetUserData<DatosUsuario>();		
			    if ( du.Actividades.Count(r=> r.Nombre== userAction)==0 )
					throw new UnauthorizedException("Usuario no autorizado para ejecutar: " + userAction);
			}
		}
		
	}
}

