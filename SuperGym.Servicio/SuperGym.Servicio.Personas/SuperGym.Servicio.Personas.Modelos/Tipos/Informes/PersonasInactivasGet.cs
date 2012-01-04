using System;

using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class PersonasInactivasGet:IRequireAuthentication
	{
		public PersonasInactivasGet ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public Int32 DiasAusenciaTopeInferior { get; set;}
		
		
		public Int32 DiasAusenciaTopeSuperior { get; set;}
	}
}

