<?php

class POFile{
    static $dict = array();

    function init( $po_file ){
        $f = fopen( $po_file, "r" );
        if( !$f ){
            return;
        }
        $current = "";
        $msgid = "";
        $msgstr = "";
        while( !feof( $f ) ){
            $line = rtrim( fgets( $f ) );
            
            if( strpos( $line, "msgid" ) === 0 ){
                $current = "msgid";
                if( strcmp( $msgid, "" ) != 0 ){
                    $this->dict[$msgid] = $msgstr;
                }
                $indx = strpos( $line, "\"", 5 ); // p̊Jnʒu
                $msgid = substr( $line, $indx + 1 );
                $msgid = $this->trimQuote( $msgid );
                $msgstr = "";
            }else if( strpos( $line, "msgstr" ) === 0 ){
                $current = "msgstr";
                $indx = strpos( $line, "\"", 6 );
                $msgstr = substr( $line, $indx + 1 );
                $msgstr = $this->trimQuote( $msgstr );
            }else if( strcmp( $line, "" ) == 0 ){
                $current = "";
            }else if( strpos( $line, "#" ) === 0 ){
                continue;
            }else{
                if( strcmp( $current, "msgid" ) == 0 ){
                    $msgid = $msgid . $this->trimQuote( $line );
                }else if( strcmp( $current, "msgstr" ) == 0 ){
                    $msgstr = $msgstr . $this->trimQuote( $line );
                }
            }
        }
        fclose( $f );
    }

    public function getMessage( $message ){
        $s = $this->dict[$message];
        if( is_null( $s ) ){
            return $message;
        }else{
            return $s;
        }
    }

    function trimQuote( $str ){
        if( is_null( $str ) ){
            return "";
        }
        if( strcmp( $str, '"' ) == 0 ){
            return "";
        }
        if( strcmp( $str, '""' ) == 0 ){
            return "";
        }
        if( strcmp( substr( $str, 0, 1 ), '"' ) == 0 ){
            $str = substr( $str, 1 );
        }
        if( strcmp( substr( $str, strlen( $str ) - 1, 1 ), '"') == 0 ){
            $str = substr( $str, 0, strlen( $str ) - 1 );
        }
        return $str;
    }
}

class Messaging{
    static $dict = array();
    static $current_lang = "en";

    static public function init( $po_dir ){
        $d = opendir( $po_dir );
        while( false !== ($f = readdir( $d )) ){
            $len = strlen( $f );
            if( $len <= 3 ){
                continue;
            }
            $indx = strripos( $f, ".po" );
            if( $indx == $len - 3 ){
                $l = substr( $f, 0, $len - 3 );
                $po = new POFile();
                $po->init( $po_dir . $f );
                self::$dict[$l] = $po;
            }
        }
        closedir( $d );
    }

    static public function gettext( $message ){
        $d = self::$dict[self::$current_lang];
        if( is_null( $d ) ){
            return $message;
        }else{
            $ret = self::$dict[self::$current_lang]->getMessage( $message );
            if( strcmp( $ret, "" ) == 0 ){
                return $message;
            }else{
                return $ret;
            }
        }
    }

    static public function setLanguage( $language ){
        self::$current_lang = $language;
    }

    static public function getLanguage(){
        return self::$current_lang;
    }
}

Messaging::init( $argv[2] );
Messaging::setLanguage( $argv[1] );

function msg( $message ){
    return Messaging::gettext( $message );
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<META HTTP-EQUIV="Content-type" CONTENT="text/html; charset=UTF-8">
<title>Cadencii version 3.2 <?php print msg( "Manual" ) ?></title>
<link rel="stylesheet" type="text/css" href="style.css">
</head>
<body>
  <h2>Cadencii verion 3.2 <?php print msg( "Manual" ) ?></h2>

  <!-- section 0 -->
  <h3>0. <?php print msg( "Table of contents" ) ?></h3>
  <div class=indent>
    <ol>
      <li><a href="#preface"><?php print msg( "Preface" ) ?></a>
      <li><a href="#platform"><?php print msg( "Platform" ) ?></a>
      <li><a href="#how_to_install"><?php print msg( "How to install" ) ?></a>
      <li><a href="#how_to_use"><?php print msg( "How to use" ) ?></a>
      <li><a href="#faq"><?php print msg( "FAQ" ) ?></a>
      <li><a href="#license"><?php print msg( "License" ) ?></a>
    </ol>
  </div>

  <!-- section 1 -->
  <a name="preface"></a>
  <h3>1. <?php print msg( "Preface" ) ?></h3>
  <p class=indent>
    <?php print msg( "Cadenncii is a free software for editing scores for several singing synthesis systems." ) . "\n" ?>
    <?php
      $url_vocaloid = "http://www.vocaloid.com/en/index.html";
      $url_vocaloid2 = "http://www.vocaloid.com/index.en.html";
      $url_utau = "http://utau2008.web.fc2.com/";
      $url_aquestone = "http://www.a-quest.com/products/aquestone_en.html";
      $lang = Messaging::getLanguage();
      if( strcmp( $lang, "ja" ) == 0 ){
          $url_vocaloid = "http://www.vocaloid.com/jp/index.html";
          $url_vocaloid2 = "http://www.vocaloid.com/";
          $url_aquestone = "http://www.a-quest.com/products/aquestone.html";
      }
      print sprintf( msg( "At this time, <a href=\"%s\">VOCALOID</a>, <a href=\"%s\">VOCALOID2</a>, <a href=\"%s\">UTAU</a>, STRAIGHT with UTAU, and <a href=\"%s\">AquesTone</a> are available as synthesizer." ), $url_vocaloid, $url_vocaloid2, $url_utau, $url_aquestone );
    ?>
  </p>
  <p class=indent>
    Screen shot<br />
    <img src="img/screen_shot.png" alt="<?php print msg( "screen shot of Cadencii" ) ?>" />
  </p>

  <!-- section 2 -->
  <a name="platform"></a>
  <h3>2. <?php print msg( "Platform" ) ?></h3>
  <p class=indent>
    <?php print msg( "Operating System" ) ?>: Windows 2000, Windows XP, Windows Vista, Windows 7<br />
    <?php print msg( "with .NET Framework 2.0 or later" ) ?>
  </p>

  <!-- section 3 -->
  <a name="how_to_install"></a>
  <h3>3. <?php print msg( "Install" ) ?></h3>
  <div class=indent>
    <h4>case A: Windows Vista, Windows 7</h4>
      <div class=indent>
        <ol>
          <li>Check .NET Framework is enabled or not.
          <li>Install Visual C++ Runtime(x86)
          <li>Unzip "Cadencii_v3.2.*.zip". (* is a number, means maintenance release number)
        </ol>
      </div>
    <h4>case B: Windows XP</h4>
    <h4>case C: Windows 2000</h4>
  </div>

  <!-- section 4 -->
  <a name="how_to_use"></a>
  <h3>4. <?php print msg( "How to use" ) ?></h3>

  <!-- section 5 -->
  <a name="faq"></a>
  <h3>5. <?php print msg( "FAQ" ) ?></h3>

  <!-- section 6 -->
  <a name="license"></a>
  <h3>6. <?php print msg( "License" ) ?></h3>
</body>
</html>
