using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class TiposDocumentoGetResponse:IHasResponseStatus
	{
		public TiposDocumentoGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			TiposDocumento = new List<TipoDocumento>();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<TipoDocumento> TiposDocumento{ get; set;}
		
		public int Total {get { return TiposDocumento.Count;} }
	}
}

