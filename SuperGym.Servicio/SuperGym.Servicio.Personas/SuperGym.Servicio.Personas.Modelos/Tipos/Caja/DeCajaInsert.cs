using System;
using SuperGym.Records;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Manejar")]
	public class DeCajaInsert:Decaja, IRequireAuthentication
	{
		public DeCajaInsert ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

