using System;


namespace SuperGym.Records.Auxiliares
{
	public class FacturacionDia
	{
		public FacturacionDia ()
		{
		}
		
		public DateTime FechaPago{get; set;}
		public string Nombre { get; set;}
		public Int16 Cantidad { get; set; }
		public Decimal Valor { get; set;} 
		
	}
}

