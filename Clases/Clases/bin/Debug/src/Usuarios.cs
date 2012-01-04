using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("USUARIOS")]
	public partial class Usuarios{

 		public Usuarios(){}

 		[Alias("IDUSUARIO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idusuario { get; set;} 

		[Alias("NOMBRE")]
		[Required]
		public System.String Nombre { get; set;} 

		[Alias("CLAVE")]
		[Required]
		public System.String Clave { get; set;} 

		[Alias("IDPERSONA")]
		public System.Int32? Idpersona { get; set;} 

 	}
 }
