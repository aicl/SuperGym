using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class CustomAuthAttribute:Attribute
	{
		public CustomAuthAttribute()
		{
		}
		
		public CustomAuthAttribute(string clientType, string action, string objectName)
		{
			ClientType = clientType;
			Action=action;
			ObjectName= objectName;
			
		}
		
		public string ClientType { get; set; }
		
		public string Action { get; set;}
		
		public string ObjectName{ get; set;}
		
	}
	
	
	
}


