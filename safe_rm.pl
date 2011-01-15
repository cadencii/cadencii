if( -e $ARGV[0] ){
	if( $^O eq "MSWin32" ){
		system( "del $ARGV[0]" );
	}else{
		system( "rm $ARGV[0]" );
	}
}
