open( FILE, "<Makefile.include" );
open( OUT, ">Makefile" );

while( $line = <FILE> ){
	if( $^O eq "MSWin32" ){
		if( ($line =~ /\$\(CP\)/) | ($line =~ /\$\(RM\)/) ){
			$line =~ s/\//\\/g;
		}
		$line =~ s/\@CP\@/copy/g;
		$line =~ s/\@RM\@/del/g;
		$line =~ s/\@TARGET\@/.\\build\\win/g;
	}else{
		$line =~ s/\@CP\@/cp/g;
		$line =~ s/\@RM\@/rm/g;
		$line =~ s/\@TARGET\@/\.\/build\/win/g;
	}
	print OUT $line;
}

close( FILE );
close( OUT );
