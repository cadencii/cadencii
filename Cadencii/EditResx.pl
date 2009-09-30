$file = $ARGV[0];

$index = rindex( $file, "." );
$newfile = substr( $file, 0, $index ) . "_.resx";

open( FILE, $file );
open( EDIT, ">" . $newfile );
while( $line = <FILE> ){
#	chomp $line;
	$line =~ s/[\\]/\//g;
	print EDIT $line;
}
close( FILE );
close( EDIT );
