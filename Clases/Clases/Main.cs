using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;

using ServiceStack.DataAnnotations;
using System.Reflection;

using ServiceStack.OrmSimple;
using ServiceStack.OrmSimple.DbSchema;
using ServiceStack.OrmSimple.Firebird;

using System.Linq;


namespace Clases
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			
			Config.DialectProvider = new FirebirdDialectProvider(); 
			using (IDbConnection db =
			       ("User=SYSDBA;Password=masterkey;Database=GYM.FDB;" +
			       	"DataSource=localhost;Dialect=3;charset=ISO8859_1;").OpenDbConnection())
			using ( IDbCommand dbConn = db.CreateCommand())
			{
				
				Schema fbd= new Schema(){
					Connection = db
				};
				
				ClassWriter cw = new ClassWriter(){
					Schema=fbd,
					SpaceName="SuperGym.Records",
					Usings= "using System;\n"+
					"using System.ComponentModel.DataAnnotations;\n"+
					"using ServiceStack.Common;\n"+
					"using ServiceStack.DataAnnotations;\n"+
					"using ServiceStack.OrmSimple;\n"
					
				};
								
				//var tables = fbd.Tables;
				//foreach(var t in tables){
				//	cw.WriteClass(t);	
				//}
					
				
				Table t = new Table(){Name=	"SALDOPORCOBRAR"};
				cw.WriteClass(t);
			
			}
			
			Console.WriteLine ("This is The End my friend!");
		}
	}
}

