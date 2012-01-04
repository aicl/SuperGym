using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records.Auxiliares  
{
 	[Alias("PAGOS")]
	public partial class UltimaFactura{

 		public UltimaFactura(){}

		[Alias("NUMERO_FACTURA")]
		public System.String Numero { get; set;} 

		
 	}
 }
