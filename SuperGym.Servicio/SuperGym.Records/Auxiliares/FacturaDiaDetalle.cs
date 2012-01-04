using System;
namespace SuperGym.Records
{
	public class FacturaDiaDetalle
	{
		public FacturaDiaDetalle ()
		{
		}
		
		public string Documento {get; set;}
		
		public string Nombres {get; set;}
		
		public string Telefono {get; set;}
		
		public string Celular {get; set;}
		
		public string Numero {get; set;}
		
		public DateTime FechaInicio {get; set;}
		
		public DateTime FechaTerminacion {get; set;}
		
		public Decimal ValorUnitario {get; set;}
		
		public System.Int16 Cantidad { get; set;} 
			
		public Decimal ValorTotal {get; set;}
		
		public string Observacion {get; set;}
		
		public bool Activa {get; set;}
		
	}
}

/*
 
Documento\", \n");
		var1.Append("       p.nombres \n");
		var1.Append("        || ' ' \n");
		var1.Append("        || p.primer_apellido \n");
		var1.Append("        || TRIM(' ' \n");
		var1.Append("                 || Coalesce(p.segundo_apellido, '')) AS \"Nombres\", \n");
		var1.Append("       p.telefono                                     AS \"Telefono\", \n");
		var1.Append("       p.celular                                      AS \"Celular\", \n");
		var1.Append("       a.numero_factura                               AS \"Numero\", \n");
		var1.Append("       a.fechainicio                                  AS \"FechaInicio\", \n");
		var1.Append("       a.fechaterminacion                             AS \"FechaTerminacion\", \n");
		var1.Append("       a.valor_unitario                               AS \"ValorUnitario\", \n");
		var1.Append("       a.cantidad                                     AS \"Cantidad\", \n");
		var1.Append("       a.valor_total                                  AS \"ValorTotal\", \n");
		var1.Append("       a.observacion                                  AS \"Observacion\", \n");
		var1.Append("       a.activa                                       AS \"Activa\" \n");
*/

