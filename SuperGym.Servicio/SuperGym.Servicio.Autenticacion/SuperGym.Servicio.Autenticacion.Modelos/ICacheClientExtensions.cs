using System;
using System.Linq;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Common;

namespace ServiceStack.CacheAccess
{
	public static class ICacheClientExtensions
	{
		
		public static  T GetFromCache<T>(this ICacheClient cacheClient, string cacheKey,
			Func<T> factoryFn){
			var res = cacheClient.Get<T>(cacheKey);
			if (res != null) return res;
			else {
				res= factoryFn();				
				if (res != null ) cacheClient.Set<T>(cacheKey, res);
				return res;
			}
		}
		
		public static IDictionary<string,T> GetFromCache<T>(this ICacheClient cacheClient,  string pattern){
			return GetFromCache<T>(cacheClient, new List<string>(), pattern, (object s)=>{ return default(T); });
		}
		
		public static IDictionary<string,T> GetFromCache<T>(this ICacheClient cacheClient,
		                                                    IEnumerable<string> cacheKeys,
		                                                    string pattern,
		                                                    Func<object,T> factoryFn ){
						
			using (IRedisClient redisClient = (cacheClient as  IRedisClientsManager).GetClient() ){
							
				List<string> res = redisClient.SearchKeys(pattern);
	
				if( cacheKeys!=null  && cacheKeys.ToList().Count>0 ) {
					var s= ( from r in res
						from c in cacheKeys
						where r==c
						select r).ToList();
	
					var cache = ( s != null && s.Count>0  )? cacheClient.GetAll<T>(s):  new Dictionary<string,T>();
	
					foreach(string key in cacheKeys){
						if( !s.Contains(key)){
							T t = factoryFn(UrnId.GetStringId(key));
							if( t != null ){
								cacheClient.Set<T>( key, t);
								cache.Add(key,t);
							}
						}
					}
					return cache;
				}
				else{
					return (res !=null && res.Count>0)? cacheClient.GetAll<T>(res): new Dictionary<string,T>();;
				}
				
			}
		
		}
		
		
	}
}

