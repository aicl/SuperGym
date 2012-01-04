using System;
using System.Collections.Generic;
using SuperGym.Records;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class DatosUsuario
	{
		public DatosUsuario ()
		{
			Actividades= new List<Actividad>();
		}
		
		public Int16 Id{get;set;}
		
		public List<Actividad> Actividades{get; set;}
	}
}

