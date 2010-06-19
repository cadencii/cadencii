if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.windows == undefined ) org.kbinani.windows = {};
if( org.kbinani.windows.forms == undefined ) org.kbinani.windows.forms = {};
if( org.kbinani.windows.forms.TestImplement == undefined ){

    org.kbinani.windows.forms.TestImplement = function(){
    };

    org.kbinani.windows.forms.TestImplement.prototype = new org.kbinani.windows.forms.Component();

    org.kbinani.windows.forms.TestImplement.prototype.repaint = function(){
        var size = this._resize();
        this.ctx.fillStyle = "rgb(104,104,104)";
        this.ctx.strokeRect( 0, 0, size[0] - 1, size[1] - 1 );
    };
}
