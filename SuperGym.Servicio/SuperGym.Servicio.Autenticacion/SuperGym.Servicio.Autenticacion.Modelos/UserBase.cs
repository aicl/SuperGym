using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class UserBase
	{
		public UserBase ()
		{
		}
		
		public Guid Id { get; set; }

		public string ClientType { get; set; }

		public DateTime ExpiresAt { get; set; }
		
		public string Name{get;set;}
		
		public string UserId{get; set;}
						
		
	}
}

