Ext.Loader.setConfig({enabled: true});

Ext.Loader.setPath('Ext.ux', '../ux');

Ext.require([
    'Ext.form.*', 
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
	'Ext.ux.grid.FiltersFeature',
	'Ext.tip.*'
]);

function getDefaultEndDate( diasRestar){
	return Ext.Date.add( new Date(), Ext.Date.DAY,diasRestar|| -1);
}


Ext.onReady(function(){
	
	
	var filters = {
        ftype: 'filters',
        encode: false, 
        local: true
    };
	
    
    Ext.QuickTips.init();
    var url= location.protocol + "//" + location.host+'/servicio';
    
    var lastForm;
    var lastResult;
    
    Ext.define('FacturacionDia',{
    	extend: 'Ext.data.Model',
    	fields:[
    		{name: 'FechaPago', type:'string', convert:function(v){ return  Ext.util.Format.date(convertToDate(v),'Y.m.d' ) } },
    		{name: 'Nombre', type:'string'},
    		{name: 'Cantidad', type:'number'},
    		{name: 'Valor' ,  type:'number'}
    	]
    });
    
    Ext.define('PersonaActiva',{
    	extend:'Ext.data.Model',
    	idProperty :'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'NombreCompleto', type:'string'},
    		{name: 'FechaNacimiento', type:'string',  convert:function(v){ return  Ext.util.Format.date(convertToDate(v),'Y.m.d' ) } },
    		{name: 'Documento', type:'string'},
    		{name: 'Sexo', type:'string'},
    		{name: 'Barrio', type:'string'},
    		{name: 'Direccion', type:'string'},
    		{name: 'Telefono', type:'string'},
    		{name: 'Celular', type:'string'},
    		{name: 'Email', type:'string'},
    		{name: 'Empresa', type:'string'},
    		{name: 'UltimaFactura', type:'string'},
    		{name: 'Inicio', type:'string',  convert:function(v){ return  Ext.util.Format.date(convertToDate(v),'Y.m.d' ) } },
    		{name: 'Terminacion', type:'string',  convert:function(v){ return  Ext.util.Format.date(convertToDate(v),'Y.m.d' ) } },
    		{name: 'Valor', type:'number'},
    		{name: 'Observacion', type:'string'},
    		{name: 'UltimoIngreso', type:'string',  convert:function(v){ return  Ext.util.Format.date(convertToDate(v),'Y.m.d' ) } },
    		{name: 'Entradas', type:'int'},
    		{name: 'DiasAusencia', type:'int'},
    		{name: 'TipoPago', type:'string'}
    	]
    });
    
    
    var storeFacturacion = Ext.create('Ext.data.Store', {
        model: 'FacturacionDia',
        autoLoad:false,
        groupField: 'FechaPago'
    });
    
    var storePersonasActivas = Ext.create('Ext.data.Store', {
        model: 'PersonaActiva',
        autoLoad:false,
    });
    
    
    
    var showSummary = true;
    var gridFacturacion = Ext.create('Ext.grid.Panel', {
        width: 700,
        height: 580,
        frame: true,
        iconCls: 'icon-grid',
        store: storeFacturacion,
        dockedItems: [{
            dock: 'top',
            itemId:'gridFacturacionToolbar',
            xtype: 'toolbar',
            items: [{
                tooltip: 'Ver/Ocultar fila de Totales',
                text: 'Ver Totales',
                handler: function(){
                    var view = gridFacturacion.getView();
                    showSummary = !showSummary;
                    view.getFeature('group').toggleSummaryRow(showSummary);
                    view.refresh();
                }
            }]
        }],
        features: [{
            id: 'group',
            ftype: 'groupingsummary',
            groupHeaderTpl: '{name}',
            hideGroupedHeader: true,
            enableGroupingMenu: true,
            startCollapsed : false
        }],
        columns: [{
            text: 'Fecha de Pago',
            flex: 1,
            //tdCls: 'task',
            sortable: true,
            dataIndex: 'FechaPago',
            hideable: false,
            
        }, {
            header: 'Tipo de Pago',
            width: 120,
            sortable: true,
            dataIndex: 'Nombre'
        }, {
            header: 'Cantidad',
            width: 95,
            sortable: true,
            groupable: false,
            dataIndex: 'Cantidad',
            summaryType: 'sum',
            field: {
                xtype: 'numberfield'
            }
        }, {
            id: 'Valor',
            header: 'Valor',
            width: 115,
            sortable: false,
            groupable: false,
            renderer:Ext.util.Format.usMoney,
            dataIndex: 'Valor',
            summaryType: 'sum',
            summaryRenderer: Ext.util.Format.usMoney
        }]
    });
        
    var toolbar = gridFacturacion.child('#gridFacturacionToolbar');
    toolbar.add('->', {
    	id: 'gridFacturacionValorTotal',
        xtype    : 'textfield',
        readOnly : true,
        readOnlyCls :'x-item-readonly-valor-total'
    });
    
    
    var gridPersonasActivas = Ext.create('Ext.grid.Panel', {
        width: 950,
        height: 580,
        frame: true,
        store: storePersonasActivas,
        features: [filters],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'totalActivos', xtype: 'textfield',  width: 180, readOnly:true
            }]
        }],
        columns: [{
            header: 'Documento',
            width: 80,
            dataIndex: 'Documento',
            filter: {type: 'string'}
        }, {
            header: 'Nombre',
 			width: 160,
            dataIndex: 'NombreCompleto',
            filter: {type: 'string'}
        }, {
            header: 'Tipo Pago',
            width: 80,
            dataIndex: 'TipoPago',
        	filterable: true
        },{
        	header: 'Ausencia',
            width: 65,
            dataIndex: 'DiasAusencia',
            filter: {type: 'numeric'}
        },{
        	header: 'Terminacion',
            width: 95,
            dataIndex: 'Terminacion'
        },{
            header: 'Sexo',
            width: 40,
            dataIndex: 'Sexo'
        },{
        	header: 'Barrio',
            width: 120,
            dataIndex: 'Barrio'
        },{
        	header: 'Direccion',
            width: 120,
            dataIndex: 'Direccion'
        },{
        	header: 'Telefono',
            width: 80,
            dataIndex: 'Telefono'
        },{
        	header: 'Celular',
            width: 90,
            dataIndex: 'Celular'
        },{
        	header: 'Mail',
            width: 120,
            dataIndex: 'Email'
        },{
        	header: 'Empresa',
            width: 80,
            dataIndex: 'Empresa'
        },{
        	header: 'Factura',
            width: 80,
            dataIndex: 'UltimaFactura'
        },{
        	header: 'Inicio',
            width: 80,
            dataIndex: 'Inicio'
        },{
        	header: 'Terminacion',
            width: 80,
            dataIndex: 'Terminacion'
        },{
        	header: 'Valor',
            width: 60,
            dataIndex: 'Valor'
        },{
        	header: 'Observacion',
            width: 60,
            dataIndex: 'Observacion'
        },{
        	header: 'UltimoIngreso',
            width: 80,
            dataIndex: 'UltimoIngreso'
        },{
        	header: 'Entradas',
            width: 60,
            dataIndex: 'Entradas'
        }]
	});
    
    
	var buttonResultados= Ext.create('Ext.Button', {
    	text    : 'Estado de Resultados',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formEstado;
    			formEstado.show();
    		}
    	}
	});
	
    var buttonMatriculas= Ext.create('Ext.Button', {
    	text    : 'Matriculas',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formMatriculas;
    			formMatriculas.show();
    		}
    	}
	});
    
	var buttonFacturacionDia= Ext.create('Ext.Button', {
    	text    : 'Facturas Dia',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formFacturacionDia;
    			formFacturacionDia.show();
    		}
    	}
	});
	
	var buttonDiario= Ext.create('Ext.Button', {
    	text    : 'Diario',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formDiario;
    			formDiario.show();
    		}
    	}
	});
	
	var buttonCartera= Ext.create('Ext.Button', {
    	text    : 'Cartera',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			executeAjaxRequest({
					url: url+'/json/asynconeway/Cartera',
					success: function(result) {   					
						if(lastForm) lastForm.hide();
						Ext.create('Ext.window.Window', {
					    	height: 720,
    						width: 950,
							autoScroll : true,
    						layout: 'fit',
    						items: {  
								xtype: 'component',
							    autoEl: {
        							html: result.HtmlResponse
    							}
							}
						}).show();							
						lastResult=null;
				    },
					params:{ SessionId:sessionStorage.id} 
				});
    		}
    	}
	});
	
	
	var buttonActivos= Ext.create('Ext.Button', {
    	text    : 'Activos',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formActivos;
    			formActivos.show();
    		}
    	}
	});
	
	var buttonInactivos= Ext.create('Ext.Button', {
    	text    : 'Ausencias',
    	scale   : 'medium',
    	flex:1,
    	handler	: function(){
    		if(sessionStorage.id){
    			hideLast();
    			lastForm=formInactivos;
    			formInactivos.show();
    		}
    	}
	});
	
	var formEstado = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 115,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Fecha Inicial',
            name: 'Desde',
            format:	'd.m.Y',
			xtype     : 'datefield',
			value: getDefaultEndDate(-1),
            allowBlank:false
        },{
            fieldLabel: 'Fecha Final',
            name: 'Hasta',
            format:	'd.m.Y',
            xtype     : 'datefield',
            value: getDefaultEndDate(-1),
            allowBlank:false
        },{
        	xtype     : 'checkboxfield',
            fieldLabel  : 'Enviar Mail',
            name      : 'SendMail',
            inputValue: false,
            checked   : false,
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){
            	var form = formEstado.getForm();
            	var record = form.getFieldValues();
            	record.Desde= convertToUTC(record.Desde);
            	record.Hasta= convertToUTC(record.Hasta);
            	
            	executeAjaxRequest({
					url: url+'/json/asynconeway/CajaConsolidadoGet',
					success: function(result) {   
						
						lastForm.hide();
						Ext.create('Ext.window.Window', {
					    	height: 620,
    						width: 650,
							autoScroll : true,
    						layout: 'fit',
    						items: {  
								xtype: 'component',
							    autoEl: {
        							html: result.HtmlResponse
    							}
							}
						}).show();							
						lastResult=null;
				    },
					params:record
				});
            	
			}              
	    }] 
    });
	
	
	var formMatriculas = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 115,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Fecha Inicial',
            name: 'FechaInicial',
            format:	'd.m.Y',
			xtype     : 'datefield',
			value: new Date(),
            allowBlank:false
        },{
            fieldLabel: 'Fecha Final',
            name: 'FechaFinal',
            format:	'd.m.Y',
            xtype     : 'datefield',
            value: new Date(),
            allowBlank:false
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){
            	var form = formMatriculas.getForm();
            	var record = form.getFieldValues();
            	record.FechaInicial= convertToUTC(record.FechaInicial);
            	record.FechaFinal= convertToUTC(record.FechaFinal);
            	executeAjaxRequest({
					url: url+'/json/asynconeway/FacturacionDiaGet',
					success: function(result) {   
						storeFacturacion.setProxy(new Ext.data.proxy.Memory({
							model:'FacturacionDia',
							data: result.FacturacionDia
						}) );
						
						lastForm.hide();
						gridFacturacion.show();
						storeFacturacion.load();
						gridFacturacion.determineScrollbars();
						var suma = formatString( storeFacturacion.sum('Valor'));
						Ext.getCmp('gridFacturacionValorTotal').setValue(suma);
						lastResult=gridFacturacion;
				    },
					params:record
				});
            	
			}              
	    }] 
    });
	
    var formFacturacionDia = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 115,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Fecha',
            name: 'Fecha',
            format:	'd.m.Y',
			xtype     : 'datefield',
			value: new Date(),
            allowBlank:false
        },{
        	xtype     : 'checkboxfield',
            fieldLabel  : 'Enviar Mail',
            name      : 'SendMail',
            inputValue: false,
            checked   : false,
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){
            	var form = formFacturacionDia.getForm();
            	var record = form.getFieldValues();
            	record.Fecha= convertToUTC(record.Fecha);
            	
            	executeAjaxRequest({
					url: url+'/json/asynconeway/DetalleFacturasDiaGet',
					success: function(result) {   
						lastForm.hide();
						Ext.create('Ext.window.Window', {
					    	height: 620,
    						width: 850,
							autoScroll : true,
    						layout: 'fit',
    						items: {  
								xtype: 'component',
							    autoEl: {
        							html: result.HtmlResponse
    							}
							}
						}).show();							
						lastResult=null;
				    },
					params:record
				});
            	
			}              
	    }] 
    });
    
    
    var formDiario = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 115,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Fecha',
            name: 'Fecha',
            format:	'd.m.Y',
			xtype     : 'datefield',
			value: new Date(),
            allowBlank:false
        },{
        	xtype     : 'checkboxfield',
            fieldLabel  : 'Enviar Mail',
            name      : 'SendMail',
            inputValue: false,
            checked   : false,
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){
            	var form = formDiario.getForm();
            	var record = form.getFieldValues();
            	record.Fecha= convertToUTC(record.Fecha);
            	
            	executeAjaxRequest({
					url: url+'/json/asynconeway/CajaCierreGet',
					success: function(result) {   
						lastForm.hide();
						Ext.create('Ext.window.Window', {
					    	height: 620,
    						width: 850,
							autoScroll : true,
    						layout: 'fit',
    						items: {  
								xtype: 'component',
							    autoEl: {
        							html: result.HtmlResponse
    							}
							}
						}).show();							
						lastResult=null;
				    },
					params:record
				});
			}              
	    }] 
    });
    
    
    var formActivos = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 260,
		height: 100,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Fecha Corte',
            name: 'FechaCorte',
            format:	'd.m.Y',
			xtype     : 'datefield',
			value: getDefaultEndDate(),
            allowBlank:true
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){	
            	
            	var form = formActivos.getForm();
            	var record = form.getFieldValues();
            	record.FechaCorte=  record.FechaCorte || getDefaultEndDate();
            	record.FechaCorte = convertToUTC(record.FechaCorte);
            	
            	executeAjaxRequest({
					url: url+'/json/asynconeway/PersonasActivasGet',
					success: function(result) {   
						storePersonasActivas.setProxy(new Ext.data.proxy.Memory({
							model:'PersonaActiva',
							data: result.PersonasActivas
						}) );
						
						lastForm.hide();
						gridPersonasActivas.show();
						storePersonasActivas.load();
						gridPersonasActivas.determineScrollbars();
						lastResult=gridPersonasActivas;
						Ext.getCmp('totalActivos').setValue("Total Activos: " + storePersonasActivas.count());
						
				    },
					params:record
				});
            	
			}              
	    }] 
    });
    
    
    var formInactivos = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 380,
		height: 110,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right'
        },
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
    		xtype: 'fieldset',
            title: 'Ausencias',
            collapsible: false,
            defaults: {
                anchor: '100%',
                layout: {
                    type: 'hbox',
                    defaultMargins: {top: 0, right: 5, bottom: 0, left: 0}
                }
            },
            items: [
                {
                    xtype: 'fieldcontainer',
                    fieldLabel: 'Dias de Ausencia',
                    defaults: {
                        hideLabel: true
                    },
                    items: [
                        {xtype: 'displayfield', value: 'entre', submitValue:false , id:'entreId'},
                        {xtype: 'numberfield',    fieldLabel: 'entre', 
                        	name: 'DiasAusenciaTopeInferior', width: 60, allowBlank: false, value: 15},
                        {xtype: 'displayfield', value: 'y', submitValue:false, id:'yId'},
                        {xtype: 'numberfield',    fieldLabel: 'y', name: 'DiasAusenciaTopeSuperior', width: 60, allowBlank: false, 
                        		value: 30,  margins: '0 5 0 0'}
                    ]
                }
            ]
        }],
		buttons:[{ 
            text:'Consultar',
            formBind: true,	 
            handler:function(){	
            	
            	var form = formInactivos.getForm();
            	var record = form.getFieldValues();
            	if(record.DiasAusenciaTopeSuperior< record.DiasAusenciaTopeInferior){
            		Ext.Msg.alert(' El segundo valor debe ser mayor al primer valor de dias de ausencia ');
            		return;
            	};
            	
            	executeAjaxRequest({
					url: url+'/json/asynconeway/PersonasInactivasGet',
					success: function(result) {   
						storePersonasActivas.setProxy(new  Ext.data.proxy.Memory ({
							model:'PersonaActiva',
							data: result.PersonasInactivas
						}) );
						
						lastForm.hide();
						gridPersonasActivas.show();
						storePersonasActivas.load();
						gridPersonasActivas.determineScrollbars();
						lastResult=gridPersonasActivas;
						Ext.getCmp('totalActivos').setValue("Total ausencias: " + storePersonasActivas.count());
				    },
					params:record
				});
            	
			}              
	    }] 
    });
    
    formEstado.hide();
    formFacturacionDia.hide();
    formDiario.hide();
    formActivos.hide();
    formInactivos.hide();
    formMatriculas.hide();
    gridFacturacion.hide();
    gridPersonasActivas.hide();
    
	var viewport = Ext.create('Ext.Viewport', {
        layout:'border',
        items: [{
            id:'menu',
            region:'north',
            baseCls:'x-plain',
            split: true,
            height: 45,
            maxHeight: 150,
            layout:'fit',
            margins: '2 2 0 2',
            items: [{
            	layout: {                        
        			type: 'hbox',
                    align:'middle'
    			},
    			defaults:{margins:'5 5 5 5'},
        		items:[
        			buttonResultados,buttonMatriculas,buttonFacturacionDia,buttonDiario,buttonCartera,buttonActivos,buttonInactivos
        		]
            }]
       		}, {
       		id:'forms',	
            region:'center',
            margins: '0 5 5 5',
            layout:'fit',
            items:[{
            	layout: {                        
        			type: 'vbox',
                    align:'center'
    			},
    			defaults:{margins:'5 5 5 5'},
    			items: [formEstado, formActivos, formMatriculas, formFacturacionDia, formDiario, formInactivos, gridFacturacion, gridPersonasActivas ]
            }]
       }]
	})
	
	var hideLast= function(){
		if(lastForm) lastForm.hide();
		if(lastResult) lastResult.hide();
	}
	    
});


