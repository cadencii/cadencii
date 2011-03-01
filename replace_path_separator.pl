my $ENABLE_DEBUG = 0;
my $ENABLE_PROPERTY = 1;
my $ENABLE_VOCALOID = 1;
my $ENABLE_AQUESTONE = 0;
my $ENABLE_MIDI = 1;
my $WINE_VERSION = "1.1.2";

for( my $i = 0; $i <= $#ARGV; $i++ ){
    my $arg = $ARGV[$i];
    if( $arg eq "--enable-debug" ){
        $ENABLE_DEBUG = 1;
    }
    if( $arg eq "--disable-debug" ){
        $ENABLE_DEBUG = 0;
    }
    if( $arg eq "--enable-property" ){
        $ENABLE_PROPERTY = 1;
    }
    if( $arg eq "--disable-property" ){
        $ENABLE_PROPERTY = 0;
    }
    if( $arg eq "--enable-vocaloid" ){
        $ENABLE_VOCALOID = 1;
    }
    if( $arg eq "--disable-vocaloid" ){
        $ENABLE_VOCALOID = 0;
    }
    if( $arg eq "--enable-aquestone" ){
        $ENABLE_AQUESTONE = 1;
    }
    if( $arg eq "--disable-aquestone" ){
        $ENABLE_AQUESTONE = 0;
    }
    if( $arg eq "--enable-midi" ){
        $ENABLE_MIDI = 1;
    }
    if( $arg eq "--disable-midi" ){
        $ENABLE_MIDI = 0;
    }
    my $search = "--wine-version=";
    if( index( $arg, $search ) == 0 ){
        $WINE_VERSION = substr( $arg, length( $search ) );
    }
}

open( FILE, "<Makefile.include" );
open( OUT, ">Makefile" );

@special_dependencies = (
"./BuildJavaUI/src/org/kbinani/ByRef.java",
"./BuildJavaUI/src/org/kbinani/math.java",
"./BuildJavaUI/src/org/kbinani/PortUtil.java",
"./BuildJavaUI/src/org/kbinani/str.java",
"./BuildJavaUI/src/org/kbinani/fsys.java",
"./BuildJavaUI/src/org/kbinani/vec.java",
"./BuildJavaUI/src/org/kbinani/cadencii/IconParader.java",
"./BuildJavaUI/src/org/kbinani/cadencii/NumberTextBox.java",
"./BuildJavaUI/src/org/kbinani/cadencii/NumericUpDownEx.java",
);

@ignore = (
"./org.kbinani/Arrays.cs",
"./org.kbinani/awt.cs",
"./org.kbinani/awt.event.cs",
"./org.kbinani/awt.geom.cs",
"./org.kbinani/awt.image.cs",
"./org.kbinani/Collections.cs",
"./org.kbinani/imageio.cs",
"./org.kbinani/io.cs",
"./org.kbinani/Iterator.cs",
"./org.kbinani/lang.cs",
"./org.kbinani/ListIterator.cs",
"./org.kbinani/RandomAccessFile.cs",
"./org.kbinani/swing.cs",
"./org.kbinani/util.cs",
"./org.kbinani/Vector.cs",
);

if( $ARGV[0] eq "MSWin32" ){
    $djava_mac = "";
    $to_devnull = "";
}else{
    $djava_mac = "-DJAVA_MAC";
    $to_devnull = "2>/dev/null";
}
if( $ENABLE_DEBUG == 0 ){
    $ddebug = "";
}else{
    $ddebug = "-DDEBUG";
}
if( $ENABLE_PROPERTY == 0 ){
    $denable_property = "";
}else{
    $denable_property = "-DENABLE_PROPERTY";
}
if( $ENABLE_VOCALOID == 0 ){
    $denable_vocaloid = "";
}else{
    $denable_vocaloid = "-DENABLE_VOCALOID";
}
if( $ENABLE_AQUESTONE == 0 ){
    $denable_aquestone = "";
}else{
    $denable_aquestone = "-DENABLE_AQUESTONE";
}
if( $ENABLE_MIDI == 0 ){
    $denable_midi = "";
}else{
    $denable_midi = "-DENABLE_MIDI";
}

open( CFG, ">./Cadencii/Config.cs" );
print CFG <<__EOD__;
\#if JAVA

package org.kbinani.cadencii;

import java.util.*;

\#else

using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii
{

\#endif

    public class Config
    {
        private static TreeMap<String, Boolean> mDirectives = new TreeMap<String, Boolean>();

\#if JAVA
        static
\#else
        static Config()
\#endif
        {
__EOD__

%directives;
$directives{"debug"} = $ENABLE_DEBUG ? "true" : "false";
$directives{"property"} = $ENABLE_PROPERTY ? "true" : "false";
$directives{"vocaloid"} = $ENABLE_VOCALOID ? "true" : "false";
$directives{"aquestone"} = $ENABLE_AQUESTONE ? "true" : "false";
$directives{"midi"} = $ENABLE_MIDI ? "true" : "false";
foreach $key ( keys %directives )
{
    print CFG "            mDirectives.put( \"$key\", $directives{$key} );\n";
    print "$key: $directives{$key}\n";
}
print "WINE_VERSION: $WINE_VERSION\n";

print CFG <<__EOD__;
        }

        public static String getWineVersion()
        {
            return "$WINE_VERSION";
        }

        public static TreeMap<String, Boolean> getDirectives()
        {
            TreeMap<String, Boolean> ret = new TreeMap<String, Boolean>();
            for( Iterator<String> itr = mDirectives.keySet().iterator(); itr.hasNext(); ){
                String key = itr.next();
                ret.put( key, mDirectives.get( key ) );
            }
            return ret;
        }

    }
\#if !JAVA
}
\#endif
__EOD__
close( CFG );

&getSrcList( "./org.kbinani", "./build/java/org/kbinani/", $src_corlib, $cp_corlib, $dep_corlib );
&getSrcList( "./org.kbinani.apputil", "./build/java/org/kbinani/apputil/", $src_apputil, $cp_apputil, $dep_apputil );
&getSrcList( "./org.kbinani.componentmodel", "./build/java/org/kbinani/componentmodel/", $src_componentmodel, $cp_componentmodel, $dep_componentmodel );
&getSrcList( "./org.kbinani.media", "./build/java/org/kbinani/media/", $src_media, $cp_media, $dep_media );
&getSrcList( "./org.kbinani.vsq", "./build/java/org/kbinani/vsq/", $src_vsq, $cp_vsq, $dep_vsq );
&getSrcList( "./org.kbinani.windows.forms", "./build/java/org/kbinani/windows/forms/", $src_winforms, $cp_winforms, $dep_winforms );
&getSrcList( "./org.kbinani.xml", "./build/java/org/kbinani/xml/", $src_xml, $cp_xml, $dep_xml );
&getSrcList( "./Cadencii", "./build/java/org/kbinani/cadencii/", $src_cadencii, $cp_cadencii, $dep_cadencii );

while( $line = <FILE> ){
    $line =~ s/\@SRC_JAPPUTIL\@/$src_apputil/g;
    $line =~ s/\@SRC_JCORLIB\@/$src_corlib/g;
    $line =~ s/\@SRC_JWINFORMS\@/$src_winforms/g;
    $line =~ s/\@SRC_JMEDIA\@/$src_media/g;
    $line =~ s/\@SRC_JVSQ\@/$src_vsq/g;
    $line =~ s/\@SRC_JCADENCII\@/$src_cadencii/g;
    $line =~ s/\@SRC_JCOMPONENTMODEL\@/$src_componentmodel/g;
    $line =~ s/\@SRC_JXML\@/$src_xml/g;

    $line =~ s/\@DEP_JAPPUTIL\@/$dep_apputil/g;
    $line =~ s/\@DEP_JCORLIB\@/$dep_corlib/g;
    $line =~ s/\@DEP_JWINFORMS\@/$dep_winforms/g;
    $line =~ s/\@DEP_JMEDIA\@/$dep_media/g;
    $line =~ s/\@DEP_JVSQ\@/$dep_vsq/g;
    $line =~ s/\@DEP_JCADENCII\@/$dep_cadencii/g;
    $line =~ s/\@DEP_JCOMPONENTMODEL\@/$dep_componentmodel/g;
    $line =~ s/\@DEP_JXML\@/$dep_xml/g;
    $line =~ s/\@DJAVA_MAC\@/$djava_mac/g;
    $line =~ s/\@DDEBUG\@/$ddebug/g;
    $line =~ s/\@DENABLE_PROPERTY\@/$denable_property/g;
    $line =~ s/\@DENABLE_VOCALOID\@/$denable_vocaloid/g;
    $line =~ s/\@DENABLE_AQUESTONE\@/$denable_aquestone/g;
    $line =~ s/\@TO_DEVNULL\@/$to_devnull/g;
    $line =~ s/\@WINE_VERSION\@/$WINE_VERSION/g;
    $line =~ s/\@DENABLE_MIDI\@/$denable_midi/g;

    #if( $ARGV[0] eq "MSWin32" ){
    #    if( ($line =~ /\$\(CP\)/) | ($line =~ /\$\(RM\)/) | ($line =~ /\$\(MKDIR\)/) ){
    #        $line =~ s/\//\\/g;
    #    }
    #    $line =~ s/\@CP\@/copy/g;
    #    $line =~ s/\@RM\@/del/g;
    #    $line =~ s/\@TARGET\@/.\\build\\win/g;
    #    $line =~ s/\@MKDIR\@/perl safe_mkdir\.pl/g;
    #    $line =~ s/\@PLAY_SOUND_DLL\@/\$\(TARGET\)\\PlaySound\.dll/g;
    #    $line =~ s/\@MONO\@//g;
    #}else{
        $line =~ s/\@CP\@/cp/g;
        $line =~ s/\@RM\@/rm/g;
        $line =~ s/\@TARGET\@/\.\/build\/win/g;
        $line =~ s/\@MKDIR\@/perl safe_mkdir\.pl/g;
        $line =~ s/\@PLAY_SOUND_DLL\@//g;
        $line =~ s/\@MONO\@/mono /g;
    #}
    print OUT $line;
}

close( FILE );
close( OUT );

sub getSrcList
{
    my $dir = $_[0];
    my $prefix = $_[1];
    my $DIR;
    opendir( DIR, $dir );
    my @file = readdir( DIR );
    closedir( DIR );
    my @src = ();
    my @srcall = ();
    foreach $v ( @file ){
        if( length( $v ) <= 3 ){
            next;
        }
        if( rindex( $v, ".cs" ) == length( $v ) - 3 ){
            my $s1 = substr( $v, 0, length( $v ) - 3 );
            my $search = $dir . "/" . $s1 . ".java";
            my $found = 0;
            foreach my $s ( @special_dependencies ){
                if( $s eq $search ){
                    $found = 1;
                    last;
                }
            }
            if( $s1 eq "Resources" ){
                $found = 1;
            }
            if( $found == 0 ){
                push( @src, $s1 );
            }
            push( @srcall, $s1 );
        }
    }
    $_[2] = ""; #src
    $_[3] = ""; #cp
    $_[4] = ""; #dep
    my $count = @srcall;
    for( $i = 0; $i < $count; $i++ ){
        my $cname = $srcall[$i];
        my $s = $cname . ".java";
        if( $i == 0 ){
            $_[2] = $prefix . $s;
        }else{
            $_[2] = $_[2] . " \\" . "\n        " . $prefix . $s;
        }
    }

    # check BuildJavaUI
    $build_java_ui_prefix = "";
    if( index( $dir, "./Cadencii" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/cadencii/";
    }elsif( index( $dir, "./org.kbinani.windows.forms" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/windows/forms/";
    }elsif( index( $dir, "./org.kbinani.xml" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/xml/";
    }elsif( index( $dir, "./org.kbinani.vsq" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/vsq/";
    }elsif( index( $dir, "./org.kbinani.media" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/media/";
    }elsif( index( $dir, "./org.kbinani.apputil" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/apputil/";
    }elsif( index( $dir, "./org.kbinani.componentmodel" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/componentmodel/";
    }elsif( index( $dir, "./org.kbinani" ) == 0 ){
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/";
    }

    $count = @src;
    for( $i = 0; $i < $count; $i++ ){
        my $cname = $src[$i];
        my $s = $cname . ".java";
        $_[3] = $_[3] . "$prefix$s:$dir/$s\n\t\$(CP) $dir/$s $prefix$s\n";
        $_[4] .= "$prefix$cname.java: $dir/$cname.cs";
        $add_to = 1;
        my $c = @special_dependencies;
        for( my $j = 0; $j < $c; $j++ ){
            if( "$build_java_ui_prefix$cname.java" eq $special_dependencies[$j] ){
                $add_to = 0;
                last;
            }
        }
        if( (-e "$build_java_ui_prefix$cname.java") && $add_to == 1 ){
            $_[4] .= " $build_java_ui_prefix$cname.java\n";
        }else{
            $_[4] .= "\n";
        }
        $_[4] .= "\tjava -jar pp_cs2java.jar \$(PPCS2JAVA_OPT) -i $dir/$cname.cs -o $prefix$cname.java\n\n";
    }
}
