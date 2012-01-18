using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class FacturaPagoImprimirResponse:IHasResponseStatus
	{
		public FacturaPagoImprimirResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
		
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
	}
}