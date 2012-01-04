using System;

using ServiceStack.Redis;
using ServiceStack.Common;
using ServiceStack.CacheAccess;

using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Host
{
	public class UserSession:IAuthUserSession
	{
		
		public UserSession ()
		{			
			TimeOut= 30;  //minutes
		}

		#region IAuhtUserSession implementation
		
		//minutes
		public Double TimeOut{get;set;}
				
		public Session Add (UserBase user, string ipAddress, string userAgent)
		{
			
			
			Session authSession=
				new Session(){
					Name=user.Name,
					UserId= user.UserId,
					Id= user.Id==default(Guid)? Guid.NewGuid():user.Id,
					ExpiresAt= DateTime.UtcNow.AddMinutes( TimeOut ),
					IpAddress= ipAddress,
					UserAgent= userAgent
				};
				
				
			if (authSession.Name == "Admin" || authSession.Name=="angel.colmenares")
			{
				authSession.ClientType = "SuperUser";
			}
			else{
				authSession.ClientType = "user";
			}
			
	
			Console.WriteLine("AuthUserSession.Add 2:{0} ",
			                  UrnId.Create<Session>(authSession.Id.ToString()) );
			
			CacheClient.Add<Session>(authSession.ToCacheKey(), authSession, authSession.ExpiresAt);
			Console.WriteLine("UserSession Add CacheClient: '{0}'", CacheClient);
			Console.WriteLine("UserSession Add CacheClient: '{0}'", CacheClient as IRedisClient);
			authSession.SetRedisClient( (CacheClient as IRedisClientsManager ).GetClient()); //CacheClient as IRedisClient);
						
			return authSession;
		}

		
		public void Remove (Guid sessionId)
		{
			Console.WriteLine("AuthUserSession.Remove:{0} - cacheKey ",Session.ToCacheKey(sessionId) );
			var rc =(CacheClient as IRedisClientsManager).GetClient();//CacheClient as IRedisClient;
			Console.WriteLine("AuthUserSession.Remove:{0} redisclient ",rc );
			if(rc != null){
				var keys = rc.SearchKeys(Session.SearchKeyString(sessionId));
				if(keys!=null & keys.Count>0)  CacheClient.RemoveAll(keys);
			}
			else{
				CacheClient.Remove(Session.ToCacheKey(sessionId));
			}
			
		}

		

		public Session this[Guid sessionId] {
			get {
				Console.WriteLine("UserSession get CacheClient: '{0}'", CacheClient);
				var authSession= CacheClient.Get<Session>(Session.ToCacheKey( sessionId));
				if(authSession !=default(Session)) authSession.SetRedisClient( (CacheClient as IRedisClientsManager).GetClient() );
				return authSession;
			}
			set {
				if( sessionId != value.Id) 
					throw new Exception(string.Format("sessionId {0}!={1} value.Id ", sessionId, value.Id) );
				CacheClient.Replace<Session>(Session.ToCacheKey(sessionId), value, value.ExpiresAt);
			}
		}
		
		public void Refresh(Guid id){
			Session a =this[id];
			if (a != default(Session) ){
				a.Refresh(TimeOut);
			}
						
		}
		
		public void Refresh(Session a){
			a.Refresh(TimeOut);
		}
		
		public ICacheClient CacheClient{get;set;}
		
		#endregion
			
		
			
		
		
	}
}

