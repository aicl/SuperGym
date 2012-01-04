using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonaValidarResponse:IHasResponseStatus
	{
		public PersonaValidarResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Persona = new Persona();
			Factura = new Pago();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public Persona Persona {get ; set;}
		
		public bool Success {get;set;}
		
		public Pago Factura { get; set;}
		
		public string UltimoIngreso {get; set;}
		
	}
}

