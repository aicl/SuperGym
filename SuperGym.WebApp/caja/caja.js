Ext.require([
    'Ext.form.*', 
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
	'Ext.tip.*'
]);

Ext.onReady(function(){

	var disableDesasentar= ! checkActividad('Caja.Asentar');
	
    var url= location.protocol + "//" + location.host+'/servicio';
    
    Ext.QuickTips.init();
    
    Ext.define('TipoPago',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'number', defaultValue:1},
    		{name: 'Nombre', type:'string'},
    		{name: 'Valor', type:'number'},
    		{name: 'Dias' ,  type:'number'},
    		{name: 'ValidoDesde', type:'date', convert:function(v){ return  convertToDate(v) } },
    		{name: 'ValidoHasta', type:'date', convert: function(v){ return  convertToDate(v) }}
    	]
    });
    
    Ext.define('Caja',{
    	extend:'Ext.data.Model',
    	idProperty :'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Fecha', type:'date',  convert:function(v){ return  convertToDate(v) } },
    		{name: 'Ingresos', type:'number'},
    		{name: 'Salidas', type:'number'},
    		{name: 'FechaAsentado', type:'date',  convert:function(v){ return  convertToDate(v) } },
    		{name: 'AsentadoPor', type:'int'},
    		{name: 'SaldoAnterior', type:'number'},
    		{name: 'TrasladarA', type:'date',  convert:function(v){ return  convertToDate(v) } }
    	]
    });
    
    Ext.define('DeCaja',{
    	extend:'Ext.data.Model',
    	idProperty :'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'IdCaja', type:'int'},
    		{name: 'Concepto', type:'string'},
    		{name: 'Factor', type:'int'},
    		{name: 'Descripcion', type:'string'},
    		{name: 'Valor', type:'number'},
    		{name: 'Documento', type:'string'},
    		{name: 'Nombre', type:'string'}
    	]
    });
    
    Ext.define('Concepto',{
    	extend:'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Descripcion', type:'string'},
    		{name: 'Factor', type:'int'},
    		{name: 'Clasificacion', type:'string'}
    	]
    });
    
    var storeEgresos= Ext.create('Ext.data.Store', {
		model:'Concepto',
		proxy: createAjaxProxy( {
			url : url+'/json/asynconeway/ConceptosCajaGet',
           	root: 'Egresos',
    	}),
	    autoLoad: true
	});
    
	var storeIngresos= Ext.create('Ext.data.Store', {
		model:'Concepto',
		proxy: createAjaxProxy( {
			url : url+'/json/asynconeway/ConceptosCajaGet',
           	root: 'Ingresos',
    	}),
	    autoLoad: true
	});
	
   	var storeTiposPago = Ext.create('Ext.data.Store', {
	    model: 'TipoPago',
		proxy: createAjaxProxy( {
			url : url+'/json/asynconeway/FacturaTipoPagoGet',
           	root: 'TiposPago',
    	}),
	    autoLoad: true
	});
	
    var storeCajas= Ext.create('Ext.data.Store', {
		model:'Caja',
		autoLoad:false
	});
    
	var storeDeCajas= Ext.create('Ext.data.Store', {
		model:'DeCaja',
		autoLoad:false,
		listeners: {
			datachanged: function(store){
				Ext.getCmp('buttonBorrarCaja').setDisabled( ( store.count()>0  ) );
				sumarValores(store);
			},
			update: function(store, record, operation){
				if(operation==Ext.data.Model.COMMIT){
					sumarValores(store);
				}
			}
        }
	});
	    
	var comboEgresos = Ext.create('Ext.form.field.ComboBox', {
		emptyText : 'Seleccione el concepto...',
		allowBlank:false,
        fieldLabel: 'Concepto',
        displayField: 'Descripcion',
		valueField: 'Descripcion',
		name:'Concepto',
        store: storeEgresos,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true,
    });

    
    var comboIngresos = Ext.create('Ext.form.field.ComboBox', {
		emptyText : 'Seleccione el concepto...',
		allowBlank:false,
        fieldLabel: 'Concepto',
        displayField: 'Descripcion',
		valueField: 'Descripcion',
		name:'Concepto',
        store: storeIngresos,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true,
    });
	
	var gridCajas = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeCajas,
        columns: [{
            text     : 'Fecha',
            flex     : 1,
            sortable : true,
            dataIndex: 'Fecha',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Saldo Anterior',
            width    : 120,
            dataIndex: 'SaldoAnterior',
            renderer: Ext.util.Format.usMoney
        },{
            text     : 'Ingresos',
            width    : 120,
            sortable : true,
            dataIndex: 'Ingresos',
            renderer: Ext.util.Format.usMoney
        },{
            text     : 'Salidas',
            width    : 120,
           	sortable : true,
           	dataIndex: 'Salidas',
           	renderer: Ext.util.Format.usMoney
        },{
            text     : 'Nuevo Saldo',
            width    : 120,
            dataIndex: 'SaldoAnterior',
            renderer: function(value, metadata, record, store){
           		
           		value =   value  + record.get('Ingresos') - record.get('Salidas' );
           		if (value>=0)  {
            		return '<div class="x-cell-positive">' + formatCurrency(value )+ '</div>';
        		} else  {
            		return '<div class="x-cell-negative">' + formatCurrency(value )+ '</div>';
        		}
           	}
        },{
            text     : 'Fecha Asentado',
            width    : 120,
            sortable : true,
            dataIndex: 'FechaAsentado',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Trasladar Saldo A',
            width    : 120,
            dataIndex: 'TrasladarA',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        }],
        height: 300,
        width: 900,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarCaja',
                text:'Documento',
                tooltip:'Crear Nuevo Documento para registrar Ingresos, Consignaciones, Pagos',
                iconCls:'add',
                handler:function(){
                	windowCaja.show();
                }
            },'-',{
            	id: 'buttonAsentarCaja',
                text:'Asentar',
                tooltip:'Asienta el Documento Seleccionado',
                iconCls:'asentar',
                disabled:true,
                handler: function() {
                	windowTrasladarA.show();
                }
            },'-',{
            	id: 'buttonDesasentarCaja',
                text:'Desasentar',
                tooltip:'Desasienta el Documento Seleccionado',
                iconCls:'desasentar',
                disabled:true,
                handler:function(){
                	asentarCaja("Desasentar")
                }
            },'-',{
            	id: 'buttonBorrarCaja',
                text:'Borrar Documento',
                tooltip:'Borrar Documento seleccionado',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridCajas.getSelectionModel().getSelection()[0].getId() 
	    			};
	    			executeAjaxRequest({
						url: url+'/json/asynconeway/CajaDelete',
						success: function(result) {   
							storeCajas.remove(gridCajas.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            },'-', {
            	id: 'anio', xtype: 'numberfield', name: 'Anio', width: 60, allowBlank: false, value: (new Date()).getFullYear()
            },{
            	id: 'mes', xtype: 'numberfield',  name: 'Mes',  width: 60, allowBlank: false, value: (new Date()).getMonth() +1 
            },{
            	xtype:'button', text:'Consultar', 
            	handler:function(){	
	    			var record = {
	    				SessionId:sessionStorage.id,
	    				Anio: Ext.getCmp('anio').getValue(),
	    				Mes:Ext.getCmp('mes').getValue()
	    			};
	    	           	
	    			if(! record.Anio){
	    				Ext.Msg.alert(' Debe indicar el valor para el periodo');
            			return;
	    			};
	    			
	    			if(! record.Mes){
	    				Ext.Msg.alert(' Debe indicar el valor para el mes del periodo');
            			return;
	    			}; 			
	    			executeAjaxRequest({
						url: url+'/json/asynconeway/CajasGet',
						success: function(result) {   
							storeCajas.setProxy(new Ext.data.proxy.Memory({
								model:'Caja',
								data: result.Cajas
							}) );
							storeCajas.load();	
							
						},
						params:record
					});
	    		}              
			}]
        }]
    });

 	gridCajas.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	if(selectedRecord[0].get('FechaAsentado')){
        		habilitarInsertarDeCaja(false);
        		habilitarOperacionesCaja(false);      		
        	}
        	else{
        		habilitarInsertarDeCaja(true);
        		habilitarOperacionesCaja(true);      	
        	}
        	
        	if(selectedRecord[0].get('Ingresos')>0 || selectedRecord[0].get('Salidas')>0 ){

        		var record = {
	    			SessionId:sessionStorage.id,
	    			IdCaja: selectedRecord[0].getId()
	    		}
    	            			  			
    			executeAjaxRequest({
					url: url+'/json/asynconeway/DeCajasGet',
					success: function(result) {   
						storeDeCajas.setProxy(new Ext.data.proxy.Memory({
							model:'DeCaja',
							data: result.DeCajas
						}) );
						storeDeCajas.load();
						gridDeCajas.determineScrollbars();
					},
					params:record,
					showReady:false
				});
        		
        	}
        	else{
        		storeDeCajas.removeAll();
        	}
        }
        else{
        	habilitarInsertarDeCaja(false);
        	habilitarOperacionesCaja(false);
        	Ext.getCmp('buttonDesasentarCaja').setDisabled(true);
        	storeDeCajas.removeAll();
        }
	});
	
	var gridDeCajas = Ext.create('Ext.grid.Panel', {
		frame:true,
        title:'Detalles',
		selType: 'rowmodel',
        store: storeDeCajas,
        columns: [{
            text     : 'Concepto',
            flex     : 1,
            sortable : true,
            dataIndex: 'Concepto'
        },{
            text     : 'Descripcion',
            width    : 180,
            sortable : true,
            dataIndex: 'Descripcion'
        },{
            text     : 'Valor',
            align	 : 'center',
            width    : 110,
           	sortable : true,
           	dataIndex: 'Valor',
           	renderer: function(value, metadata, record, store){
           		var factor= parseInt(record.get('Factor'));
           		value = formatCurrency( value*factor );
           		if (factor>0)  {
            		return '<div class="x-cell-positive">' + value + '</div>';
        		} else  {
            		return '<div class="x-cell-negative">' + value + '</div>';
        		}
           	}
        },{
            text     : 'Documento',
            width    : 120,
            sortable : true,
            dataIndex: 'Documento'
        },{
            text     : 'Nombre',
            width    : 180,
            sortable : true,
            dataIndex: 'Nombre'
        }],
        height: 300,
        width: 900,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarDeCajaPago',
            	disabled:true,
                text:'Egreso(-)',
                tooltip:'Crear Nuevo Registro de Pago',
                handler:function(){
                	formPago.getForm().reset();
                	windowPago.show();
                }
            },'-',{
            	id: 'buttonInsertarDeCajaConsignacion',
            	disabled:true,
                text:'Consignacion(-)',
                tooltip:'Crear Nuevo Registro de Consignacion',
                handler:function(){
                	showWindowDeCaja('Consignacion',-1);
                }
            },'-',{
            	id: 'buttonInsertarDeCajaSaldoPorCobrar',
            	disabled:true,
                text:'Saldo x Cobrar(-)',
                tooltip:'registro de saldo por cobrar',
                handler:function(){
                	showWindowDeCaja('Saldo x Cobrar',-1);
                }
            },'-',{
            	id: 'buttonInsertarDeCajaIngreso',
            	disabled:true,
                text:'Ingreso(+) ',
                tooltip:'Crear Nuevo Registro de Ingreso',
                handler:function(){
                	formIngreso.getForm().reset();
                	windowIngreso.show();
                }
            },'-',{
            	id: 'buttonInsertarDeCajaRutina',
            	disabled:true,
                text:'Rutina(+) ',
                tooltip:'Nuevo Registro de IngresoxRutina',
                handler:function(){
                	showWindowDeCaja('Rutina',1);
                }
            },'-',{
            	id: 'buttonInsertarDeCajaSaldoCobrado',
            	disabled:true,
                text:'Saldo Cobrado(+) ',
                tooltip:'registro de saldo cobrado',
                handler:function(){
                	showWindowDeCaja('Saldo Cobrado',1)
                }
            },'-',{
            	id: 'buttonActualizarDeCaja',
            	disabled:true,
                text:'Actualizar',
                tooltip:'Actualizar la informacion del registro',
                iconCls:'edit',
                handler:function(){
                	var r = gridDeCajas.getSelectionModel().getSelection()[0];
                	formDeCaja.getForm().loadRecord(r);
                	windowDeCaja.setTitle(r.get('Factor')==1?'Actualizar Ingreso (+)': 'Actualizar Egreso (-)' );
                	windowDeCaja.show();
                }
            },'-',{
            	id: 'buttonBorrarDeCaja',
                text:'Borrar',
                tooltip:'Borrar registro seleccionado',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
                		SessionId:sessionStorage.id,
                		Id: gridDeCajas.getSelectionModel().getSelection()[0].getId()
                	}
                	executeAjaxRequest({
                		url: url+'/json/asynconeway/DeCajaDelete',
	                	success: function(result) {
							storeDeCajas.remove(gridDeCajas.getSelectionModel().getSelection()[0]);
				    	},
						params:record
                	})
                }              
			}]
        }]
    });

 	gridDeCajas.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {
        	if ( gridCajas.getSelectionModel().getSelection()[0].get('FechaAsentado') 
        	|| selectedRecord[0].get('Concepto')=='Facturacion Ventas' ){
        		habilitarDeCajaAB(false);
        	}
        	else{
        		habilitarDeCajaAB(true);
        	}
        }
        else{
        	habilitarDeCajaAB(false);
        }
	});
  
    
    var panelModulo = Ext.create('Ext.Panel', {
        id:'panelModulo',
        baseCls:'x-plain',
        renderTo: 'modulo',
        layout: {
            type: 'table',
            columns: 1
        },
        items:[
			gridCajas,
			gridDeCajas
		]
    });
    
    var formCaja = Ext.create('Ext.form.Panel', {
        bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
        },
        defaults: {
            anchor: '100%'
        },
        items: [{
        	
        	xtype: 'fieldset',
        	title: 'Informacion del Documento',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items:[{
	            xtype:'hidden',  
	            name:'SessionId',  
	            value:sessionStorage.id
	        },{
	            fieldLabel: 'Fecha',
	            name: 'Fecha',
				format:	'd.m.Y',
				xtype     : 'datefield',
				value : new Date(),
	            allowBlank:false,
	            cls:'x-item-fecha-documento',
                height:30
	        }]
        }],
		buttons:[{ 
            text:'Crear',
            formBind: true,	 
            handler:function(){
				var form = formCaja.getForm();            
				var record = form.getFieldValues();
				
				record.Fecha= convertToUTC(record.Fecha);
				
				executeAjaxRequest({
					url: url+'/json/asynconeway/CajaInsert',
					success: function(result) {
                        var nr = Ext.ModelManager.create(result.Caja, 'Caja') ;
						storeCajas.add(nr);
						gridCajas.getSelectionModel().doSingleSelect(nr,false);
						windowCaja.hide();
				    },
					params:record
				});
			}              
	    }] 
    });

        
    var windowCaja = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 400,
         height: 150,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formCaja
         ]
    });
	
    var formPago = createFormEgresoIngreso(comboEgresos, -1, function(){windowPago.hide() } );
    
    var formIngreso = createFormEgresoIngreso(comboIngresos, 1, function(){windowIngreso.hide() });
    
    var formDeCaja = Ext.create('Ext.form.Panel', {
        bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaults: {
            anchor: '100%'
        },
        items: [{
        	
        	xtype: 'fieldset',
        	title: 'Datos del Registro',
            layout: 'anchor',
            defaultType: 'textfield',
            defaults: {
                anchor: '100%'
            },
            items:[{
	            xtype:'hidden',  
	            name:'SessionId',  
	            value:sessionStorage.id
	        },{
	            xtype:'hidden',  
	            name:'Id',  
	            value:0
	        },{
	            xtype:'hidden',  
	            name:'IdCaja',  
	            value:0
	        },{
	            xtype:'hidden',  
	            name:'Factor',  
	            value:0
	        },{
	        	fieldLabel: 'Concepto',
            	name: 'Concepto',
            	allowBlank:false,
            	enforceMaxLength:true,
            	maxLength :30,
            	readOnly: true
	        },{
	        	fieldLabel: 'Descripcion',
            	name: 'Descripcion',
            	allowBlank:false,
            	enforceMaxLength:true,
            	maxLength :80
	        },{
	            xtype: 'numberfield',
            	fieldLabel: 'Valor',
                name: 'Valor',
                value: 0,
                minValue: 0,
                allowBlank:false
	        },{
	        	fieldLabel: 'Documento',
            	name: 'Documento',
            	enforceMaxLength:true,
            	maxLength :12
	        },{
	        	fieldLabel: 'Nombre',
            	name: 'Nombre',
            	enforceMaxLength:true,
            	maxLength :50
	        }]
        }],
		buttons:[{ 
            text:'Guardar',
            formBind: true,	 
            handler:function(){
				
            	var form = formDeCaja.getForm();
            	if(!form.isValid()){
            		Ext.Msg.alert('Digite toda la informacion... ');
					return ;
            	}

				var record = form.getFieldValues();
				executeAjaxRequest({
					url: url+'/json/asynconeway/DeCaja'+( (record.Id != "0")? 'Update':'Insert' ),
					success: function(result) {
                        
						if (record.Id != "0"){
							var ur = storeDeCajas.getById(parseInt( record.Id) );
							ur.beginEdit();
							var fr = form.getFieldValues(true);
							for( var r in fr){
								ur.set(r, fr[r])
							}
							ur.endEdit();
							ur.commit();
							
						}
						else{
							var nr = Ext.ModelManager.create(result.DeCaja, 'DeCaja') ;
							storeDeCajas.add(nr);
							gridDeCajas.getSelectionModel().doSingleSelect(nr,false);	
						}					
						windowDeCaja.hide();
				    },
					params:record
				});
            	
			}              
	    }] 
    });

    
    var windowDeCaja = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 500,
         height: 250,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formDeCaja
         ]
    });
    
    var windowPago = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 500,
         height: 250,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formPago
         ]
    });
    
    
    var windowIngreso = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 500,
         height: 250,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formIngreso
         ]
    });
    
    var formTrasladarA = Ext.create('Ext.form.Panel', {
        bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
        },
        defaults: {
            anchor: '100%'
        },
        items: [{
        	
        	xtype: 'fieldset',
        	title: 'Trasladar Saldo a:',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items:[{
	            fieldLabel: 'Fecha',
	            name: 'TrasladarA',
				format:	'd.m.Y',
				xtype     : 'datefield',
				value : new Date(),
	            allowBlank:false,
	            cls:'x-item-fecha-documento',
                height:30
	        }]
        }],
		buttons:[{ 
            text:'Asentar',
            formBind: true,	 
            handler:function(){
           		asentarCaja('Asentar');
           		windowTrasladarA.hide();
            }       
	    }] 
    });
 
    
    var windowTrasladarA = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 400,
         height: 150,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formTrasladarA
         ]
    });
    
    var asentarCaja= function (accion){
		var record = {
			SessionId:sessionStorage.id,
			Id:gridCajas.getSelectionModel().getSelection()[0].getId() 
		};
		if(accion=='Asentar') record.TrasladarA=  convertToUTC(formTrasladarA.getForm().getFieldValues().TrasladarA );
		
		executeAjaxRequest({
			url: url+'/json/asynconeway/Caja'+accion,
			success: function(result) {   
				var ur = storeCajas.getById(parseInt( record.Id) );
				ur.beginEdit();
				var fr = result.Caja;
				for( var r in ur.data){
					ur.set(r, fr[r])
				}
				ur.endEdit();
				ur.commit();
				
				var ta = convertToDate(result.Caja.TrasladarA);
								
				var recordIndex = storeCajas.findBy(
			    	function(record, id){
			    		if ( Ext.util.Format.date( record.get('Fecha'),'Y.m.d')
			    			 == Ext.util.Format.date( ta, 'Y.m.d') ) return true 			        
			        	return false;  // there is no record in the store with this data
			    });
			    
			    if (recordIndex>=0){
			    	var r = storeCajas.getAt(recordIndex);
			    	r.beginEdit();
			    	r.set('SaldoAnterior', ur.get('SaldoAnterior')+ ur.get('Ingresos')- ur.get('Salidas') );
			    	r.endEdit();
					r.commit();
			    }
				
				
				habilitarOperacionesCaja(accion=='Asentar'? false:true );
				habilitarInsertarDeCaja(accion=='Asentar'? false:true);
				habilitarDeCajaAB(accion=='Asentar'? false: gridDeCajas.getSelectionModel().getSelection().length? true:false);
				Ext.getCmp('buttonBorrarCaja').setDisabled( ( storeDeCajas.count()>0  ) );
			},
			params:record
		});	
	}
    
	var showWindowDeCaja = function(concepto, factor){
		formDeCaja.getForm().reset();
        formDeCaja.getForm().setValues({
        	Id:0, 
        	IdCaja:gridCajas.getSelectionModel().getSelection()[0].getId(),
        	Concepto:concepto,
       		Factor:factor,
       		Descripcion:concepto,
       		Valor: concepto=='Rutina'? storeTiposPago.getById(1).get('Valor'):0
      	})
      	windowDeCaja.setTitle('Nuevo ' + concepto);
        windowDeCaja.show();
	}
	
	var sumarValores= function (store){
		var ingresos=0;	var salidas=0;
		store.each( function(record){
			if (record.get('Factor')== 1)	ingresos+=record.get('Valor');
			else	salidas+=record.get('Valor');
		})
		var record = gridCajas.getSelectionModel().getSelection()[0];
		record.beginEdit();
		record.set('Ingresos', ingresos);
		record.set('Salidas', salidas);
		record.endEdit();
		record.commit();
		
	}
	
	var habilitarInsertarDeCaja = function ( enable ){
		Ext.getCmp('buttonInsertarDeCajaPago').setDisabled(!enable);
		Ext.getCmp('buttonInsertarDeCajaConsignacion').setDisabled(!enable);
		Ext.getCmp('buttonInsertarDeCajaIngreso').setDisabled(!enable);
		Ext.getCmp('buttonInsertarDeCajaRutina').setDisabled(!enable);
		Ext.getCmp('buttonInsertarDeCajaSaldoPorCobrar').setDisabled(!enable);
		Ext.getCmp('buttonInsertarDeCajaSaldoCobrado').setDisabled(!enable);
	}

	var habilitarOperacionesCaja = function ( enable ){
		Ext.getCmp('buttonAsentarCaja').setDisabled( ( !enable ) || (
			Ext.util.Format.date( gridCajas.getSelectionModel().getSelection()[0].get('Fecha'),'Y.m.d')
			>= Ext.util.Format.date(new Date(), 'Y.m.d') ) );
		
		Ext.getCmp('buttonDesasentarCaja').setDisabled(enable || disableDesasentar);
		Ext.getCmp('buttonBorrarCaja').setDisabled( (! enable  ) );
	}

	var habilitarDeCajaAB= function (enable){
		Ext.getCmp('buttonActualizarDeCaja').setDisabled(!enable);
		Ext.getCmp('buttonBorrarDeCaja').setDisabled(!enable);
	}
	
	function createFormEgresoIngreso (combo, factor, onSuccess){
		
		return  Ext.create('Ext.form.Panel', {
	        bodyPadding: 15,
	        fieldDefaults: {
	            msgTarget: 'side',
	            labelWidth: 120,
				labelAlign: 'right'
	        },
	        defaults: {
	            anchor: '100%'
	        },
	        items: [{
	        	xtype: 'fieldset',
	        	title: 'Datos del Registro',
	            layout: 'anchor',
	            defaultType: 'textfield',
	            defaults: {
	                anchor: '100%'
	            },
	            items:[{
		            xtype:'hidden',  
		            name:'SessionId',  
		            value:sessionStorage.id
		        },{
		            xtype:'hidden',  
		            name:'Id',  
		            value:0
		        },{
		            xtype:'hidden',  
		            name:'IdCaja',  
		            value:0
		        },{
		            xtype:'hidden',  
		            name:'Factor',  
		            value:factor
		        },
		        	combo,
		        {
		        	fieldLabel: 'Descripcion',
	            	name: 'Descripcion',
	            	allowBlank:false,
	            	enforceMaxLength:true,
	            	maxLength :80
		        },{
		            xtype: 'numberfield',
	            	fieldLabel: 'Valor',
	                name: 'Valor',
	                value: 0,
	                minValue: 0,
	                allowBlank:false
		        },{
		        	fieldLabel: 'Documento',
	            	name: 'Documento',
	            	enforceMaxLength:true,
	            	maxLength :12
		        },{
		        	fieldLabel: 'Nombre',
	            	name: 'Nombre',
	            	enforceMaxLength:true,
	            	maxLength :50
		        }]
	        }],
			buttons:[{ 
	            text:'Guardar',
	            formBind: true,
	            handler:function(){
	            	var form = this.up('form').getForm();            	
	            	if(!form.isValid()){
	            		Ext.Msg.alert('Digite toda la informacion... ');
						return ;
	            	}
					var record = form.getFieldValues();
					record.IdCaja=gridCajas.getSelectionModel().getSelection()[0].getId();
					executeAjaxRequest({
						url: url+'/json/asynconeway/DeCajaInsert', 
						success: function(result) {
								var nr = Ext.ModelManager.create(result.DeCaja, 'DeCaja') ;
								storeDeCajas.add(nr);
								gridDeCajas.getSelectionModel().doSingleSelect(nr,false);						
							if(onSuccess) onSuccess(); 
					    },
						params:record
					});
				}              
		    }] 
	    });
	}
		    
});


