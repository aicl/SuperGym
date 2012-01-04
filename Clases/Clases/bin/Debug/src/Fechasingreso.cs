using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("FECHASINGRESO")]
	public partial class Fechasingreso{

 		public Fechasingreso(){}

 		[Alias("IDFECHAINGRESO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idfechaingreso { get; set;} 

		[Alias("IDPERSONA")]
		public System.Int32? Idpersona { get; set;} 

		[Alias("FECHA")]
		public System.DateTime? Fecha { get; set;} 

 	}
 }
