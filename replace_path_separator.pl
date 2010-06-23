open( FILE, "<Makefile.include" );
open( OUT, ">Makefile" );

&getSrcList( "./org.kbinani", "./build/java/org/kbinani/", $src_corlib, $cp_corlib, $dep_corlib );
&getSrcList( "./org.kbinani.apputil", "./build/java/org/kbinani/apputil/", $src_apputil, $cp_apputil, $dep_apputil );
&getSrcList( "./org.kbinani.componentmodel", "./build/java/org/kbinani/componentmodel/", $src_componentmodel, $cp_componentmodel, $dep_componentmodel );
&getSrcList( "./org.kbinani.media", "./build/java/org/kbinani/media/", $src_media, $cp_media, $dep_mdeia );
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

    $line =~ s/\@CP_JAPPUTIL\@/$cp_apputil/g;
    $line =~ s/\@CP_JCORLIB\@/$cp_corlib/g;
    $line =~ s/\@CP_JWINFORMS\@/$cp_winforms/g;
    $line =~ s/\@CP_JMEDIA\@/$cp_media/g;
    $line =~ s/\@CP_JVSQ\@/$cp_vsq/g;
    $line =~ s/\@CP_JCADENCII\@/$cp_cadencii/g;
    $line =~ s/\@CP_JCOMPONENTMODEL\@/$cp_componentmodel/g;
    $line =~ s/\@CP_JXML\@/$cp_xml/g;

    $line =~ s/\@DEP_JAPPUTIL\@/$dep_apputil/g;
    $line =~ s/\@DEP_JCORLIB\@/$dep_corlib/g;
    $line =~ s/\@DEP_JWINFORMS\@/$dep_winforms/g;
    $line =~ s/\@DEP_JMEDIA\@/$dep_media/g;
    $line =~ s/\@DEP_JVSQ\@/$dep_vsq/g;
    $line =~ s/\@DEP_JCADENCII\@/$dep_cadencii/g;
    $line =~ s/\@DEP_JCOMPONENTMODEL\@/$dep_componentmodel/g;
    $line =~ s/\@DEP_JXML\@/$dep_xml/g;

    if( $ARGV[0] eq "MSWin32" ){
        if( ($line =~ /\$\(CP\)/) | ($line =~ /\$\(RM\)/) | ($line =~ /\$\(MKDIR\)/) ){
            $line =~ s/\//\\/g;
        }
        $line =~ s/\@CP\@/copy/g;
        $line =~ s/\@RM\@/del/g;
        $line =~ s/\@TARGET\@/.\\build\\win/g;
        $line =~ s/\@MKDIR\@/perl safe_mkdir\.pl/g;
        $line =~ s/\@PLAY_SOUND_DLL\@/\$\(TARGET\)\\PlaySound\.dll/g;
    }else{
        $line =~ s/\@CP\@/cp/g;
        $line =~ s/\@RM\@/rm/g;
        $line =~ s/\@TARGET\@/\.\/build\/win/g;
        $line =~ s/\@MKDIR\@/perl safe_mkdir\.pl/g;
        $line =~ s/\@PLAY_SOUND_DLL\@//g;
    }
    print OUT $line;
}

close( FILE );
close( OUT );

sub getSrcList{
    my $dir = $_[0];
    my $prefix = $_[1];
    my $DIR;
    opendir( DIR, $dir );
    my @file = readdir( DIR );
    closedir( DIR );
    my @src = ();
    foreach $v ( @file ){
        if( length( $v ) <= 3 ){
            next;
        }
        if( rindex( $v, ".cs" ) == length( $v ) - 3 ){
            my $s1 = substr( $v, 0, length( $v ) - 3 );
            push( @src, $s1 );
        }
    }
    $_[2] = "";
    $_[3] = "";
    $_[4] = "";
    my $count = @src;
    for( $i = 0; $i < $count; $i++ ){
        my $cname = $src[$i];
        my $s = $cname . ".java";
        if( $i == 0 ){
            $_[2] = $prefix . $s;
        }else{
            $_[2] = $_[2] . " \\" . "\n        " . $prefix . $s;
        }
        $_[3] = $_[3] . "$prefix$s:$dir/$s\n\t\$(CP) $dir/$s $prefix$s\n";
        $_[4] = $_[4] . "$prefix$cname.java: $dir/$cname.cs\n";
    }
}
