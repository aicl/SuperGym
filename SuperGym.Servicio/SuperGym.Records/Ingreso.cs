using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("FECHASINGRESO")]
	public partial class Ingreso{

 		public Ingreso(){}

 		[Alias("IDFECHAINGRESO")]
		[PrimaryKey]
		[Required]
		[Sequence("GEN_FECHASINGRESO_ID")]
		public System.Int16 Id { get; set;} 

		[Alias("IDPERSONA")]
		public System.Int32 IdPersona { get; set;} 

		[Alias("FECHA")]
		public System.DateTime Fecha { get; set;} 

 	}
 }
