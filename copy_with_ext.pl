#!/bin/perl
use File::Copy;

my $srcdir = $ARGV[0];
my $dstdir = $ARGV[1];
my $ext = $ARGV[2];

opendir DH, $srcdir or die "$srcdir:$!";
while( my $file = readdir DH ){
	next if $file =~ /^\.{1,2}$/; #skip '.' and '..'
	my $indx = rindex( $file, $ext );
	if( $indx == length( $file ) - length( $ext ) ){
		my $srcfile = $srcdir . $file;
		my $dstfile = $dstdir . $file;
		copy( $srcfile, $dstfile ) or die $!;
	}
}
closedir DH;
