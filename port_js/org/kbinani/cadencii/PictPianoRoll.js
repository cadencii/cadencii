if( org == undefined ) var org = {};
if( org.kbinani == undefined ) org.kbinani = {};
if( org.kbinani.cadencii == undefined ) org.kbinani.cadencii = {};
if( org.kbinani.cadencii.PictPianoRoll == undefined ){

    org.kbinani.cadencii.PictPianoRoll = function(){
    };

    org.kbinani.cadencii.PictPianoRoll.prototype = {
        foo : function(){
        },

        paint : function( g ){
alert( "PictPianoRoll#paint; this.mouseMove=" + this.mouseMove );
        },

        mouseMove : function( e ){
            var ctx = e.target.getContext( "2d" );
alert( "PictPianoRoll#mouseMove; this.mouseMove=" + this.mouseMove );
            this.paint( ctx );
        },
    };

}
