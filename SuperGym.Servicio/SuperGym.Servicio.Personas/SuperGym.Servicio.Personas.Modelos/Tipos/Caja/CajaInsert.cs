using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Manejar")]
	public class CajaInsert:  IRequireAuthentication
	{
		public CajaInsert ()
		{
		}
				
		public Guid SessionId {get; set;}
		
		public DateTime Fecha { get; set;}
		
	}
}

