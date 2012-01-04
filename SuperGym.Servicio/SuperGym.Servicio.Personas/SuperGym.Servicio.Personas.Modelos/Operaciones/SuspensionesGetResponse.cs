using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class SuspensionesGetResponse:IHasResponseStatus
	{
		public SuspensionesGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Suspensiones = new List<Suspension>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public List<Suspension> Suspensiones  {get ; set;}
		
		public int Count {get { return Suspensiones.Count ;  } }
		
		public bool Success {get;set;}
	}
}

