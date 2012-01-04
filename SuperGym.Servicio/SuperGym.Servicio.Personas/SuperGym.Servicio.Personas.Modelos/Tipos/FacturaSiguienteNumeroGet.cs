using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Insertar")]
	public class FacturaSiguienteNumeroGet:IRequireAuthentication
	{
		public FacturaSiguienteNumeroGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

