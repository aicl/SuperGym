using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("ACTIVIDADES")]
	public partial class Actividad{

 		public Actividad(){}

 		[Alias("IDACTIVIDAD")]
		[PrimaryKey]
		[Sequence("GEN_ACTIVIDADES_ID")]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("NOMBRE")]
		[Index(true)]
		[Required]
		public System.String Nombre { get; set;} 

 	}
 }
