#!/usr/bin/perl -w

use Encode;
use MIME::Base64;

package MessageBody;

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
	$argFile =~ /(.*)\.po$/;
	$self = {
		file => $argFile,
		dictionary => \%dict,
		language => $1,
	};
	return bless $self, $class;
}

sub getLanguage{
	my $self = shift;
	return $self->{language};
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

sub size{
	my $self = shift;
	my %hash = %{ $self->{dictionary} };
	my $count = keys %hash;
	return $count;
}

sub getKey{
	my $self = shift;
	my %hash = %{ $self->{dictionary} };
	my @key = keys %hash;
	return \@key;
}

package Messaging;

sub new{
	my $class = shift;
	my $dir = shift;
	my %dict = ();
	# read *.po file in the directory "$dir"
	opendir DH, $dir;
	while( my $file = readdir DH ){
		next if $file =~ /^.{1,2}$/;
		next if !($file =~ /.*\.po$/);
		my $po = new MessageBody( $file );
		my $lang = $po->getLanguage();
		$dict{$lang} = $po;
	}
	my $self = {
		list => \%dict,
	};
	return bless $self, $class;
}

sub getRegisteredLanguage{
	my $self = shift;
	my %list = %{ $self->{list} };
	my @ret = keys %list;
	return \@ret;
}

sub getMessageBody{
	my $self = shift;
	my $lang = shift;
	my %list = %{ $self->{list} };
	foreach my $s ( keys %list ){
		if( $lang eq $s ){
			return $list{$s};
		}
	}
	return new MessageBody( "" );
}

package main;

my $textEncode = "Shift_JIS";
my $project_name = "Cadencii";
my $LANGS = (
	["Afar", "aa"],
	["Abkhazian", "ab"],
	["Afrikaans", "af"],
	["Amharic", "am"],
	["Arabic", "ar"],
	["Assamese", "as"],
	["Aymara", "ay"],
	["Azerbaijani", "az"],
	["Bashkir", "ba"],
	["Byelorussian (Belarussian)", "be"],
	["Bulgarian", "bg"],
	["Bihari", "bh"],
	["Bislama", "bi"],
	["Bengali", "bn"],
	["Tibetan", "bo"],
	["Breton", "br"],
	["Catalan", "ca"],
	["Corsican", "co"],
	["Czech", "cs"],
	["Welsh", "cy"],
	["Danish", "da"],
	["German", "de"],
	["Bhutani", "dz"],
	["Greek", "el"],
	["English", "en"],
	["Esperanto", "eo"],
	["Spanish", "es"],
	["Estonian", "et"],
	["Basque", "eu"],
	["Persian", "fa"],
	["Finnish", "fi"],
	["Fiji", "fj"],
	["Faroese", "fo"],
	["French", "fr"],
	["Frisian", "fy"],
	["Irish (Irish Gaelic)", "ga"],
	["Scots Gaelic (Scottish Gaelic)", "gd"],
	["Galician", "gl"],
	["Guarani", "gn"],
	["Gujarati", "gu"],
	["Manx Gaelic", "gv"],
	["Hausa", "ha"],
	["Hebrew", "he"],
	["Hindi", "hi"],
	["Croatian", "hr"],
	["Hungarian", "hu"],
	["Armenian", "hy"],
	["Interlingua", "ia"],
	["Indonesian", "id"],
	["Interlingue", "ie"],
	["Inupiak", "ik"],
	["Icelandic", "is"],
	["Italian", "it"],
	["Inuktitut", "iu"],
	["Japanese", "ja"],
	["Javanese", "jw"],
	["Georgian", "ka"],
	["Kazakh", "kk"],
	["Greenlandic", "kl"],
	["Cambodian", "km"],
	["Kannada", "kn"],
	["Korean", "ko"],
	["Kashmiri", "ks"],
	["Kurdish", "ku"],
	["Cornish", "kw"],
	["Kirghiz", "ky"],
	["Latin", "la"],
	["Luxemburgish", "lb"],
	["Lingala", "ln"],
	["Laotian", "lo"],
	["Lithuanian", "lt"],
	["Latvian Lettish", "lv"],
	["Malagasy", "mg"],
	["Maori", "mi"],
	["Macedonian", "mk"],
	["Malayalam", "ml"],
	["Mongolian", "mn"],
	["Moldavian", "mo"],
	["Marathi", "mr"],
	["Malay", "ms"],
	["Maltese", "mt"],
	["Burmese", "my"],
	["Nauru", "na"],
	["Nepali", "ne"],
	["Dutch", "nl"],
	["Norwegian", "no"],
	["Occitan", "oc"],
	["Oromo", "om"],
	["Oriya", "or"],
	["Punjabi", "pa"],
	["Polish", "pl"],
	["Pashto", "ps"],
	["Portuguese", "pt"],
	["Quechua", "qu"],
	["Rhaeto-Romance", "rm"],
	["Kirundi", "rn"],
	["Romanian", "ro"],
	["Russian", "ru"],
	["Kiyarwanda", "rw"],
	["Sanskrit", "sa"],
	["Sindhi", "sd"],
	["Northern Sami", "se"],
	["Sangho", "sg"],
	["Serbo-Croatian", "sh"],
	["Singhalese", "si"],
	["Slovak", "sk"],
	["Slovenian", "sl"],
	["Samoan", "sm"],
	["Shona", "sn"],
	["Somali", "so"],
	["Albanian", "sq"],
	["Serbian", "sr"],
	["Siswati", "ss"],
	["Sesotho", "st"],
	["Sudanese", "su"],
	["Swedish", "sv"],
	["Swahili", "sw"],
	["Tamil", "ta"],
	["Telugu", "te"],
	["Tajik", "tg"],
	["Thai", "th"],
	["Tigrinya", "ti"],
	["Turkmen", "tk"],
	["Tagalog", "tl"],
	["Setswana", "tn"],
	["Tonga", "to"],
	["Turkish", "tr"],
	["Tsonga", "ts"],
	["Tatar", "tt"],
	["Twi", "tw"],
	["Uigur", "ug"],
	["Ukrainian", "uk"],
	["Urdu", "ur"],
	["Uzbek", "uz"],
	["Vietnamese", "vi"],
	["Volapuk", "vo"],
	["Wolof", "wo"],
	["Xhosa", "xh"],
	["Yiddish", "yi"],
	["Yorouba", "yo"],
	["Zhuang", "za"],
	["Chinese(Taiwan)", "zh-TW"],
	["Chinese(China)", "zh-CN"],
	["Zulu", "zu"] );

print "Content-type: text/html" . "\n\n";

$app_name = "webpoedit";
$cgi_name = "webpoedit.cgi";
print "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n";

print "<html>\n";
print "<head>\n";
print "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=" . $textEncode . "\">\n";
print "<meta http-equiv=\"Content-Style-Type\" content=\"text/css\">\n";
print "<link rel=\"stylesheet\" type=\"text/css\" href=\"../style.css\">\n";

my $stdin = "";
while( my $line = <STDIN> ){
	$stdin = $stdin . $line;
}
my $get_res = $ENV{"QUERY_STRING"};
my $command0 = "";
my $author = "";
my @commands;
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
			$author = encode_base64( url_decode( $spl2[1] ) );
		}
	}
}
my $messaging = new Messaging( "./" );
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
	my @languages = @{ $messaging->getRegisteredLanguage() };
	my $count = -1;
	my $mben = new MessageBody( "./en.po" );
	foreach my $lang ( @languages ){
		if( $lang eq "en" ){
			next;
		}
		$count = $count + 1;
		$class_kind = "\"even\"";
		if( $count % 2 != 0 ){
			$class_kind = "\"odd\"";
		}
		print "    <tr>\n";
		$desc = "";
		for( $i = 0; $i <= $#LANGS; $i++ ){
			if( $LANGS[$i][1] eq $lang ){
				$desc = $LANGS[$i][0];
				last;
			}
		}
		my $mb = $messaging->getMessageBody( $lang );
		my $en_count = $mben->size();
		my $lang_count = 0;
		foreach my $id ( @{ $mben->getKey() } ){
			
		}
	}
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
