using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class MunicipiosGetResponse:IHasResponseStatus
	{
		public MunicipiosGetResponse ()
		{
			ResponseStatus = new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Municipios = new List<Municipio>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Municipio> Municipios{ get; set;}
		
		public int Total {get { return  Municipios.Count;} }
		
	}
}

