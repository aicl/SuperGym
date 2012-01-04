using System;
using ServiceStack.CacheAccess;


namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public interface IAuthUserSession
	{
		double TimeOut{get;set;}
		
		Session Add(UserBase response, string ipAddress, string userAgent);
				
		void Remove (Guid id);	
				
		Session this[Guid id]  { get;set;	}
		
		void Refresh(Guid id);
		
		void Refresh(Session userClientSession);
				
		ICacheClient CacheClient{get;set;}

		
	}
}



