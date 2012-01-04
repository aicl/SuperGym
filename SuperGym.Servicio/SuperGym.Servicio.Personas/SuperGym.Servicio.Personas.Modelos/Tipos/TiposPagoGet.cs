using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="TiposPago.Consultar")]
	public class TiposPagoGet:IRequireAuthentication
	{
		public TiposPagoGet ()
		{
		}
		public Guid SessionId {get; set;}
	}
}

