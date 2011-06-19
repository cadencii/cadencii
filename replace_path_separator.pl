my %directive = (
    "debug"     => 0,
    "property"  => 1,
    "vocaloid"  => 1,
    "aquestone" => 0,
    "midi"      => 1,
    "script"    => 0,
);
my $WINE_VERSION = "1.1.2";

for( my $i = 0; $i <= $#ARGV; $i++ ){
    my $arg = $ARGV[$i];
    foreach my $key ( keys %directive ){
        my $search_enable = "--enable-" . $key;
        if( $arg eq $search_enable ){
            $directive{$key} = 1;
        }
        my $search_disable = "--disable-" . $key;
        if( $arg eq $search ){
            $directive{$key} = 0;
        }
    }
    my $search = "--wine-version=";
    if( index( $arg, $search ) == 0 ){
        $WINE_VERSION = substr( $arg, length( $search ) );
    }
}

open( FILE, "<Makefile.include" );
open( OUT, ">Makefile" );

my @special_dependencies = (
    "./BuildJavaUI/src/org/kbinani/ByRef.java",
    "./BuildJavaUI/src/org/kbinani/math.java",
    "./BuildJavaUI/src/org/kbinani/PortUtil.java",
    "./BuildJavaUI/src/org/kbinani/str.java",
    "./BuildJavaUI/src/org/kbinani/fsys.java",
    "./BuildJavaUI/src/org/kbinani/vec.java",
    "./BuildJavaUI/src/org/kbinani/cadencii/IconParader.java",
    "./BuildJavaUI/src/org/kbinani/cadencii/NumberTextBox.java",
    "./BuildJavaUI/src/org/kbinani/cadencii/NumericUpDownEx.java",
    "./BuildJavaUI/src/org/kbinani/cadencii/TrackSelectorSingerPopupMenu.java",
);

my @ignore = (
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

my $djava_mac = "-DJAVA_MAC";
my $to_devnull = "2>/dev/null";
my $cp = "cp";
my $rm = "rm";
my $plaf = $^O;
if( $plaf eq "MSWin32" ){
    $djava_mac = "";
    $to_devnull = "";
    $cp = "copy";
    $rm = "rm";
}elsif( $plaf eq "linux" ){
    $djava_mac = "";
}
print "platform: $plaf\n";

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

foreach $key ( keys %directive )
{
    my $tbool = $directive{$key} == 0 ? "false" : "true";
    print CFG "            mDirectives.put( \"$key\", $tbool );\n";
    print "$key: $tbool\n";
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

&getSrcList( "./org.kbinani", "./build/java/org/kbinani/", $src_java_core, $dep_java_core );
&getSrcList( "./org.kbinani.apputil", "./build/java/org/kbinani/apputil/", $src_java_apputil, $dep_java_apputil );
&getSrcList( "./org.kbinani.componentmodel", "./build/java/org/kbinani/componentmodel/", $src_java_componentmodel, $dep_java_componentmodel );
&getSrcList( "./org.kbinani.media", "./build/java/org/kbinani/media/", $src_java_media, $dep_java_media );
&getSrcList( "./org.kbinani.vsq", "./build/java/org/kbinani/vsq/", $src_java_vsq, $dep_java_vsq );
&getSrcList( "./org.kbinani.windows.forms", "./build/java/org/kbinani/windows/forms/", $src_java_winforms, $dep_java_winforms );
&getSrcList( "./org.kbinani.xml", "./build/java/org/kbinani/xml/", $src_java_xml, $dep_java_xml );
&getSrcList( "./Cadencii", "./build/java/org/kbinani/cadencii/", $src_java_cadencii, $dep_java_cadencii, $post_process_java_cadencii );
$post_process_java .= $post_process_java_cadencii;

&getSrcListCpp( "./org.kbinani", "./org.kbinani/", $src_cpp_core, $dep_cpp_core );
&getSrcListCpp( "./org.kbinani.vsq", "./org.kbinani.vsq/", $src_cpp_vsq, $dep_cpp_vsq );

while( $line = <FILE> ){
    $line =~ s/\@SRC_JAPPUTIL\@/$src_java_apputil/g;
    $line =~ s/\@SRC_JCORLIB\@/$src_java_core/g;
    $line =~ s/\@SRC_JWINFORMS\@/$src_java_winforms/g;
    $line =~ s/\@SRC_JMEDIA\@/$src_java_media/g;
    $line =~ s/\@SRC_JVSQ\@/$src_java_vsq/g;
    $line =~ s/\@SRC_JCADENCII\@/$src_java_cadencii/g;
    $line =~ s/\@SRC_JCOMPONENTMODEL\@/$src_java_componentmodel/g;
    $line =~ s/\@SRC_JXML\@/$src_java_xml/g;
    $line =~ s/\@SRC_CPP_CORE\@/$src_cpp_core/g;
    $line =~ s/\@SRC_CPP_VSQ\@/$src_cpp_vsq/g;

    $line =~ s/\@DEP_JAPPUTIL\@/$dep_java_apputil/g;
    $line =~ s/\@DEP_JCORLIB\@/$dep_java_core/g;
    $line =~ s/\@DEP_JWINFORMS\@/$dep_java_winforms/g;
    $line =~ s/\@DEP_JMEDIA\@/$dep_java_media/g;
    $line =~ s/\@DEP_JVSQ\@/$dep_java_vsq/g;
    $line =~ s/\@DEP_JCADENCII\@/$dep_java_cadencii/g;
    $line =~ s/\@DEP_JCOMPONENTMODEL\@/$dep_java_componentmodel/g;
    $line =~ s/\@DEP_JXML\@/$dep_java_xml/g;
    $line =~ s/\@DJAVA_MAC\@/$djava_mac/g;
    $line =~ s/\@DEP_CPP_CORE\@/$dep_cpp_core/g;
    $line =~ s/\@DEP_CPP_VSQ\@/$dep_cpp_vsq/g;

    $line =~ s/\@POST_PROCESS_JAVA\@/$post_process_java/g;

    foreach $key ( keys %directive ){
        my $search = "\@DENABLE_" . (uc $key) . "\@";
        my $rep_draft = "-DENABLE_" . (uc $key);
        if( $key eq "debug" ){
            $rep_draft = "-DDEBUG";
        }
        my $rep = $directive{$key} == 0 ? "" : $rep_draft;
        $line =~ s/$search/$rep/g;
    }
    $line =~ s/\@TO_DEVNULL\@/$to_devnull/g;
    $line =~ s/\@WINE_VERSION\@/$WINE_VERSION/g;

    if( $ARGV[0] eq "MSWin32" ){
        if( ($line =~ /\$\(CP\)/) | ($line =~ /\$\(RM\)/) | ($line =~ /\$\(MKDIR\)/) ){
            $line =~ s/\//\\/g;
        }
    }

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
        $line =~ s/\@CP\@/$cp/g;
        $line =~ s/\@RM\@/$rm/g;
        $line =~ s/\@TARGET\@/\.\/build\/win/g;
        $line =~ s/\@MKDIR\@/perl safe_mkdir\.pl/g;
        $line =~ s/\@PLAY_SOUND_DLL\@//g;
        $line =~ s/\@MONO\@/mono /g;
    #}
    print OUT $line;
}

close( FILE );
close( OUT );

##
# @param  string  search path
# @param  string  prefix of file-name for copy
# @param  string  list of converted sources
# @param  string  definition of dependencies: .cs -> .h
# @return  void
#
sub getSrcListCpp
{
    my $DIR;
    my $search_path = $_[0];
    my $prefix = $_[1];
    my @files = ();

    # search all files in specified search path
    #opendir( DIR, $search_path );
    #my @files = readdir( DIR );
    #closedir( DIR );
    if( $search_path eq "./org.kbinani" )
    {
        @files = ( "vec.cs", "str.cs", "dic.cs", "sout.cs", "serr.cs" );
    }
    elsif( $search_path eq "./org.kbinani.vsq" )
    {
        @files = ( "Lyric.cs", "BPPair.cs", "IconHandle.cs", "LyricHandle.cs", "VsqHandleType.cs" );
    }

    # check file names
    $_[2] = "";
    $_[3] = "";
    my $count = 0;
    foreach my $name ( @files )
    {
        # skip file with name shorter than ".cs"
        if( length( $name ) <= 3 )
        {
            next;
        }

        # skip non .cs files
        if( rindex( $name, ".cs" ) != length( $name ) - 3 )
        {
            next;
        }

        # name without extension
        my $cname = substr( $name, 0, length( $name ) - 3 );

        # concatenate source files
        if( $count > 0 )
        {
            $_[2] .= " \\" . "\n        ";
        }
        $_[2] .= $prefix . $cname . ".h";

        # concatenate dependency definitions
        $_[3] .= $search_path . "/" . $cname . ".h: " . $prefix . $name . "\n";
        $_[3] .= "\tjava -jar pp_cs2java.jar \$(PPCS2CPP_OPT) -i $search_path/$name -o $prefix$cname.h\n";
        $count++;
    }
}

##
# @param string [in] ファイルを検索するディレクトリのパス
# @param string [in] ファイル名の先頭に付ける接頭句
# @param string [out] プリプロセッサで変換元となるファイルのリスト
# @param string [out] プリプロセッサにより変換するファイルの依存関係を定義したリスト
# @param string [out] BuildJavaUIへのコピー操作を定義した文字列
#
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

    # 拡張子が.csでないファイルを除外する
    my $changed = 1;
    while( $changed )
    {
        $changed = 0;
        for( my $i = 0; $i < @file; $i++ )
        {
            my $v = $file[$i];
            my $ext = ".cs";
            if( length( $v ) <= length( $ext ) )
            {
                splice @file, $i, 1;
                $changed = 1;
                last;
            }
            
            if( rindex( $v, $ext ) != length( $v ) - length( $ext ) )
            {
                splice @file, $i, 1;
                $changed = 1;
                last;
            }
        }
    }

    # ファイル名が以下の規則に当てはまっているものを抽出する
    # @view_implには，*の部分が重複無く入るようにする
    # *UiImpl.cs
    # *.cs
    # *Ui.cs
    # *Controller.cs
    # *UiListener.cs
    my @view_impl = ();
    my @suffix_rule = ( "UiImpl.cs", "Ui.cs", "Controller.cs", "UiListener.cs" );
    {
        # rule_map["FormFoo"] = "(UiImpl.cs)(Ui.cs)";
        my %rule_map = ();

        foreach my $name ( @file )
        {
            foreach my $rule ( @suffix_rule )
            {
                my $indx = rindex( $name, $rule );
                if( $indx < 0 ){
                    next;
                }
                if( $indx != length( $name ) - length( $rule ) )
                {
                    next;
                }
                my $prefix_name = substr( $name, 0, length( $name ) - length( $rule ) );
                $rule_map{$prefix_name} .= "(" . $rule . ")";
            }
        }

        # suffix_rulesの全てが付いているファイルは，V-C分離済みの画面実装とみなす
        foreach my $key ( keys( %rule_map ) )
        {
            my $value = $rule_map{$key};
            my $contains_all_rules = 1;
            foreach my $rule ( @suffix_rule )
            {
                if( index( $value, "(" . $rule . ")" ) < 0 )
                {
                    $contains_all_rules = 0;
                    last;
                }
            }
            
            # suffix_rulesが全て含まれているので，チェックリストに追加するよ
            if( $contains_all_rules )
            {
                push( @view_impl, $key );
            }
        }
    }

    foreach my $v ( @file )
    {
        if( length( $v ) <= 3 )
        {
            next;
        }
        if( rindex( $v, ".cs" ) != length( $v ) - 3 )
        {
            next;
        }
        my $s1 = substr( $v, 0, length( $v ) - 3 );
        my $search = $dir . "/" . $s1 . ".java";
        my $found = 0;
        foreach my $s ( @special_dependencies )
        {
            if( $s eq $search )
            {
                $found = 1;
                last;
            }
        }
        if( $s1 eq "Resources" )
        {
            $found = 1;
        }
        
        if( index( $v, "Form" ) == 0 && rindex( $v, "Impl.cs" ) == length( $v ) - length( "Impl.cs" ) )
        {
            $found = 2;
        }
        
        if( $found == 0 )
        {
            push( @src, $s1 );
        }
        if( $found != 2 )
        {
            push( @srcall, $s1 );
        }
    }
    $_[2] = ""; #src
    $_[3] = ""; #dep
    $_[4] = ""; # UIのインターフェース定義をBuildJavaUIにコピーするお
    my $count = @srcall;
    for( my $i = 0; $i < $count; $i++ )
    {
        my $cname = $srcall[$i];
        my $s = $cname . ".java";
        if( $i == 0 )
        {
            $_[2] = $prefix . $s;
        }
        else
        {
            $_[2] = $_[2] . " \\" . "\n        " . $prefix . $s;
        }
    }

    # check BuildJavaUI
    $build_java_ui_prefix = "";
    if( index( $dir, "./Cadencii" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/cadencii/";
    }
    elsif( index( $dir, "./org.kbinani.windows.forms" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/windows/forms/";
    }
    elsif( index( $dir, "./org.kbinani.xml" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/xml/";
    }
    elsif( index( $dir, "./org.kbinani.vsq" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/vsq/";
    }
    elsif( index( $dir, "./org.kbinani.media" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/media/";
    }
    elsif( index( $dir, "./org.kbinani.apputil" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/apputil/";
    }
    elsif( index( $dir, "./org.kbinani.componentmodel" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/componentmodel/";
    }
    elsif( index( $dir, "./org.kbinani" ) == 0 )
    {
        $build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/";
    }

    $count = @src;
    for( my $i = 0; $i < $count; $i++ )
    {
        my $cname = $src[$i];
        my $s = $cname . ".java";
        $_[3] .= "$prefix$cname.java: $dir/$cname.cs";
        $add_to = 1;
        my $c = @special_dependencies;
        for( my $j = 0; $j < $c; $j++ )
        {
            if( "$build_java_ui_prefix$cname.java" eq $special_dependencies[$j] )
            {
                $add_to = 0;
                last;
            }
        }
        if( (-e "$build_java_ui_prefix$cname.java") && $add_to == 1 )
        {
            $_[3] .= " $build_java_ui_prefix$cname.java\n";
        }
        else
        {
            $_[3] .= "\n";
        }
        $_[3] .= "\tjava -jar pp_cs2java.jar \$(PPCS2JAVA_OPT) -i $dir/$cname.cs -o $prefix$cname.java\n\n";
    }

    # UI実装に必要なインターフェース定義ファイルをコピー
    foreach my $name ( @view_impl )
    {
#	$(CP) ./build/java/org/kbinani/cadencii/FormAskKeySoundGenerationController.java    ./BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGenerationController.java
#	$(CP) ./build/java/org/kbinani/cadencii/FormAskKeySoundGenerationUi.java            ./BuildJavaUI/src/org/kbinani/cadencii/FormAskKeySoundGenerationUi.java
        #$build_java_ui_prefix = "./BuildJavaUI/src/org/kbinani/componentmodel/";
        my $com_dirname_part = substr( $build_java_ui_prefix, length( "./BuildJavaUI/src/" ) );

        $_[4] .= "\t\$(CP) ./build/java/" . $com_dirname_part . $name . "Controller.java  ./BuildJavaUI/src/" . $com_dirname_part . $name . "Controller.java\n";
        $_[4] .= "\t\$(CP) ./build/java/" . $com_dirname_part . $name . "Ui.java          ./BuildJavaUI/src/" . $com_dirname_part . $name . "Ui.java\n";
    }
}
