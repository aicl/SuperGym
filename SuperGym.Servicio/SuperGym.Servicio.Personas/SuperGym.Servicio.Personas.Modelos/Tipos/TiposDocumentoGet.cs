using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="TiposDocumento.Consultar")]
	public class TiposDocumentoGet:IRequireAuthentication
	{
		public TiposDocumentoGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

