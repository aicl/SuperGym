using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Manejar")]
	public class DeCajaUpdate:Decaja, IRequireAuthentication
	{
		public DeCajaUpdate ():base()
		{
		}
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

