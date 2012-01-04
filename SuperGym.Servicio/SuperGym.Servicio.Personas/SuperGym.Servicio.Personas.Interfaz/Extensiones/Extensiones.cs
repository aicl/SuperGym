using System;
using System.Linq;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public static class Extensiones{
		
		
		public static string ToHtml(this List<DeCajaClasificacion> decajas, DateTime fecha, decimal saldoAnterior){
		
			StringWriter stringWriter = new StringWriter();

			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				var ingresos = decajas.Where(r=> r.Clasificacion=="Ingresos").Sum(r=> r.Valor);
				var gastos = decajas.Where(r=> r.Clasificacion=="Gastos").Sum(r=> r.Valor)*-1;
				
				var otros = 
        			(from p in decajas
					 orderby p.Factor descending
					 where  p.Clasificacion=="Otros"
        			 group p by p.Concepto into g 
        			 select new { Concepto = g.Key, Valor = g.Sum(p => p.Valor*p.Factor) } ).ToList(); 
				
				var saldo =saldoAnterior+ ingresos+gastos;
				
				writer.RenderBeginTag(HtmlTextWriterTag.H1);
				writer.Write(string.Format( "Informe de Caja del:{0} </br>",
				                           fecha.ToString("dd.MM.yyyy") )  );
				writer.RenderEndTag();
				
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write("Resumen General </br>");
				writer.RenderEndTag();
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:90%;margin: 1em; border-collapse: collapse;"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);				
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "background:white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Concepto");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor");
				writer.RenderEndTag(); //th
				
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
								
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
				
				int j=0;
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Saldo Anterior" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((saldoAnterior>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", saldoAnterior);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Total Ingresos" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((ingresos>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", ingresos);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Total Gastos" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((gastos>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", gastos);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
				
				
				foreach( var x in otros){
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( x.Concepto);
					writer.RenderEndTag(); //td
										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((x.Valor>0)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", x.Valor);
					writer.RenderEndTag(); //td
					
					writer.RenderEndTag(); //tr
					
					saldo+= x.Valor;
				}
	
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Nuevo Saldo" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((saldo>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", saldo);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
	
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
							
				//
				
				var conceptos = 
        			(from p in decajas
					 orderby p.Factor descending, p.Clasificacion ascending
        		group p by p.Concepto into g 
        		select new { Concepto = g.Key, TotalxConcepto = g.Sum(p => p.Valor*((p.Factor>0)?1:-1)) } ).ToList(); 
								
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write("Resumen Por Concepto </br>");
				writer.RenderEndTag();
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                   "width:90%;margin: 1em; border-collapse: collapse;"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "background:white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Concepto");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor");
				writer.RenderEndTag(); //th
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
				
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
				
				j=0;
				foreach( var c in conceptos){
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"font-size:90%;"+
					                    ((j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;") );
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( c.Concepto );
					writer.RenderEndTag(); //td
					
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((c.TotalxConcepto>0)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", Math.Abs(c.TotalxConcepto));
					writer.RenderEndTag(); //td
					
					writer.RenderEndTag(); //tr
					
				}
				
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
						
				//
				
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write("Detalle</br>");
				writer.RenderEndTag();
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                   "width:90%;margin: 1em; border-collapse: collapse;"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                   "background: white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Concepto");
				writer.RenderEndTag(); //th

				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Descripcion");
				writer.RenderEndTag(); //th
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor");
				writer.RenderEndTag(); //th
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Documento");
				writer.RenderEndTag(); //th
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Nombre");
				writer.RenderEndTag(); //th
								
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
				
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
				
				int i=0;
				var dc = decajas.OrderBy(r=>r.Clasificacion).OrderByDescending(r=>r.Factor);
				foreach(var r in dc){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"font-size:90%;"+
					                    ((i++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(r.Concepto);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(r.Descripcion);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;text-align:right;"+
					                    ((r.Factor==1)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", r.Valor);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding:.3em; border:1px #ccc solid; text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(r.Documento);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; border: 1px #ccc solid;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(r.Nombre);
					writer.RenderEndTag(); //td
									
							
					writer.RenderEndTag(); //tr
				}
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
				
				
			}
			return stringWriter.ToString();
		}
		
		
		public static string ToHtml( this List<FacturaDiaDetalle> facturas, DateTime fecha){
						
			StringWriter stringWriter = new StringWriter();

			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write(string.Format("Facturacion del Dia : {0}<br/>", fecha.ToString("dd.MM.yyyy")) );
				writer.RenderEndTag();
				
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:100%"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
								
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "background:white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Documento");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Nombres");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Telefono");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Celular");
				writer.RenderEndTag(); //th
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Numero");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Inicio");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Terminacion");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Cantidad");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Observacion");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Estado");
				writer.RenderEndTag(); //th
				
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
								
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
			
				int j=0;
				
				foreach(FacturaDiaDetalle f in facturas){
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:75%;"+
					                    ((j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;"));
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Documento );
					writer.RenderEndTag(); //td
										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Nombres );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Telefono );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Celular );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;"+
					                    ((f.Activa)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Numero);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.FechaInicio.ToString("dd.MM.yyyy" ) );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.FechaTerminacion.ToString("dd.MM.yyyy" ) );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Cantidad);
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", f.ValorTotal);
					writer.RenderEndTag(); //td

					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Observacion );
					writer.RenderEndTag(); //td
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( f.Activa?"Vigente":"Anulada" );
					writer.RenderEndTag(); //td
					
					writer.RenderEndTag(); //tr	
				}
				
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
			}
			
			return stringWriter.ToString();
		}
		
		public static string AnticiposToHtml( this List<DeCajaClasificacion> decajas){
			
			StringWriter stringWriter = new StringWriter();

			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				var dc =
					(from p in decajas
						where p.Clasificacion=="Otros" && p.Concepto.StartsWith("Anticipo Utilidades")
						select p).ToList();
				
				var grupos = 
					(from r in dc
					 group r by r.Concepto into g 
        			 select new { Concepto = g.Key, Valor = g.Sum(p => p.Valor) } ).ToList(); 
										
				
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write("Detalle de los Anticipos de Utilidades<br/>" );
				writer.RenderEndTag();
												
				int j=0;				
				foreach( var gr in grupos){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.AddAttribute(HtmlTextWriterAttribute.Name,gr.Concepto);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write( "<b>"
					             +gr.Concepto+
					             string.Format(": {0:##,000.00}", gr.Valor)
					             +"</b>" );
					writer.RenderEndTag(); // A					
										
					List<DeCajaClasificacion> detalle= dc.Where(r=>r.Concepto== gr.Concepto).ToList();
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:100%"); 
					writer.RenderBeginTag(HtmlTextWriterTag.Table);
									
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "background:white"); 
					writer.RenderBeginTag(HtmlTextWriterTag.Thead);
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Descripcion");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Documento");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Beneficiario");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Valor ($)");
					writer.RenderEndTag(); //th
					writer.RenderEndTag(); //tr
					writer.RenderEndTag(); //thead
									
					writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
										
					foreach( var c in detalle){
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:80%;"+
						                    ((j++%2==0)? 
						                    "background-color:#eee;":
						                    "background-color:#ccc;"));
						writer.RenderBeginTag(HtmlTextWriterTag.Tr);
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Descripcion );
						writer.RenderEndTag(); //td
											
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Documento );
						writer.RenderEndTag(); //td
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Nombre );
						writer.RenderEndTag(); //td
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write("{0:##,000.00}", c.Valor);
						writer.RenderEndTag(); //td

						writer.RenderEndTag(); //tr	
					}
					
					writer.RenderEndTag(); //tbody
					writer.RenderEndTag(); //table				
					
				}				
				
			}
			return stringWriter.ToString();
		}
		
		public static string PagosToHtml( this List<DeCajaClasificacion> decajas){
			
			StringWriter stringWriter = new StringWriter();

			// Put HtmlTextWriter in using block because it needs to call Dispose.
			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{
				var dc =
					(from p in decajas
						where p.Clasificacion=="Gastos"
						select p).ToList();
				
				var grupos = 
					(from r in dc
					 group r by r.Concepto into g 
        			 select new { Concepto = g.Key, Valor = g.Sum(p => p.Valor) } ).ToList(); 
										
				
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				writer.Write("Detalle de los Pagos<br/>" );
				writer.RenderEndTag();
												
				int j=0;				
				foreach( var gr in grupos){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
					writer.AddAttribute(HtmlTextWriterAttribute.Name,gr.Concepto);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write( "<b>"
					             +gr.Concepto+
					             string.Format(": {0:##,000.00}", gr.Valor)
					             +"</b>" );
					writer.RenderEndTag(); // A					
										
					List<DeCajaClasificacion> detalle= dc.Where(r=>r.Concepto== gr.Concepto).ToList();
					
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:100%"); 
					writer.RenderBeginTag(HtmlTextWriterTag.Table);
									
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "background:white"); 
					writer.RenderBeginTag(HtmlTextWriterTag.Thead);
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Descripcion");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Documento");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding:.3em;text-align:center;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Beneficiario");
					writer.RenderEndTag(); //th
					writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
					writer.RenderBeginTag(HtmlTextWriterTag.Th);
					writer.Write("Valor ($)");
					writer.RenderEndTag(); //th
					writer.RenderEndTag(); //tr
					writer.RenderEndTag(); //thead
									
					writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
										
					foreach( var c in detalle){
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:80%;"+
						                    ((j++%2==0)? 
						                    "background-color:#eee;":
						                    "background-color:#ccc;"));
						writer.RenderBeginTag(HtmlTextWriterTag.Tr);
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Descripcion );
						writer.RenderEndTag(); //td
											
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Documento );
						writer.RenderEndTag(); //td
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write( c.Nombre );
						writer.RenderEndTag(); //td
						
						writer.AddAttribute(HtmlTextWriterAttribute.Style,"padding:.3em;text-align:right;");
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						writer.Write("{0:##,000.00}", c.Valor);
						writer.RenderEndTag(); //td

						writer.RenderEndTag(); //tr	
					}
					
					writer.RenderEndTag(); //tbody
					writer.RenderEndTag(); //table				
					
				}				
				
			}
			return stringWriter.ToString();
		}
		
		
		
		public static string ToHtml(this List<ConceptoValor> consolidado,
		                            DateTime desde, DateTime hasta){
			
			StringWriter stringWriter = new StringWriter();

			using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
			{			
								
				var saldoAnterior= consolidado.Where(r=> r.Factor==0).Sum(r=> r.Valor);
				var ingresos = consolidado.Where(r=> r.Clasificacion=="Ingresos").Sum(r=> r.Valor);
				var egresos = consolidado.Where(r=> r.Clasificacion=="Gastos").Sum(r=> r.Valor);
				var saldo= saldoAnterior + ingresos+egresos;
				var estado = ingresos+egresos;
				
				writer.RenderBeginTag(HtmlTextWriterTag.H1);
				writer.Write(string.Format("Estado de Resultados<br/>"+
				                           "Del: {0} al: {1}",
				                           desde.ToString("dd.MM.yyyy"),
				                           hasta.ToString("dd.MM.yyyy"))  );
				writer.RenderEndTag(); //h1
				
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:100%"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
								
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                   "background: white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:center; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Concepto");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;color:white");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; text-align:right; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor ($)");
				writer.RenderEndTag(); //th
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
								
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
				
				int j=0;
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);  //start tr
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Ingresos" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; text-align:right;"+
					                    ((saldoAnterior>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", ingresos);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr	

				List<ConceptoValor> tipo= consolidado.Where(p=>p.Clasificacion=="Ingresos").ToList();						
				foreach( var x in tipo){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
					writer.RenderBeginTag(HtmlTextWriterTag.Tr); //start tr
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; ");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write( x.Concepto);
					writer.RenderEndTag(); //td										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:right;"+
					                    ((x.Valor>0)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", x.Valor);
					writer.RenderEndTag(); //td					
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: .3em; ");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag(); //td					
					writer.RenderEndTag(); //tr
				}
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr); //start tr
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Gastos" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:right;"+
					                    ((egresos>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", egresos);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
								
				tipo= consolidado.Where(p=>p.Clasificacion=="Gastos").ToList();									
				foreach( var x in tipo){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write(string.Format("<a href='#{0}'>", x.Concepto )+ x.Concepto + "</a>");
					writer.RenderEndTag(); //td										
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:right;"+
					                    ((x.Valor>0)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", x.Valor);
					writer.RenderEndTag(); //td
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: .3em; ");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag(); //td					
					writer.RenderEndTag(); //tr
				}
									
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);	
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "<b>Ingresos - Gastos</b>" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:right;"+
					                    ((estado>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("<b>{0:##,000.00}</b>", estado);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
				
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
								
				//
				
				writer.RenderBeginTag(HtmlTextWriterTag.H1);
				writer.Write(string.Format( "<br/>Flujo de Caja<br/>"+
				                           "Del: {0} al: {1}",
				                           desde.ToString("dd.MM.yyyy"),
				                           hasta.ToString("dd.MM.yyyy"))  );
				writer.RenderEndTag();
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,"width:100%"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Table);
				
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                   "background: white"); 
				writer.RenderBeginTag(HtmlTextWriterTag.Thead);
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;text-align:center; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Concepto");
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; color:white");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.RenderEndTag(); //th
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; text-align:right; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Th);
				writer.Write("Valor ($)");
				writer.RenderEndTag(); //th
				writer.RenderEndTag(); //tr
				writer.RenderEndTag(); //thead
		
				writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "Saldo Anterior" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; text-align:right;"+
					                    ((saldoAnterior>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("{0:##,000.00}", saldoAnterior);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr
				
				tipo= consolidado.Where(p=>p.Clasificacion=="Otros").OrderByDescending(p=>p.Factor).ToList();									
				foreach( var x in tipo){
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);	
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em;");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					if( ! x.Concepto.StartsWith("Anticipo Utilidades") )
						writer.Write( x.Concepto );
					else 
						writer.Write(string.Format("<a href='#{0}'>", x.Concepto )+ x.Concepto + "</a>");
					writer.RenderEndTag(); //td
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; ");
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.RenderEndTag(); //td
					writer.AddAttribute(HtmlTextWriterAttribute.Style,
						                    "padding: .3em;text-align:right;"+
						                    ((x.Valor>0)?"color:black;":"color:red;" ));
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					writer.Write("{0:##,000.00}", x.Valor);
					writer.RenderEndTag(); //td
					writer.RenderEndTag(); //tr
					saldo+=x.Valor;
				}
					
				
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    (j++%2==0)? 
					                    "background-color:#eee;":
					                    "background-color:#ccc;");
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);	
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em;");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write( "<b>Nuevo Saldo</b>" );
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
				                    "padding: .3em; ");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag(); //td
				writer.AddAttribute(HtmlTextWriterAttribute.Style,
					                    "padding: .3em; text-align:right;"+
					                    ((saldo>0)?"color:black;":"color:red;" ));
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write("<b>{0:##,000.00}</b>", saldo);
				writer.RenderEndTag(); //td
				writer.RenderEndTag(); //tr			
								
				writer.RenderEndTag(); //tbody
				writer.RenderEndTag(); //table
										
			}
			
			return stringWriter.ToString();
		}
		
	}
}

/*
				writer.RenderBeginTag(HtmlTextWriterTag.Head);
				writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
				writer.RenderBeginTag(HtmlTextWriterTag.Style);
								
				writer.Write(@"
					body{
						background-color: #fff;
        				color: #000;
        				font-family: arial, sans-serif;
        				font-size: 90%;
    				}
					td {border:1px solid #fff;}            
					tbody tr {background-color:#ccc;}
					tbody tr.alt {background-color:#eee;}

					table { margin: 1em; border-collapse: collapse; }
					td, th { padding: .3em; border: 1px #ccc solid; }
					thead { background: white }
				");
				writer.RenderEndTag();		
				*/