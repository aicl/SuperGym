using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class ConceptosGetResponse:IHasResponseStatus
	{
		public ConceptosGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Conceptos = new List<Concepto>();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Concepto> Conceptos{ get; set;}
		
		public int Total {get { return  Conceptos.Count;} }
	}
}

