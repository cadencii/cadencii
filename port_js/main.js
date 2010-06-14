function dragenter( e ){
    dropbox.setAttribute( "dragenter", true );
}

var dropbox;
var keyA, keyB, bar;
var filename;

function init(){
    window.addEventListener( "dragenter", dragenter, true );
    dropbox = document.getElementById( "dropbox" );
    window.addEventListener( "dragleave", dragleave, true );
    dropbox.addEventListener( "dragover", dragover, true );
    dropbox.addEventListener( "drop", drop, true );
    filename = document.getElementById( "filename" );
    keyA = new Array();
    keyB = new Array();
    bar = new Array();
    for( var i = 0; i < 128; i++ ){
        var k = i + "";
        if( i < 10 ){
            k = "00" + k;
        }else if( i < 100 ){
            k = "0" + k;
        }
        var kname = "key" + k;
        keyA.push( document.getElementById( kname + "a" ) );
        keyB.push( document.getElementById( kname + "b" ) );
        bar.push( document.getElementById( "bar" + k ) );
    }
    for( var i = 0; i < keyB.length; i++ ){
        keyB[i].addEventListener( "mouseover", key_mouseover, true );
        keyB[i].addEventListener( "mouseout", key_mouseout, true );
        keyA[i].addEventListener( "mouseover", key_mouseover, true );
        keyA[i].addEventListener( "mouseout", key_mouseout, true );
        bar[i].addEventListener( "mouseover", key_mouseover, true );
        bar[i].addEventListener( "mouseout", key_mouseout, true );
    }
}

function key_mouseout( e ){
    var id = e.target.id;
    var i = parseInt( id.substr( 3, 3 ), 10 );
    keyB[i].setAttribute( "class", "key" );
    if( i % 12 != 0 ){
        keyB[i].innerHTML = "";
    }
}

function key_mouseover( e ){
    var id = e.target.id;
    var i = parseInt( id.substr( 3, 3 ), 10 );
    keyB[i].setAttribute( "class", "key_mouseover" );
    if( i % 12 != 0 ){
        keyB[i].innerHTML = org.kbinani.vsq.VsqNote.getNoteString( i );
    }
}

function get_style_attribute( style, attr ){
    attr = attr + ":";
    var indx = style.indexOf( attr );
    var indx2 = style.indexOf( ";", indx + attr.length );
    return oldvalue = style.substring( indx + attr.length, indx2 );
}

function set_style_attribute( style, attr, value ){
    attr = attr + ":";
    var indx = style.indexOf( attr );
    var indx2 = style.indexOf( ";", indx + attr.length );
    var prefix = "";
    if( indx > 0 ){
        prefix = style.substring( 0, indx - 1 );
    }
    return style = prefix + attr + value + style.substring( indx2 );
}

function drop( e ){
    var files = e.dataTransfer.files;

    // ブラウザのデフォルトの動作を抑制
    e.preventDefault();

    // ファイルでないものがドロップされた場合
    if( files.length == 0 ){
        return;
    }

    // 1個目のファイルだけ取り扱う
    handleFile( files[0] );
}

function handleFile( file ){
    alert( file.name );
    var reader = new FileReader();

    reader.onloadend = function(){
        var dat = reader.result;
        var search = "data:application/octet-stream;base64,";
        if( dat.indexOf( search ) == 0 ){
            var b64 = dat.substring( search.length );
            var decoded = org.kbinani.Base64.decode( b64 );
            var stream = new org.kbinani.ByteArrayInputStream( decoded );
            var vsq = new org.kbinani.vsq.VsqFile( stream, "Shift_JIS" );
            alert( "number of tracks=" + vsq.Track.length );
            for( var i = 0; i < vsq.Track.length; i++ ){
                alert( "track#" + i );
                var vsq_track = vsq.Track[i];
                alert( "number of events=" + vsq_track.getEventCount() );
                var c = vsq_track.getEventCount();
                for( var j = 0; j < c; j++ ){
                    var item = vsq_track.getEvent( j );
                    alert( "clock=" + item.Clock + "; ID.type=" + item.ID.type );
                }
            }
        }
    }

    reader.readAsDataURL( file );
}

function dragover( e ){
    e.preventDefault();
}

function dragleave( e ){
    dropbox.removeAttribute( "dragenter" );
}

window.addEventListener( "load", init, true );
