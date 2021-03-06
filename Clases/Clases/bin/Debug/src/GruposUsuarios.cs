using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS_USUARIOS")]
	public partial class GruposUsuarios{

 		public GruposUsuarios(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idgrupo { get; set;} 

		[Alias("IDUSUARIO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idusuario { get; set;} 

 	}
 }
