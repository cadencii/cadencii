<?php
class POFile{
    static $dict = array();

    function init( $po_file ){
        $f = fopen( $po_file, "r" );
        if( !$f ){
            return;
        }
        $current = "";
        while( !feof( $f ) ){
            $line = fgets( $f );
            $indx = strpos( $line, "msgid" );
            if( $indx == 0 ){
                $current = "msgid";
            }
            $indx = strpos( $line, "msgstr" );
            if( $indx == 0 ){
                $current = "msgstr";
            }
            //TODO: この辺から
        }
        fclose( $f );
    }

    public function getMessage( $message ){
        $s = $dict[$message];
        if( is_null( $s ) ){
            return $message;
        }else{
            return $s;
        }
    }
}
?>

<?php
class Messaging{
    static $dict = array();
    static $current_lang = "en";

    static public function init(){
        $basedir = "../";
        $d = opendir( $basedir );
        while( false !== ($f = readdir( $d )) ){
            $len = strlen( $f );
            if( $len <= 3 ){
                continue;
            }
            $indx = strripos( $f, ".po" );
            if( $indx == $len - 3 ){
                $lang = substr( $f, 0, $len - 3 );
                $po = new POFile();
                $po->init( $basedir . $f );
                $dict[$lang] = $po;
            }
        }
        closedir( $d );
    }

    static public function gettext( $message ){
        $d = $dict[$current_lang];
        if( is_null( $d ) ){
            return $message;
        }else{
            return $d->getMessage( $message );
        }
    }

    static public function setLanguage( $lang ){
        $current_lang = $lang;
    }

    static public function getLanguage(){
        return $current_lang;
    }
}

Messaging::init();
Messaging::setLanguage( $argv[1] );

function _( $message ){
    echo Messaging::gettext( $message );
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<META HTTP-EQUIV="Content-type" CONTENT="text/html; charset=UTF-8">
<title>Cadencii version 3.2 Manual</title>
<link rel="stylesheet" type="text/css" href="style.css">
</head>
<body>
  <h2>Cadencii verion 3.2 Manual</h2>

  <!-- section 0 -->
  <h3>0. <?php _( "Table of contents" ) ?></h3>
  <div class=indent>
    <ol>
      <li><a href="#preface"><?php _( "Preface" ) ?></a>
      <li><a href="#platform"><?php _( "Platform" ) ?></a>
      <li><a href="#how_to_install"><?php _( "How to install" ) ?></a>
      <li><a href="#how_to_use"><?php _( "How to use" ) ?></a>
      <li><a href="#faq"><?php _( "FAQ" ) ?></a>
      <li><a href="#license"><?php _( "License" ) ?></a>
    </ol>
  </div>

  <!-- section 1 -->
  <a name="preface"></a>
  <h3>1. <?php _( "Preface" ) ?></h3>
  <p class=indent>
    Cadenncii is a free software for editing scores for several singing synthesis systems.
    At this time, <a href="http://www.vocaloid.com/en/index.html">VOCALOID</a>,
    <a href="http://www.vocaloid.com/index.en.html">VOCALOID2</a>,
    <a href="http://utau2008.web.fc2.com/">UTAU</a>, STRAIGHT with UTAU, and 
    <a href="http://www.a-quest.com/products/aquestone_en.html">AquesTone</a> are available as synthesizer.
  </p>
  <p class=indent>
    Screen shot<br />
    <img src="" alt="screen shot of Cadencii">
  </p>

  <!-- section 2 -->
  <a name="platform"></a>
  <h3>2. Platform</h3>
  <p class=indent>
    Operating System: Windows 2000, Windows XP, Windows Vista, Windows7<br />
    with .NET Framework 2.0 or later
  </p>
  <!-- section 3 -->
  <a name="how_to_install"></a>
  <h3>3. How to install</h3>

  <!-- section 4 -->
  <a name="how_to_use"></a>
  <h3>4. How to use</h3>

  <!-- section 5 -->
  <a name="faq"></a>
  <h3>5. FAQs (Frequently Asked Questions)</h3>

  <!-- section 6 -->
  <a name="license"></a>
  <h3>6. License</h3>
</body>
</html>
