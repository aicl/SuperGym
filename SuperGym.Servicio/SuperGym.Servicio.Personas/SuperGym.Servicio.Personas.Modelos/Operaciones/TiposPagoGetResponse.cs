using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class TiposPagoGetResponse:IHasResponseStatus
	{
		public TiposPagoGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			TiposPago = new List<TipoPago>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<TipoPago> TiposPago{ get; set;}
		
		public int Total {get { return  TiposPago.Count;} }
	}
}

