using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Consultar")]
	public class CajasGet:IRequireAuthentication
	{
		public CajasGet ()
		{
			
		}
		
		public Guid SessionId {get; set;}
		
		public string Anio { get; set;}
		
		public string Mes { get; set;}
		
	}
}

