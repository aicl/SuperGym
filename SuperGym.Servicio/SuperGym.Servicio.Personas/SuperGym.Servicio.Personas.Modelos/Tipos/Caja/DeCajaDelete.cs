using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Manejar")]
	public class DeCajaDelete:IRequireAuthentication
	{
		public DeCajaDelete ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int32 Id{ get; set;}
	}
}

