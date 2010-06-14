if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.ByRef == undefined ){

    org.kbinani.ByRef = function(){
        this.value = null;
        if( arguments.length == 1 ){
            this._init_1( arguments[0] );
        }
    };

    org.kbinani.ByRef.prototype = {
        _init_1 : function( value ){
            this.value = value;
            return this;
        },
    };

}
