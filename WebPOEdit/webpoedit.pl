use Encode;

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
		close IN;
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
	close $out;
}

package main;

my $ja = new POFile( "ja.po" );
my $zhTW = new POFile( "zh-TW.po" );

