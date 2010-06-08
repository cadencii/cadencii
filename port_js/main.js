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

/*
 * ノートナンバーから，その音階を表す文字列を取得する
 */
function string_from_note( note ){
    
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
            var smf = new org.kbinani.vsq.MidiFile( stream );
            //alert( "smf.getTrackCount()=" + smf.getTrackCount() );
            var tracks = smf.getTrackCount();
            for( var track = 1; track < tracks; track++ ){
                var sw = "";
                var midi_event = smf.getMidiEventList( track );
                var count = midi_event.length;
                var buffer = new Array();
                for( var i = 0; i < count; i++ ){
                    var item = midi_event[i];
//alert( "item.data.length=" + item.data.length );
                    if( item.firstByte == 0xff && item.data.length > 0 ){
                        var type = item.data[0];
                        if( type == 0x01 || type == 0x03 ){
                            if( type == 0x01 ) {
                                var colon_count = 0;
                                for ( var j = 0; j < item.data.length - 1; j++ ) {
                                    var d = item.data[j + 1];
                                    if ( d == 0x3a ) {
                                        colon_count++;
                                        if ( colon_count <= 2 ) {
                                            continue;
                                        }
                                    }
                                    if ( colon_count < 2 ) {
                                        continue;
                                    }
                                    buffer.push( d );
                                }

                                var index_0x0a = -1;
                                for( var k = 0; k < buffer.length; k++ ){
                                    if( buffer[k] == 0x0a ){
                                        index_0x0a = k;
                                        break;
                                    }
                                }
//alert( "index_0x0a=" + index_0x0a );
                                while ( index_0x0a >= 0 ) {
                                    var cpy = new Array( index_0x0a );// byte[index_0x0a];
                                    for ( var j = 0; j < index_0x0a; j++ ) {
                                        cpy[j] = 0xff & buffer[0];
                                        buffer.shift();
                                    }

                                    var line = "";
                                    for( var k = 0; k < cpy.length; k++ ){
                                        line += String.fromCharCode( cpy[k] );
                                    }// PortUtil.getDecodedString( encoding, cpy );
                                    //sw.writeLine( line );
                                    sw += line + "\n";
//alert( "line=" + line );
                                    buffer.shift();
                                    index_0x0a = -1;
                                    for( var k = 0; k < buffer.length; k++ ){
                                        if( buffer[k] == 0x0a ){
                                            index_0x0a = k;
                                            break;
                                        }
                                    }
                                }
                            } else {
                                for ( var j = 0; j < item.data.Length - 1; j++ ) {
                                    buffer.add( item.data[j + 1] );
                                }
                                var c = buffer.length;
                                var d = new Array( c );// byte[c];
                                for ( var j = 0; j < c; j++ ) {
                                    d[j] = 0xff & buffer[j];
                                }
                                track_name = "";
                                for( var k = 0; k < d.length; k++ ){
                                    track_name += String.fromCharCode( d[k] );
                                }
alert( "track_name=" + track_name );
                                buffer = new Array();
                            }
                        }
                    }else{
                        continue;
                    }
                }
alert( "sw=" + sw );
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
