using System;
using SuperGym.Records;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Conceptos.Manejar")]
	public class ConceptoUpdate:Concepto, IRequireAuthentication
	{
		public ConceptoUpdate ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}