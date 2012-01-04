using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace SuperGym.Records.Auxiliares
{
	public class UsuarioGruposActividades
	{
		public UsuarioGruposActividades ()
		{
		}
		
		public System.Int16 IdGrupo { get; set;} 

		public System.String NombreGrupo { get; set;} 
		
		public System.String Directorio { get; set;} 
		
		public Boolean Menu { get; set;}
				
		public System.Int16 IdActividad { get; set;} 
		
		public System.String NombreActividad { get; set;} 

		
		
	}
}

