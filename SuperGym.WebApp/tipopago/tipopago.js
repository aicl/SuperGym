Ext.require([
    'Ext.form.*',
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
    'Ext.tip.*'
]);
 
Ext.onReady(function(){

    Ext.QuickTips.init();
    var url= location.protocol + "//" + location.host+'/servicio';
    var modo='Insert';
	/*
	* ==========  Modelos ==========================
	*/ 
	    
	Ext.define('TipoPago', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {name:'Id', type:'int', defaultValue:0},
            {name:'Nombre', type: 'string'},
            {name:'Valor',  type:'number' },
            {name:'Dias', type:'int', defaultValue:0}, 
            {name:'ValidoDesde', type :'date', convert: function(v){ return  convertToDate(v) } },
            {name:'ValidoHasta', type :'date', convert: function(v){ return  convertToDate(v) } }
        ]
    });
    
    var storeTiposPago = Ext.create('Ext.data.Store', {
	    model: 'TipoPago',
		proxy:  createAjaxProxy( {
	        url : url+'/json/asynconeway/TiposPagoGet',
        	root: 'TiposPago'
    	}),
	    autoLoad: true
	});
       
    var gridTiposPago = Ext.create('Ext.grid.Panel', {
    	frame:true,
		height: 300,
        width: 550,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
		selType: 'rowmodel',
        store: storeTiposPago,
        columns: [{
            text     : 'Descripcion',
            flex     : 1,
            sortable : true,
            dataIndex: 'Nombre'
        },{
            text     : 'Valor',
            width    : 100,
            sortable : true,
            dataIndex: 'Valor',
            renderer: Ext.util.Format.usMoney
        },{
            text     : 'Dias',
            width    : 100,
           	sortable : true,
            dataIndex: 'Dias'
        },{
            text     : 'Desde',
            width    : 100,
            sortable : true,
            dataIndex: 'ValidoDesde',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Hasta',
            width    : 100,
            sortable : true,
            dataIndex: 'ValidoHasta',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        }],
        listeners: {
            selectionchange: function(model, records) {
                if (records[0]) {
                    formTipoPago.getForm().loadRecord(records[0]);
                    modo='Update';
                }
                else{
                	formTipoPago.getForm().reset();
                	modo='Insert';
                }
                Ext.getCmp('buttonGuardar').setText( (modo=='Insert')?'Insertar':'Actualizar');
                Ext.getCmp('removeButton').setDisabled(records.length == 0);
            }
        },
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
                text:'Nuevo',
                tooltip:'Crear nuevo tipo de pago',
                iconCls:'add',
                handler:function(){
                	modo='Insert';
                	formTipoPago.getForm().reset();
                	Ext.getCmp('buttonGuardar').setText( 'Insertar');
                }
            }, '-',{
                id: 'removeButton',
                text:'Borrar',
                tooltip:'Borrar el tipo de pago seleccionado',
                iconCls:'remove',
                disabled: true,
                handler:function(){
                	executeAjaxRequest({
                		url: url+'/json/asynconeway/TipoPagoDelete',
	                	success: function(result) {
							gridTiposPago.getStore().remove(gridTiposPago.getSelectionModel().getSelection()[0]);
							modo='Insert';
				    	},
						callback: function(result, success) {
							Ext.getCmp('buttonGuardar').setText( (modo=='Insert')?'Insertar':'Actualizar');	
	    				},
						params:{Id:gridTiposPago.getSelectionModel().getSelection()[0].data.Id, SessionId: sessionStorage.id }
                	})    	
                }
            }]
        }],
    });
    
     
    var formTipoPago = Ext.create('Ext.form.Panel', {
    	frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 350,
        height: 250,
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
            name: 'Id'
        },{
            fieldLabel: 'Descripcion',
            name: 'Nombre',
            allowBlank:false
        },{
        	xtype:'numberfield',
        	value: 1,
            minValue: 1,
            fieldLabel: 'Valor',
            name: 'Valor',
            allowBlank:false
        },{
            xtype:'numberfield',
        	value: 1,
            minValue: 1,
            fieldLabel: 'Dias',
            name: 'Dias',
            allowBlank:false
        },{
            fieldLabel: 'Valido Desde',
            name: 'ValidoDesde',
			format:	'd.m.Y',
			xtype     : 'datefield'
        },{
            fieldLabel: 'Hasta',
            name: 'ValidoHasta',
			format:	'd.m.Y',
			xtype     : 'datefield'
        }],
		buttons:[{ 
			id:'buttonGuardar',
            text:'Insertar',
            formBind: true,	 
            handler:function(){
				var form = formTipoPago.getForm();
				
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Registre todos los datos por favor -) ');
					return ;
				}	
                
                var record = form.getFieldValues();
                if(modo=='Insert'){
                	delete record.Id;
                }
                
				record.ValidoDesde= convertToUTC(record.ValidoDesde);
				record.ValidoHasta= convertToUTC(record.ValidoHasta);
				
				executeAjaxRequest({
					url: url+'/json/asynconeway/TipoPago'+modo,
				    success: function(result) {
						if(modo=='Insert'){
							var nr = Ext.ModelManager.create(result.TipoPago, 'TipoPago')
							form.loadRecord(nr);
							storeTiposPago.add(nr);
							gridTiposPago.getSelectionModel().doSingleSelect(nr,false);
						}
						else{
							var ur = storeTiposPago.getById(parseInt( record.Id) );
							ur.beginEdit();
							var fr = form.getFieldValues(true);
							for( var r in fr){
								ur.set(r, fr[r])
							}
							ur.endEdit();
							ur.commit();
						}
						modo='Update';						
				    },
					callback: function(result, success) {
						Ext.getCmp('buttonGuardar').setText( (modo=='Insert')?'Insertar':'Actualizar');
	    			},
					params:record
				})				
			}              
	    }] 
       
    });
    
    var panelModulo = Ext.create('Ext.Panel', {
    	frame:true,
    	title: 'Tipos de Pago',
    	id:'panelModulo',
    	width: 910,
        renderTo: 'modulo',
        layout: {
            type: 'table',
            columns: 2
        },
        items:[
			gridTiposPago,
			formTipoPago
		]
    });

    
});


