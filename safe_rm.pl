$f = $ARGV[0];
if( -e $f ){
    if( $^O eq "MSWin32" ){
        $f =~ s/\//\\/g;
        system( "del \"$f\"" );
    }else{
        system( "rm $f" );
    }
}
