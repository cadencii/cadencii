if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.windows == undefined ) org.kbinani.windows = {};
if( org.kbinani.windows.forms == undefined ) org.kbinani.windows.forms = {};
if( org.kbinani.windows.forms.Component == undefined ){
    
    org.kbinani.windows.forms.Component = function(){
    };

    org.kbinani.windows.forms.Component.prototype = {
        bind : function( div ){
            this.canvas = document.createElement( "canvas" );
            this.canvas.setAttribute( "style", "width: 100%; height: 100%;" );
            this.ctx = this.canvas.getContext( "2d" );
            this.div = div;
            this.div.innerHTML = "";
            this.div.style.overflow = "hidden";
            this.div.appendChild( this.canvas );
        },

        _resize : function(){
            var size = bytefx.$size( this.div );
            var w = size[0];
            var h = size[1];
            this.canvas.setAttribute( "width", w );
            this.canvas.setAttribute( "height", h );
            return size;
        },

        /**
         * @return [org.kbinani.java.awt.Dimension]
         */
        getSize : function(){
            var size = bytefx.$size( this.div );
            return new org.kbinani.java.awt.Dimension( size[0], size[1] );
        },

        /**
         * @return [int]
         */
        getWidth : function(){
            var size = this.getSize();
            return size.width;
        },

        /**
         * @return [int]
         */
        getHeight : function(){
            var size = this.getSize();
            return size.height;
        },

        setSize : function( value ){
            this.div.style.width = value.width + "px";
            this.div.style.height = value.height + "px";
            this.canvas.setAttribute( "width", value.width );
            this.canvas.setAttribute( "height", value.height );
        },

        setWidth : function( value ){
            this.div.style.width = value + "px";
            this.canvas.setAttribute( "width", value );
        },

        setHeight : function( value ){
            this.div.style.height = value + "px";
            this.canvas.setAttribute( "height", value );
        },
    };

}
