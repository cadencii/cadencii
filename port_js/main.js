var dropbox;
var filename;

function init(){
    window.addEventListener( "dragenter", dragenter, true );
    dropbox = document.getElementById( "dropbox" );
    window.addEventListener( "dragleave", dragleave, true );
    dropbox.addEventListener( "dragover", dragover, true );
    dropbox.addEventListener( "drop", drop, true );
    filename = document.getElementById( "filename" );
}

function drop( e ){
	var dt = e.dataTransfer;
	var files = dt.files;

    e.preventDefault();

	if( files.length == 0 ){
		return;
	}

	for( var i = 0; i < files.length; i++ ){
        handleFile( files[i] );
	}
}

function handleFile( file ){
    alert( file.name );
    var reader = new FileReader();
    reader.onloadend = function(){
        var dat = reader.result;
        var search = "data:application/octet-stream;base64,";
        //dropbox.innerHTML = file.name;
        if( dat.indexOf( search ) == 0 ){
            var b64 = dat.substring( search.length );
            var decoded = org.kbinani.Base64.decode( b64 );
            //var smf = new org.kbinani.vsq.MidiFile( decoded );
            //var tracks = smf. ...
        }
    }
    reader.readAsDataURL( file );
}

function dragover( e ){
	e.preventDefault();
}

function dragenter( e ){
    dropbox.setAttribute( "dragenter", true );
}

function dragleave( e ){
	dropbox.removeAttribute( "dragenter" );
}

window.addEventListener( "load", init, true );
