/*

*/
Ext.require(['*']);
Ext.onReady(function() {
    
	var buttons=[];
	var i=0;
	var grupos = Ext.decode(sessionStorage.grupos);
	for(var grupo in grupos ){
		buttons[i]= Ext.create('Ext.Button', {
    		text    : grupos[grupo].Nombre,
    		directory:grupos[grupo].Directorio,
    		scale   : 'large',
    		handler	: function(){
    			Ext.getDom('iframe-win').src = '../'+this.directory;
    		}
		});
		i++;
	};
	
	if(sessionStorage.usuario!='registro.bp'){
		buttons[i]= Ext.create('Ext.Button', {
	    	text    : 'Salir',
	    	scale   : 'large',
	    	handler	: function(){
	    		logout();
	 
	    	}
		});
	}
	
    Ext.create('Ext.Viewport', {
        layout: {
            type: 'border',
            padding: 5
        },
        defaults: {
            split: true
        },
        items: [{
            region: 'west',
            layout:'fit',
            items:[{
            	layout: {                        
        			type: 'vbox',
        			align:'stretch'
    			},
    			defaults:{margins:'5 5 5 5'},
        		items:buttons
            }],
            collapsible: true,
            split: true,
            width: '20%'
        },{
            region: 'center',
            layout:'fit',
            items:[{
        		xtype : "component",
        		id    : 'iframe-win', 
        		autoEl : {
            		tag : "iframe",
            		src : "intro.html"
        		}
            }]
        }]
    });
});