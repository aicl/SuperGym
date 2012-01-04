using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS_ACTIVIDADES")]
	public partial class GrupoActividad{

 		public GrupoActividad(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdGrupo { get; set;} 

		[Alias("IDACTIVIDAD")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdActividad { get; set;} 

 	}
 }
