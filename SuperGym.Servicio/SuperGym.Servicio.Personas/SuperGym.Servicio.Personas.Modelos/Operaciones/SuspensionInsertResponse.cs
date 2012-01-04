using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class SuspensionInsertResponse:FacturaPagoAnularResponse, IHasResponseStatus
	{
		public SuspensionInsertResponse ():base()
		{
			Suspension= new Suspension();
		}
		
		public Suspension Suspension { get; set;}
		
	}
}

