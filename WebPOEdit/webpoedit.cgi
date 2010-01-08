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

sub containsKey{
	my $self = shift;
	my $id = shift;
	my %hash = %{ $self->{dictionary} };
	return exists( $hash{$id} );
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
my @LANGS = (
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
print "get_res=\"" . $get_res . "\"\n";
if( $get_res ne "" ){
	my @spl = split( /\&/, $get_res );
	foreach my $s ( @spl ){
		print "s=\"" . $s . "\"\n";
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
print "stdin=\"" . $stdin . "\"\n";
if( $stdin ne "" ){
	my @spl = split( /\&/, $stdin );
	foreach my $s ( @spl ){
		my @spl2 = split( /\=/, $s );
		push( @commands, $s );
		if( $spl2[0] eq "author" ){
			$author = $spl2[1];
		}
		if( $spl2[0] eq "rauthor" ){
			$author = enc_b64( url_decode( $spl2[1] ) );
		}
	}
}
print "command0=", $command0;
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
		
		# progress percentage
		my $mb = $messaging->getMessageBody( $lang );
		my $en_count = $mben->size();
		my $lang_count = 0;
		foreach my $id ( @{ $mben->getKey() } ){
			if( $mb->containsKey( $id ) ){
				if( !($mb->getMessage( $id ) eq $id ) ){
					$lang_count++;
				}
			}
		}
		my $prog = $lang_count / $en_count * 100.0;
		print "      <td class=" . $class_kind . ">" . $desc . "&nbsp;[" . $lang . "]&nbsp;&nbsp;<a href=\"" . $cgi_name . "?target=" . $lang . "&author=" . $author . "\">edit</a></td>\n";
		my $strProg = sprintf "%3.2f", $prog;
		print "      <td class=" . $class_kind . ">" . $strProg . "% translated</td>\n";
		print "      <td class=" . $class_kind . "><a href=\"" . $project_name . "/" . $lang . ".po\">Download</a></td>\n";
		print "    </tr>\n";
	}
	print "  </table>\n";
	print "  <br>\n";
	print "<h4>If you want to create new language configuration, select language and press \"create\" button.</h4>\n";
	print "  <div class=\"padleft\">\n";
	print "  <form method=\"post\" action=\"" . $cgi_name . "?create=0&author=" . $author . "\">\n";
	
	# get browser UI language
	my $http_accept_language = $ENV{"HTTP_ACCEPT_LANGUAGE"};
	my @spl = split( /\,/, $http_accept_language );
	my %accept_language_list = ();
	foreach my $s ( @spl ){
		# ja,fr;q=0.7,de;q=0.3
		if( $s =~ /\;/ ){
			my @spl2 = split( /\;/, $s ); #spl2 = { "fr", "q=0.7" }
			if( $#spl2 + 1 >= 2 ){
				my @spl3 = split( /\=/, $spl2[1] ); #spl3 = { "q", "0.7" }
				if( $#spl3 + 1 >= 2 ){
					my $value = $spl3[1] + 0.0;
					$accept_language_list{$spl2[0]} = $value;
				}
			}
		} else {
			$accept_language_list{$s} = 1.0;
		}
	}

	# detect most highest q value
	my $most_used = "en";
	my $most_used_q = 0.0;
	foreach my $key ( keys %accept_language_list ){
		if( $most_used_q < $accept_language_list{$key} ){
			$most_used = $key;
			$most_used_q = $accept_language_list{$key};
		}
	}

	# list of languages which has not been genearted
	print "  <select name=\"lang\">\n";
	my $len = $#LANGS + 1;
	for( $i = 0; $i < $len; $i++ ){
		my $found = 0;
		foreach my $lang ( @languages ){
			if( $lang eq $LANGS[$i][1] ){
				$found = 1;
				last;
			}
		}
		if( $found == 1 ){
			print "    <option value=\"" . $LANGS[$i][1] . "\" disabled>" . $LANGS[$i][0] . "\n";
		}elsif( $most_used eq $LANGS[$i, 1] ){
			print "    <option value=\"" . $LANGS[$i][1] . "\" selected>" . $LANGS[$i][0] . "\n";
		}else{
			print "    <option value=\"" . $LANGS[$i][1] . "\">" . $LANGS[$i][0] . "\n";
		}
	}
	print "  <input type=\"submit\" value=\"create\">\n";
	print "  </form>\n";
	print "  </div>\n";
	print "</body>\n";
	print "</html>\n";
}elsif( $command0 eq "target" || $command0 eq "upate" || $command0 eq "create" ){
	my @splLang = split( /\=/, $commands[0] );
	my $lang = $splLang[1];

	my $en = $messaging->getMessageBody( "en" );
	my @keys_ = @{ $en->getKeys() };
	sort @keys_;

	if( $command0 eq "create" ){
		foreach my $v ( @commands ){
			my @splV = split( /\=/, $v );
			if( $splV[0] eq "lang" ){
				$lang = $splV[1];
				last;
			}
		}
		my $newpo = $lang . ".po";
		my $mb0;
		if( -e $newpo ){
			$mb0 = new MessageBody( $newpo );
		}else{
			$mb0 = new MessageBody( "" );
			$mb0->{language} = $lang;
		}
		foreach my $id ( @keys_ ){
			if( !($mb0->containsKey( $id ) ){
				$mb0->put( $id, $en->getMessage( $id ) );
			}
		}
		$mb0->printTo( $newpo );
	}

	$is_rtl = 0;

	print "<title>" . $project_name . "&nbsp;&gt;&gt;&nbsp;" . $lang . "</title>\n";
	print "</head>\n";
	if( $is_rtl != 0 ){
		print "<body dir=\"rtl\">\n";
	}else{
		print "<body>\n";
	}
	print "<form method=\"post\" action=\"" + cgi_name + "?update=" + lang + "&author=" + author + "\">" );
	print "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" + cgi_name + "?start=0&author=" + author + "\">" + project_name + "</a>&gt;&gt;" + lang + "</div>" );
	print "<div align=\"center\">" );
	print "<table border=0 cellspacing=0 width=\"100%\">" );
	print "  <tr>" );
	print "    <td class=\"header\">English</td>" );
	print "    <td class=\"header\">translation</td>" );
	print "  </tr>" );
}
print "</head>\n";
print "<body>\n";
print "</body>\n";
print "</html>\n";

exit;

sub dec_b64{
	my $str = shift;
	$str =~ s/\_/\=/g;
	return decode_base64( $str );
}

sub enc_b64{
	my $str = shift;
	$str = encode_base64( $str );
	$str =~ s/\=/\_/g;
	return $str;
}

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
