using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("PROFESION")]
	public partial class Profesion{

 		public Profesion(){}

 		[Alias("IDPROFESION")]
		[PrimaryKey]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("NOMBRE_PROFESION")]
		[Required]
		public System.String Nombre { get; set;} 

 	}
 }
