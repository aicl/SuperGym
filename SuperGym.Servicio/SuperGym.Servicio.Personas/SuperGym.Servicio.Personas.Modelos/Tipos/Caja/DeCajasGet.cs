using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Consultar")]
	public class DeCajasGet:IRequireAuthentication
	{
		public DeCajasGet ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int32 IdCaja { get; set;}
	}
}

