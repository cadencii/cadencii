open( FILE, "<Makefile.include" );
open( OUT, ">Makefile" );

@special_dependencies = (
"./org.kbinani.windows.forms/BDialog.java",
"./Cadencii/FormImportLyric.java",
"./Cadencii/FormMain.java",
"./Cadencii/LyricTextBox.java",
"./Cadencii/TrackSelector.java",
"./org.kbinani/BDelegate.java",
"./org.kbinani/BEvent.java",
"./org.kbinani/BEventArgs.java",
"./org.kbinani/BEventHandler.java",
"./Cadencii/Preference.java",
"./Cadencii/FormAskKeySoundGeneration.java",
"./Cadencii/FormBeatConfig.java",
"./Cadencii/FormBezierPointEdit.java",
"./Cadencii/FormCompileResult.java",
"./Cadencii/FormCurvePointEdit.java",
"./Cadencii/FormDeleteBar.java",
"./Cadencii/FormGameControlerConfig.java",
"./Cadencii/FormGenerateKeySound.java",
"./Cadencii/FormIconPalette.java",
"./Cadencii/FormInsertBar.java",
"./Cadencii/FormMidiConfig.java",
"./Cadencii/FormMidiImExport.java",
"./Cadencii/FormMixer.java",
"./Cadencii/FormNoteExpressionConfig.java",
"./Cadencii/FormNoteProperty.java",
"./Cadencii/FormRandomize.java",
"./Cadencii/FormRealtimeConfig.java",
"./Cadencii/FormShortcutKeys.java",
"./Cadencii/FormSingerStyleConfig.java",
"./Cadencii/FormSynthesize.java",
"./Cadencii/FormTempoConfig.java",
"./Cadencii/FormTrackProperty.java",
"./Cadencii/FormVibratoConfig.java",
"./Cadencii/FormWordDictionary.java",
);

&getSrcList( "./org.kbinani", "./build/java/org/kbinani/", $src_corlib, $cp_corlib, $dep_corlib );
&getSrcList( "./org.kbinani.apputil", "./build/java/org/kbinani/apputil/", $src_apputil, $cp_apputil, $dep_apputil );
&getSrcList( "./org.kbinani.componentmodel", "./build/java/org/kbinani/componentmodel/", $src_componentmodel, $cp_componentmodel, $dep_componentmodel );
&getSrcList( "./org.kbinani.media", "./build/java/org/kbinani/media/", $src_media, $cp_media, $dep_media );
&getSrcList( "./org.kbinani.vsq", "./build/java/org/kbinani/vsq/", $src_vsq, $cp_vsq, $dep_vsq );
&getSrcList( "./org.kbinani.windows.forms", "./build/java/org/kbinani/windows/forms/", $src_winforms, $cp_winforms, $dep_winforms );
&getSrcList( "./org.kbinani.xml", "./build/java/org/kbinani/xml/", $src_xml, $cp_xml, $dep_xml );
&getSrcList( "./Cadencii", "./build/java/org/kbinani/cadencii/", $src_cadencii, $cp_cadencii, $dep_cadencii );

$dep_special = "";
foreach my $sdep ( @special_dependencies ){
    my $indx = rindex( $sdep, "/" );
    my $fname = substr( $sdep, $indx + 1 );
    my $indx2 = rindex( $sdep, ".java" );
    my $sdep_cs = substr( $sdep, 0, length( $sdep ) - 5 ) . ".cs";
    my $prefix = "";
    if( index( $sdep, "./Cadencii/" ) == 0 ){
        $prefix = "org/kbinani/cadencii";
    }elsif( index( $sdep, "./org.kbinani/" ) == 0 ){
        $prefix = "org/kbinani";
    }elsif( index( $sdep, "./org.kbinani.windows.forms/" ) == 0 ){
        $prefix = "org/kbinani/windows/forms";
    }
    $dep_special .= "./build/java/$prefix/$fname: ./BuildJavaUI/src/$prefix/$fname $sdep_cs\n";
    $dep_special .= "\tmono ./pp_cs2java.exe \$(PPCS2JAVA_OPT) -i $sdep_cs -o $sdep\n";
    $dep_special .= "\t\$(CP) $sdep ./build/java/$prefix/$fname\n\n";
}

while( $line = <FILE> ){
    $line =~ s/\@SRC_JAPPUTIL\@/$src_apputil/g;
    $line =~ s/\@SRC_JCORLIB\@/$src_corlib/g;
    $line =~ s/\@SRC_JWINFORMS\@/$src_winforms/g;
    $line =~ s/\@SRC_JMEDIA\@/$src_media/g;
    $line =~ s/\@SRC_JVSQ\@/$src_vsq/g;
    $line =~ s/\@SRC_JCADENCII\@/$src_cadencii/g;
    $line =~ s/\@SRC_JCOMPONENTMODEL\@/$src_componentmodel/g;
    $line =~ s/\@SRC_JXML\@/$src_xml/g;

#    $line =~ s/\@CP_JAPPUTIL\@/$cp_apputil/g;
#    $line =~ s/\@CP_JCORLIB\@/$cp_corlib/g;
#    $line =~ s/\@CP_JWINFORMS\@/$cp_winforms/g;
#    $line =~ s/\@CP_JMEDIA\@/$cp_media/g;
#    $line =~ s/\@CP_JVSQ\@/$cp_vsq/g;
#    $line =~ s/\@CP_JCADENCII\@/$cp_cadencii/g;
#    $line =~ s/\@CP_JCOMPONENTMODEL\@/$cp_componentmodel/g;
#    $line =~ s/\@CP_JXML\@/$cp_xml/g;

    $line =~ s/\@DEP_JAPPUTIL\@/$dep_apputil/g;
    $line =~ s/\@DEP_JCORLIB\@/$dep_corlib/g;
    $line =~ s/\@DEP_JWINFORMS\@/$dep_winforms/g;
    $line =~ s/\@DEP_JMEDIA\@/$dep_media/g;
    $line =~ s/\@DEP_JVSQ\@/$dep_vsq/g;
    $line =~ s/\@DEP_JCADENCII\@/$dep_cadencii/g;
    $line =~ s/\@DEP_JCOMPONENTMODEL\@/$dep_componentmodel/g;
    $line =~ s/\@DEP_JXML\@/$dep_xml/g;
    $line =~ s/\@DEP_SPECIAL\@/$dep_special/g;

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
                    break;
                }
            }
            if( $found == 0 ){
                push( @src, $s1 );
            }
            push( @srcall, $s1 );
        }
    }
    $_[2] = "";
    $_[3] = "";
    $_[4] = "";
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
    
    $count = @src;
    for( $i = 0; $i < $count; $i++ ){
        my $cname = $src[$i];
        my $s = $cname . ".java";
        $_[3] = $_[3] . "$prefix$s:$dir/$s\n\t\$(CP) $dir/$s $prefix$s\n";
        #old => $_[4] = $_[4] . "$prefix$cname.java: $dir/$cname.cs\n";
        $_[4] .= "$prefix$cname.java: $dir/$cname.cs\n";
        $_[4] .= "\tmono ./pp_cs2java.exe \$(PPCS2JAVA_OPT) -i $dir/$cname.cs -o $dir/$cname.java\n";
        $_[4] .= "\t\$(CP) $dir/$cname.java $prefix$cname.java\n\n";
#    $dep_special .= "./build/java/$prefix/$fname: ./BuildJavaUI/src/$prefix/$fname $sdep_cs\n";
#    $dep_special .= "\tmono ./pp_cs2java.exe \$(PPCS2JAVA_OPT) -i $sdep_cs -o $sdep\n";
#    $dep_special .= "\t\$(CP) $sdep ./build/java/$prefix/$fname\n\n";
    }
}
