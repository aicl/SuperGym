Ext.require([
    'Ext.form.*', 
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
	'Ext.tip.*'
]);

Ext.onReady(function(){
	
    var url= location.protocol + "//" + location.host+'/servicio';
    
    Ext.QuickTips.init();
    
    Ext.define('Concepto',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Descripcion', type:'string'},
    		{name: 'Factor', type:'int'},
    		{name: 'Clasificacion' ,  type:'string'}
    	]
    });
    
	var storeConceptos= Ext.create('Ext.data.Store', {
		model:'Concepto',
		proxy:  createAjaxProxy( {
	        url : url+'/json/asynconeway/ConceptosGet',
        	root: 'Conceptos'
    	}),
		autoLoad:true,
	});
	    	
	
	var gridConceptos = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeConceptos,
        height: 500,
        width: 400,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Descripcion',
            flex     : 1,
            sortable : true,
            dataIndex: 'Descripcion'
        },{
            text     : 'Saldo En Caja',
            width    : 120,
            dataIndex: 'Factor',
            renderer: function(value, metadata, record, store){
            	if(value==1)
            		return "Sumar";
            	else if(value==-1)
            		return "Restar";
            	else
            		return "Error!!!";
           	}
        },{
            text     : 'Estado de Resultados',
            width    : 120,
            sortable : true,
            dataIndex: 'Clasificacion',
        }],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarConcepto',
                text:'Nuevo',
                tooltip:'Crear Nuevo Concepto',
                iconCls:'add',
                handler:function(){
                		showWindowConcepto("Insert");
                }
            },'-',{
            	id: 'buttonActualizarConcepto',
                text:'Actualizar',
                tooltip:'Actualizar Concepto',
                disabled:true,
                iconCls:'edit',
                handler:function(){
                	showWindowConcepto("Update");
                }
            },'-',{
            	id: 'buttonBorrarConcepto',
                text:'Borrar',
                tooltip:'Borrar Concepto',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridConceptos.getSelectionModel().getSelection()[0].getId() 
	    			};
	    			executeAjaxRequest({
						url: url+'/json/asynconeway/ConceptoDelete',
						success: function(result) {   
							storeConceptos.remove(gridConceptos.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    });

 	gridConceptos.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonActualizarConcepto').setDisabled(false);
        	Ext.getCmp('buttonBorrarConcepto').setDisabled(false);
        }
        else{
        	Ext.getCmp('buttonActualizarConcepto').setDisabled(true);
        	Ext.getCmp('buttonBorrarConcepto').setDisabled(true);
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
			gridConceptos
		]
    });
    
    var formConcepto = Ext.create('Ext.form.Panel', {
        bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%',
			labelStyle: 'padding-left:4px;'
        },
        items: [{	
        	xtype:'hidden',  
	        name:'SessionId',  
	        value:sessionStorage.id
	    },{
	        xtype:'hidden',  
	        name:'Id'
	    },{
	        fieldLabel: 'Descripcion',
	        name: 'Descripcion',
			type     : 'textfield',
			allowBlank:false      
        },{
        	xtype: 'fieldset',
            title: 'Estado de Resultados',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [{
            	xtype: 'radiogroup',
            	//fieldLabel: 'Tipo',
            	cls: 'x-check-group-alt',
            	items: [
                	{boxLabel: 'Ingresos', name: 'Clasificacion', inputValue: 'Ingresos'},
                	{boxLabel: 'Gastos',   name: 'Clasificacion', inputValue: 'Gastos'},
                	{boxLabel: 'Ninguno',  name: 'Clasificacion', inputValue: 'Otros'},
            	]
            }]
        },{
        	xtype: 'fieldset',
            title: 'Saldo en Caja',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [{
            	xtype: 'radiogroup',
            	//fieldLabel: 'Accion',
            	cls: 'x-check-group-alt',
            	items: [
                	{boxLabel: 'Restar', name: 'Factor', inputValue: -1},
                	{boxLabel: 'Sumar',   name: 'Factor', inputValue: 1}
            	]
            }]
        }],
		buttons:[{ 
            text:'Guardar',
            formBind: true,	 
            handler:function(){
				var form = formConcepto.getForm();            
				var record = form.getFieldValues();	
				executeAjaxRequest({
					url: url+'/json/asynconeway/Concepto'+( (record.Id != "0")? 'Update':'Insert' ),
					success: function(result) {
						if (record.Id != "0"){
							var ur = storeConceptos.getById(parseInt( record.Id) );
							ur.beginEdit();
							var fr = form.getFieldValues(true);
							for( var r in fr){
								ur.set(r, fr[r])
							}
							ur.endEdit();
							ur.commit(); 
						}
						else{
                        	var nr = Ext.ModelManager.create(result.Concepto, 'Concepto') ;
							storeConceptos.add(nr);
							gridConceptos.getSelectionModel().doSingleSelect(nr,false);
							windowConcepto.hide();
						}
				    },
					params:record
				});
			}              
	    }] 
    });

        
    var windowConcepto = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 400,
         height: 265,
         layout: 'fit',
         modal: true,
         y:10,
         items:[
         	formConcepto
         ]
    });
	    
  	var showWindowConcepto = function(modo){
		
  		if(modo=="Insert"){
  			formConcepto.getForm().reset();
  			formConcepto.getForm().setValues({
        	Id:0}); 
  		}
  		else{
  			var r = gridConceptos.getSelectionModel().getSelection()[0];
            formConcepto.getForm().loadRecord(r);
      	}
  			
      	windowConcepto.setTitle( (modo=="Insert")? "Insertar Concepto":"Actualizar Concepto");
        windowConcepto.show();
	}
	    
});


