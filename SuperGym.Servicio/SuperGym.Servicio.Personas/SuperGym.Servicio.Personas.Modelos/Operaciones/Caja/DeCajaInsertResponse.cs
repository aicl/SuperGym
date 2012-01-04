using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class DeCajaInsertResponse:IHasResponseStatus
	{
		public DeCajaInsertResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			DeCaja = new Decaja();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public Decaja DeCaja{ get; set;}
	}
}

