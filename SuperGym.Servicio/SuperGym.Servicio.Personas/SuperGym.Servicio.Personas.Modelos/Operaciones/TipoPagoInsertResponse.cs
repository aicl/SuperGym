using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class TipoPagoInsertResponse:IHasResponseStatus
	{
		public TipoPagoInsertResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			TipoPago = new TipoPago();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public TipoPago TipoPago {get ; set;}
		
		public bool Success {get;set;}
	}
}

