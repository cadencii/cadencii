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

package com.github.cadencii;

import java.util.*;

\#else

using System;
using com.github.cadencii.java.util;

namespace com.github.cadencii
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

my $java_post_process = "";
&getSrcList( "./org.kbinani", "./build/java/com/github/cadencii/", $src_java_core, $dep_java_core );
&getSrcList( "./org.kbinani.apputil", "./build/java/com/github/cadencii/apputil/", $src_java_apputil, $dep_java_apputil );
&getSrcList( "./org.kbinani.componentmodel", "./build/java/com/github/cadencii/componentmodel/", $src_java_componentmodel, $dep_java_componentmodel );
&getSrcList( "./org.kbinani.media", "./build/java/com/github/cadencii/media/", $src_java_media, $dep_java_media );
&getSrcList( "./org.kbinani.vsq", "./build/java/com/github/cadencii/vsq/", $src_java_vsq, $dep_java_vsq );
&getSrcList( "./org.kbinani.windows.forms", "./build/java/com/github/cadencii/windows/forms/", $src_java_winforms, $dep_java_winforms );
&getSrcList( "./org.kbinani.xml", "./build/java/com/github/cadencii/xml/", $src_java_xml, $dep_java_xml );
&getSrcList( "./Cadencii", "./build/java/com/github/cadencii/", $src_java_cadencii, $dep_java_cadencii, $post_process_java_cadencii );
$java_post_process .= $post_process_java_cadencii;

my $cpp_post_process = "";
&getSrcListCpp( "./org.kbinani", "./build/cpp/org/kbinani/", $src_cpp_core, $dep_cpp_core );
&getSrcListCpp( "./org.kbinani.vsq", "./build/cpp/org/kbinani/vsq/", $src_cpp_vsq, $dep_cpp_vsq );
&getSrcListCpp( "./Cadencii", "./build/cpp/org/kbinani/cadencii/", $src_cpp_cadencii, $dep_cpp_cadencii, $post_process_cpp_cadencii );
$cpp_post_process .= $post_process_cpp_cadencii;

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
    $line =~ s/\@SRC_CPP_CADENCII\@/$src_cpp_cadencii/g;

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
    $line =~ s/\@DEP_CPP_CADENCII\@/$dep_cpp_cadencii/g;

    $line =~ s/\@JAVA_POST_PROCESS\@/$java_post_process/g;
    $line =~ s/\@CPP_POST_PROCESS\@/$cpp_post_process/g;

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
# ファイルリストのうち，拡張子が.csであるファイルのみを抽出します．
# @param array [in] ファイル名のリスト
# @return array 抽出したファイルのリスト
#
sub extractCsFiles
{
    # 拡張子が.csでないファイルを除外する
    my $changed = 1;
    local( @file ) = @_;
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
    @file;
}

##
# ファイル一覧から，V-C分離が行われているファイルを抽出します．
# @param array [in] ファイルのリスト
# @return array 抽出結果
#
sub extractViewImplementation
{
    # ファイル名が以下の規則に当てはまっているものを抽出する
    # @view_implには，*の部分が重複無く入るようにする
    # *UiImpl.cs
    # *.cs
    # *Ui.cs
    # *Controller.cs
    # *UiListener.cs
    local( @file ) = @_;
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

    @view_impl;
}

##
# @param  string  search path
# @param  string  prefix of file-name for copy
# @param  string  list of converted sources
# @param  string  definition of dependencies: .cs -> .h
# @param  string  Makefileのcpp_post_process用のコマンド文字列
# @return  void
#
sub getSrcListCpp
{
    my $DIR;
    my $search_path = $_[0];
    my $prefix = $_[1];
    my @files = ();

    # search all files in specified search path
    opendir( DIR, $search_path );
    my @files = readdir( DIR );
    closedir( DIR );
#    if( $search_path eq "./org.kbinani" )
#    {
#        @files = ( "vec.cs", "str.cs", "dic.cs", "sout.cs", "serr.cs" );
#    }
#    elsif( $search_path eq "./org.kbinani.vsq" )
#    {
#        @files = ( "Lyric.cs", "BPPair.cs", "IconHandle.cs", "LyricHandle.cs", "VsqHandleType.cs" );
#    }
#    elsif( $search_path eq "./Cadencii" )
#    {
#        @files = ( "FormAskKeySoundGenerationController.cs", "FormAskKeySoundGenerationUi.cs", "FormAskKeySoundGenerationUiListener.cs", "FormAskKeySoundGenerationUiImpl.cs" );
#    }

    @files = &extractCsFiles( @files );
    my @view_impl = &extractViewImplementation( @files );

    # check file names
    $_[2] = "";
    $_[3] = "";
    my $count = 0;
    foreach my $name ( @files )
    {
        # name without extension
        my $cname = substr( $name, 0, length( $name ) - 3 );

        # concatenate source files
        if( $count > 0 )
        {
            $_[2] .= " \\" . "\n        ";
        }
        $_[2] .= $prefix . $cname . ".h";

        # concatenate dependency definitions
        $_[3] .= $prefix . $cname . ".h: " . $search_path . "/" . $name . "\n";
        $_[3] .= "\tjava -jar pp_cs2java.jar \$(PPCS2CPP_OPT) -i $search_path/$name -o $prefix$cname.h\n";
        $count++;
    }

    $_[4] = "";
    $com_dirname_part = &getDirectoryForPackageName( $search_path );
    foreach my $name ( @view_impl )
    {
        # build/cpp => buildQtUI
        $_[4] .= "\t\$(CP) build/cpp/" . $com_dirname_part . "/" . $name . "UiListener.h   buildQtUI/" . $com_dirname_part . "/" . $name . "UiListener.h\n";
        $_[4] .= "\t\$(CP) build/cpp/" . $com_dirname_part . "/" . $name . "Ui.h           buildQtUI/" . $com_dirname_part . "/" . $name . "Ui.h\n";

        # buildQtUI => build/cpp
    }
}

##
# C#で実装しているディレクトリ構造から，javaのパッケージ構造に準拠したパスを取得します．
# @param  string  C#で使ってるパス
# @return  string  パスの文字列
# @example
# ex. 1)   引数が"./Cadencii"の場合"org/kbinani/cadencii"を返す．
# ex. 2)   引数が"./org.kbinani.windows.forms"の場合"org/kbinani/windows/forms"を返す．
#
sub getDirectoryForPackageName
{
    my $dir = $_[0];
    my $ret = "";
    if( index( $dir, "./Cadencii" ) == 0 )
    {
        $ret = "org/kbinani/cadencii";
    }
    elsif( index( $dir, "./org.kbinani.windows.forms" ) == 0 )
    {
        $ret = "org/kbinani/windows/forms";
    }
    elsif( index( $dir, "./org.kbinani.xml" ) == 0 )
    {
        $ret = "org/kbinani/xml";
    }
    elsif( index( $dir, "./org.kbinani.vsq" ) == 0 )
    {
        $ret = "org/kbinani/vsq";
    }
    elsif( index( $dir, "./org.kbinani.media" ) == 0 )
    {
        $ret = "org/kbinani/media";
    }
    elsif( index( $dir, "./org.kbinani.apputil" ) == 0 )
    {
        $ret = "org/kbinani/apputil";
    }
    elsif( index( $dir, "./org.kbinani.componentmodel" ) == 0 )
    {
        $ret = "org/kbinani/componentmodel";
    }
    elsif( index( $dir, "./org.kbinani" ) == 0 )
    {
        $ret = "org/kbinani";
    }

    $ret;
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

    # .csファイルだけにする
    @file = &extractCsFiles( @file );

    my @view_impl = &extractViewImplementation( @file );

    foreach my $v ( @file )
    {
        if( length( $v ) <= 3 )
        {
            next;
        }
        if( !&stringEndsWith( $v, ".cs" ) )
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

        if( index( $v, "Form" ) == 0 && &stringEndsWith( $v, "Impl.cs" ) )
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
    $build_java_ui_prefix = "./BuildJavaUI/src/" . &getDirectoryForPackageName( $dir ) . "/";

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
        if( &stringEndsWith( $cname, "Ui" ) ){
            $add_to = 0;
        }
        if( &stringEndsWith( $cname, "UiListener" ) ){
            $add_to = 0;
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
        my $com_dirname_part = substr( $build_java_ui_prefix, length( "./BuildJavaUI/src/" ) );

        # build/java => BuildJavaUI/src
        $_[4] .= "\t\$(CP) build/java/" . $com_dirname_part . $name . "UiListener.java    BuildJavaUI/src/" . $com_dirname_part . $name . "UiListener.java\n";
        $_[4] .= "\t\$(CP) build/java/" . $com_dirname_part . $name . "Ui.java            BuildJavaUI/src/" . $com_dirname_part . $name . "Ui.java\n";

        # BuildJavaUI/src => build/java
        $_[4] .= "\t\$(CP) BuildJavaUI/src/" . $com_dirname_part . $name . "UiImpl.java   build/java/" . $com_dirname_part . $name . "UiImpl.java\n";
    }
}

sub stringEndsWith{
    my $target = $_[0];
    my $search = $_[1];
    if( rindex( $target, $search ) == length( $target ) - length( $search ) ){
        return 1;
    }else{
        return 0;
    }
}
