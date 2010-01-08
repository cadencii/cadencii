#!/usr/bin/perl -w

use Encode;
use MIME::Base64;

package POFile;

sub new{
	my $class = shift;
	my $argFile = shift;
	my %dict = ();

	# read file 
	if( -e $argFile ){
		open my $in, "<", $argFile or die "cannot open file\n";
		$id = "";
		$str = "";
		$mode = 0; #0: reading id, 1: reding str
		while( $line = <$in> ){
			chomp $line;
			Encode::from_to( $line, "utf8", "shift_jis" );
			if( $line =~ /^msgid \"(.*)\"/ ){
				if( !($id eq "") ){
					$dict{$id} = $str;
				}
				$id = $1;
				$mode = 0;
			}elsif( $line =~ /^msgstr \"(.*)\"/ ){
				$str = $1;
				$mode = 1;
			}elsif( $line =~ /\"(.*)\"/ ){
				if( $mode == 0 ){
					$id = $id . $1;
				}else{
					$str = $str . $1;
				}
			}
		}
		close $in;
	}
	$self = {
		file => $argFile,
		dictionary => \%dict,
	};
	return bless $self, $class;
}

sub getFile{
	my $self = shift;
	return $self->{file};
}

sub getMessage{
	my $self = shift;
	my $id = shift;
	my %hash = %{ $self->{dictionary} };
	if( exists( $hash{$id} ) ){
		return $hash{$id};
	}else{
		return $id;
	}
}

sub put{
	my $self = shift;
	my $id = shift;
	my $str = shift;
	my %hash = %{ $self->{dictionary} };
	$hash{$id} = $str;
	$self->{dictionary} = \%hash;
}

sub printTo{
	my $self = shift;
	my $file = shift;
	my %hash = %{ $self->{dictionary} };
	open( OUT, ">", $file ) or die "cannot open file\n";

	print OUT "msgid \"\"\n";
	print OUT "msgstr \"\"\n";
	print OUT "\"Content-Type: text/plain; charset=UTF-8\\n\"\n";
	print OUT "\"Content-Transfer-Encoding: 8bit\\n\"\n\n";

	foreach $key ( keys %hash ){
		$id = $key;
		$str = $hash{$key};
		Encode::from_to( $id, "shift_jis", "utf8" );
		Encode::from_to( $str, "shift_jis", "utf8" );
		print OUT "msgid \"" . $id . "\"\n";
		print OUT "msgstr \"" . $str . "\"\n\n";
	}
	close OUT;
}

package Messaging;

sub loadMessages{
	my $class = shift;
	my $dir = shift;
}

package main;

my $ja = new POFile( "ja.po" );
#my $zhTW = new POFile( "zh-TW.po" );
#$ja->printTo( "../htdocs/foo.po" );
my $textEncode = "Shift_JIS";
my $project_name = "Cadencii";

print "Content-type: text/html" . "\n\n";

$app_name = "webpoedit";
$cgi_name = "webpoedit.cgi";
print "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n";

print "<html>\n";
print "<head>\n";
print "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=" . $textEncode . "\">\n";
print "<meta http-equiv=\"Content-Style-Type\" content=\"text/css\">\n";
print "<link rel=\"stylesheet\" type=\"text/css\" href=\"../style.css\">\n";

$stdin = "";
while( $line = <STDIN> ){
	$stdin = $stdin . $line;
}
$get_res = $ENV{"QUERY_STRING"};
$command0 = "";
$author = "";
@commands;
if( !($get_res eq "") ){
	my @spl = split( /\&/, $stdin );
	foreach my $s ( @spl ){
		my @spl2 = split( /\=/, $s );
		push( @commands, $s );
		if( $spl2[0] eq "author" ){
			$author = $spl2[1];
		}else{
			if( $command0 eq "" ){
				$command0 = $spl2[0];
			}
		}
		
	}
}
if( !($stdin eq "") ){
	my @spl = split( /\&/, $stdin );
	foreach my $s ( @spl ){
		my @spl2 = split( /\=/, $s );
		push( @commands, $s );
		if( $spl2[0] eq "author" ){
			$author = $spl2[1];
		}
		if( $spl2[0] eq "rauthor" ){
			$author = encode_base64( $url_decode( $spl2[1] ) );
		}
	}
}
#Messaging::loadMessages
if( $author eq "" ){
	print "<title>" . $project_name . " localization</title>\n";
	print "</head>\n";
	print "<body>\n";
	print "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" . $cgi_name . "\">" . $project_name . "</a></div>\n";
	print "enter your nickname<br>\n";
	print "<form method=\"post\" action=\"" . $cgi_name . "?start=0\">\n";
	print "<input type=\"text\" name=\"rauthor\" size=30 value=\"\">\n";
	print "<input type=\"submit\" value=\"start\"></form>\n";
	print "</body>\n";
	print "</html>\n";
}elsif( $command0 eq "start" ){
	print "<title>" . $project_name . " localization</title>\n";
	print "</head>\n";
	print "<body>\n";
	print "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" . $cgi_name . "?start=0&author=" . $author . "\">" . $project_name . "</a></div>\n";
	print "<h4>List of language configuration</h4>\n";
	print "  <table class=\"padleft\" border=0 cellspacing=0 width=\"100%\">\n";
	print "    <tr>\n";
	print "      <td class=\"header\">Language</td>\n";
	print "      <td class=\"header\">Progress</td>\n";
	print "      <td class=\"header\">Download language config</td>\n";
	print "    </tr>\n";
}
print "</head>\n";
print "<body>\n";
print "</body>\n";
print "</html>\n";

exit;

sub url_encode($) {
	my $str = shift;
	$str =~ s/([^\w ])/'%'.unpack('H2', $1)/eg;
	$str =~ tr/ /+/;
	return $str;
}

sub url_decode($) {
	my $str = shift;
	$str =~ tr/+/ /;
	$str =~ s/%([0-9A-Fa-f][0-9A-Fa-f])/pack('H2', $1)/eg;
	return $str;
}
