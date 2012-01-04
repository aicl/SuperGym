using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS_USUARIOS")]
	public partial class GrupoUsuario{

 		public GrupoUsuario(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdGrupo { get; set;} 

		[Alias("IDUSUARIO")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdUsuario { get; set;} 

 	}
 }
