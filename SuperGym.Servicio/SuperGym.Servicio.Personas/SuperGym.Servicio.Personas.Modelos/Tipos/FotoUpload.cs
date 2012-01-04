using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Personas.Actualizar")]
	public class FotoUpload:IRequireAuthentication
	{
		public FotoUpload ()
		{
		}
		
		public Int32 Id {get; set;}
		public Guid SessionId {get; set;}
	}
}

