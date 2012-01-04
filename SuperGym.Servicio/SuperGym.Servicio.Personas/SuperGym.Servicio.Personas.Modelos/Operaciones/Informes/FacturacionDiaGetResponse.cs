using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records.Auxiliares;


namespace SuperGym.Servicio.Personas.Modelos
{
	public class FacturacionDiaGetResponse:IHasResponseStatus
	{
		public FacturacionDiaGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			FacturacionDia= new List<FacturacionDia>();
						
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
		public List<FacturacionDia> FacturacionDia { get; set;}
		
	}
}

