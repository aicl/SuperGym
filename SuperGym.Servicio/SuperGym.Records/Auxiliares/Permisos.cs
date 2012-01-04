using System;
using System.Collections.Generic;
namespace SuperGym.Records.Auxiliares
{
	public class Permisos
	{
		public Permisos ()
		{
			Actividades = new List<Actividad>();
			Grupos = new List<UsuarioGrupo>();
		}
		
		public List<Actividad> Actividades{get; set;}
		
		public List<UsuarioGrupo> Grupos{get; set;}
	}
}

