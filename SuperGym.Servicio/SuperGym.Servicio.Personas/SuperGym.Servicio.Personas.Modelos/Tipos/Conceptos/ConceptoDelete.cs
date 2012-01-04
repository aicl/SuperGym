using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Conceptos.Manejar")]
	public class ConceptoDelete:IRequireAuthentication
	{
		public ConceptoDelete ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int16 Id{ get; set;}
	}
}

