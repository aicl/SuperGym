using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class ConceptosCajaGetResponse:IHasResponseStatus
	{
		public ConceptosCajaGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Ingresos = new List<Concepto>();
			Egresos = new List<Concepto>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Concepto> Ingresos{ get; set;}
		public List<Concepto> Egresos { get; set;}
		
	}
}

