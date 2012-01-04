using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class CajasGetResponse:IHasResponseStatus
	{
		public CajasGetResponse ()
		{
			
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Cajas = new List<Caja>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Caja> Cajas{ get; set;}
		
		public int Total {get { return  Cajas.Count;} }
	}
}

