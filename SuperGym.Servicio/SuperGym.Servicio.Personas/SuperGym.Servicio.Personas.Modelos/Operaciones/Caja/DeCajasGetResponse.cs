using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class DeCajasGetResponse:IHasResponseStatus
	{
		public DeCajasGetResponse ()
		{
			
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			DeCajas = new List<Decaja>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Decaja> DeCajas{ get; set;}
		
		public int Total {get { return  DeCajas.Count;} }
	}
}

