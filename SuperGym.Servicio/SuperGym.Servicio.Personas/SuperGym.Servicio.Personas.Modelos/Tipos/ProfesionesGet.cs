using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Profesiones.Consultar")]
	public class ProfesionesGet:IRequireAuthentication
	{
		public ProfesionesGet ()
		{
			
		}
		
		public Guid SessionId {get; set;}
	}
}

