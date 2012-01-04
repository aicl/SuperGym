using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("CORREOS")]
	public partial class Correo{

 		public Correo(){}

 		[Alias("ID")]
		[Sequence("GEN_CORREOS_ID")]
		[PrimaryKey]
		[AutoIncrement]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("IDUSUARIO")]
		[Required]
		public System.Int16 IdUsuario { get; set;} 

		[Alias("IDACTIVIDAD")]
		[Required]
		public System.Int16 IdActividad { get; set;} 

 	}
 }