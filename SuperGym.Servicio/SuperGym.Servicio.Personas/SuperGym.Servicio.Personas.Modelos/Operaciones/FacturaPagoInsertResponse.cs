using System;

using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class FacturaPagoInsertResponse:IHasResponseStatus
	{
		public FacturaPagoInsertResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Factura= new Pago();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public Pago Factura {get ; set;}
		
		public bool Success {get;set;}
		
		public bool PrintSuccess{ get; set;}
		
		public string PrintMessage { get; set;}
	}
}

