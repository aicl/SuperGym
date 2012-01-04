
using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Actualizar")]
	public class SuspensionInsert: IRequireAuthentication
	{
		public SuspensionInsert ()
		{
		}
		
		public Int32 IdPago { get; set; }
		public string Numero { get; set;}
		public DateTime Desde {get; set;}
		public DateTime Hasta {get; set;}
		
		public Guid SessionId {get; set;}
		
	}
}

