
using System;
using System.Collections.Generic;

using ServiceStack.Redis;
using ServiceStack.Common;
using ServiceStack.DesignPatterns.Model;


namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class Session:UserBase,IHasGuidId,IDisposable
	{	
		private IRedisClient redisClient;
		
		public Session(){
			CreatedDate= DateTime.UtcNow;
		}
				
		public DateTime CreatedDate{
			get;  set;
		}
				
		public string IpAddress { get; set; }
		
		public string UserAgent { get; set; }
		
		public void Refresh(double timeout){
			
			ExpiresAt= DateTime.UtcNow.AddMinutes( timeout );
			
			if( redisClient != null) {
				var keys =  redisClient.SearchKeys( SearchKeyString());
				if(keys!=null & keys.Count>0) {
					if(timeout>=0){
						foreach( var s in keys)
							redisClient.ExpireEntryAt(s, ExpiresAt);
					}
					else{
						redisClient.RemoveAll(keys);					
					}
				}
			}
			
		}
		
		public string ToCacheKey()
		{
			return UrnId.Create<Session>(Id.ToString());
		}

		
		public static  string ToCacheKey(Guid sessionId )
		{
			return UrnId.Create<Session>(sessionId.ToString());
		}
		
		public void SetRedisClient(IRedisClient redisClient){
			this.redisClient= redisClient;
			Console.WriteLine("Session SetRedisClient: '{0}'", redisClient);
			Console.WriteLine("Session SetRedisClient: '{0}'", this.redisClient);
		}
		
		public void Add<T>(string key, T value){
			if(redisClient != null){
				redisClient.Set<T>(ToUDCacheKey<T>(key),value,ExpiresAt);
			}
		}
		
		public void Add<T>( T value){
			Console.WriteLine("Session.Add : '{0}'", value);
			Console.WriteLine("Session.Add redisClient: '{0}'", redisClient);
			if(redisClient != null){
				redisClient.Set<T>(ToUDCacheKey<T>(),value,ExpiresAt);
			}
			
		}
		
		public void Remove<T>(string key){
			if(redisClient != null){
				redisClient.Remove( ToUDCacheKey<T>( key) );
			}
		}
		
		public void Remove<T>(){
			if(redisClient != null){
				redisClient.Remove( ToUDCacheKey<T>( typeof(T).Name) );
			}
		}
				
						
		public T GetUserData<T>(string key){
			if(redisClient != null){
				Console.WriteLine("SessionGetObject id : {0}", ToUDCacheKey<T>(key));
				return  redisClient.Get<T>(ToUDCacheKey<T>(key));
			}
			else return default(T);
		}
		
		public T GetUserData<T>(){
			if(redisClient != null){
				Console.WriteLine("SessionGetObject id : {0}", ToUDCacheKey<T>( ));
				return  redisClient.Get<T>(ToUDCacheKey<T>());
			}
			else return default(T);
		}
		
		
		public static string ToUDCacheKey<T>( Guid sessionId, string key ){
			return UrnId.Create<T>(sessionId.ToString(),key);
		}
	
		
		public  string ToUDCacheKey<T>(  string key ){
			return UrnId.Create<T>(Id.ToString(),key);
		}
		
		
		
		public string ToUDCacheKey<T>( ){
			return UrnId.Create<T>(Id.ToString(), typeof(T).Name);
		}
		
		public static string ToUDCacheKey<T>(Guid sessionId, T type){
			return UrnId.Create<T>(sessionId.ToString(), typeof(T).Name);
		}
				
		
		public static string SearchKeyString(Guid sessionId){
			return "urn:*:"+ sessionId.ToString()+"*";	
		}
		
		public string SearchKeyString(){
			return "urn:*:"+ Id.ToString()+"*";	
		}
		
		#region IDisposable implementation
		public void Dispose ()
		{
			if(redisClient != null)redisClient.Dispose();
		}
		#endregion		
	}
	
	
}

// urn:ClassName:sessionId:key:valor
