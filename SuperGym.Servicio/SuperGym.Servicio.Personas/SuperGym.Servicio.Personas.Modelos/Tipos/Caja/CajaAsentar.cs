using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Asentar")]
	public class CajaAsentar:IRequireAuthentication
	{
		public CajaAsentar ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int32 Id {get; set;}
		
		public DateTime TrasladarA {get;set;}
	}
	
	[CustomAuth(Action="Caja.Desasentar")]
	public class CajaDesasentar:IRequireAuthentication
	{
		public CajaDesasentar ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int32 Id {get; set;}
	}
	
}

