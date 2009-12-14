/*
 * Program.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of WebPOEdit.
 *
 * WebPOEdit is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * WebPOEdit is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Net;

using org.kbinani.apputil;

namespace org.WebPOEdit {

    class Program {
        const string _TEXT_ENC = "utf-8";
        const string config = "WebPOEdit.ini";
        static readonly string[,] _LANGS = new string[,]{
            {"Afar", "aa"},
            {"Abkhazian", "ab"},
            {"Afrikaans", "af"},
            {"Amharic", "am"},
            {"Arabic", "ar"},
            {"Assamese", "as"},
            {"Aymara", "ay"},
            {"Azerbaijani", "az"},
            {"Bashkir", "ba"},
            {"Byelorussian (Belarussian)", "be"},
            {"Bulgarian", "bg"},
            {"Bihari", "bh"},
            {"Bislama", "bi"},
            {"Bengali", "bn"},
            {"Tibetan", "bo"},
            {"Breton", "br"},
            {"Catalan", "ca"},
            {"Corsican", "co"},
            {"Czech", "cs"},
            {"Welsh", "cy"},
            {"Danish", "da"},
            {"German", "de"},
            {"Bhutani", "dz"},
            {"Greek", "el"},
            {"English", "en"},
            {"Esperanto", "eo"},
            {"Spanish", "es"},
            {"Estonian", "et"},
            {"Basque", "eu"},
            {"Persian", "fa"},
            {"Finnish", "fi"},
            {"Fiji", "fj"},
            {"Faroese", "fo"},
            {"French", "fr"},
            {"Frisian", "fy"},
            {"Irish (Irish Gaelic)", "ga"},
            {"Scots Gaelic (Scottish Gaelic)", "gd"},
            {"Galician", "gl"},
            {"Guarani", "gn"},
            {"Gujarati", "gu"},
            {"Manx Gaelic", "gv"},
            {"Hausa", "ha"},
            {"Hebrew", "he"},
            {"Hindi", "hi"},
            {"Croatian", "hr"},
            {"Hungarian", "hu"},
            {"Armenian", "hy"},
            {"Interlingua", "ia"},
            {"Indonesian", "id"},
            {"Interlingue", "ie"},
            {"Inupiak", "ik"},
            {"Icelandic", "is"},
            {"Italian", "it"},
            {"Inuktitut", "iu"},
            {"Japanese", "ja"},
            {"Javanese", "jw"},
            {"Georgian", "ka"},
            {"Kazakh", "kk"},
            {"Greenlandic", "kl"},
            {"Cambodian", "km"},
            {"Kannada", "kn"},
            {"Korean", "ko"},
            {"Kashmiri", "ks"},
            {"Kurdish", "ku"},
            {"Cornish", "kw"},
            {"Kirghiz", "ky"},
            {"Latin", "la"},
            {"Luxemburgish", "lb"},
            {"Lingala", "ln"},
            {"Laotian", "lo"},
            {"Lithuanian", "lt"},
            {"Latvian Lettish", "lv"},
            {"Malagasy", "mg"},
            {"Maori", "mi"},
            {"Macedonian", "mk"},
            {"Malayalam", "ml"},
            {"Mongolian", "mn"},
            {"Moldavian", "mo"},
            {"Marathi", "mr"},
            {"Malay", "ms"},
            {"Maltese", "mt"},
            {"Burmese", "my"},
            {"Nauru", "na"},
            {"Nepali", "ne"},
            {"Dutch", "nl"},
            {"Norwegian", "no"},
            {"Occitan", "oc"},
            {"Oromo", "om"},
            {"Oriya", "or"},
            {"Punjabi", "pa"},
            {"Polish", "pl"},
            {"Pashto", "ps"},
            {"Portuguese", "pt"},
            {"Quechua", "qu"},
            {"Rhaeto-Romance", "rm"},
            {"Kirundi", "rn"},
            {"Romanian", "ro"},
            {"Russian", "ru"},
            {"Kiyarwanda", "rw"},
            {"Sanskrit", "sa"},
            {"Sindhi", "sd"},
            {"Northern Sami", "se"},
            {"Sangho", "sg"},
            {"Serbo-Croatian", "sh"},
            {"Singhalese", "si"},
            {"Slovak", "sk"},
            {"Slovenian", "sl"},
            {"Samoan", "sm"},
            {"Shona", "sn"},
            {"Somali", "so"},
            {"Albanian", "sq"},
            {"Serbian", "sr"},
            {"Siswati", "ss"},
            {"Sesotho", "st"},
            {"Sudanese", "su"},
            {"Swedish", "sv"},
            {"Swahili", "sw"},
            {"Tamil", "ta"},
            {"Telugu", "te"},
            {"Tajik", "tg"},
            {"Thai", "th"},
            {"Tigrinya", "ti"},
            {"Turkmen", "tk"},
            {"Tagalog", "tl"},
            {"Setswana", "tn"},
            {"Tonga", "to"},
            {"Turkish", "tr"},
            {"Tsonga", "ts"},
            {"Tatar", "tt"},
            {"Twi", "tw"},
            {"Uigur", "ug"},
            {"Ukrainian", "uk"},
            {"Urdu", "ur"},
            {"Uzbek", "uz"},
            {"Vietnamese", "vi"},
            {"Volapuk", "vo"},
            {"Wolof", "wo"},
            {"Xhosa", "xh"},
            {"Yiddish", "yi"},
            {"Yorouba", "yo"},
            {"Zhuang", "za"},
            {"Chinese(Taiwan)", "zh-TW"},
            {"Chinese(China)", "zh-CN"},
            {"Zulu", "zu"}};

        static void Main( string[] args ) {
            print( "Content-type: text/html\n" );
            Console.OutputEncoding = Encoding.GetEncoding( _TEXT_ENC );
            string app_name = "webpoedit";
            string executing_file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            app_name = Path.GetFileNameWithoutExtension( executing_file ).ToLower();
            string cgi_name = Path.GetFileName( executing_file ).ToLower();
            StreamWriter logger = null;
            print( "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" );

            print( "<html>" );
            print( "<head>" );
            print( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=" + _TEXT_ENC + "\">" );
            print( "<meta http-equiv=\"Content-Style-Type\" content=\"text/css\">" );
            try {
                logger = new StreamWriter( "webpoedit.log", true );
                string project_name = "";
                StreamReader sr = null;
                if ( !File.Exists( config ) ) {
                    print( "<head><title>" + app_name + ": Error</title></head>" );
                    print( "<body>" );
                    print( "<b>configuration file not found</b>" );
                    print( "</body>" );
                    print( "</html>" );
                    return;
                }
                try {
                    sr = new StreamReader( config );
                    project_name = sr.ReadLine();
                } catch ( Exception ex ) {
                    print( "<head><title>" + app_name + ": Error<title></head>" );
                    print( "<body>" );
                    print( "<b>configuration file I/O error</b>" );
                    print( "</body>" );
                    print( "</html>" );
                    return;
                }
                print( "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + project_name + "/style.css\">" );

                TextReader tr = Console.In;
                string stdin = "";
                string line = "";
                while ( (line = tr.ReadLine()) != null ) {
                    stdin += line;
                }
                string get_res = Environment.GetEnvironmentVariable( "QUERY_STRING" );
                string command0 = "";
                string author = "";
                List<KeyValuePair<string, string>> commands = new List<KeyValuePair<string, string>>();
                if ( get_res != "" ) {
                    string[] spl = get_res.Split( '&' );
                    for ( int i = 0; i < spl.Length; i++ ) {
                        string s = spl[i];
                        string[] spl2 = s.Split( "=".ToCharArray(), 2 );
                        commands.Add( new KeyValuePair<string, string>( spl2[0], spl2[1] ) );
                        if ( spl2[0] == "author" ) {
                            author = spl2[1];
                        } else {
                            if ( command0 == "" ) {
                                command0 = spl2[0];
                            }
                        }
                    }
                }
                if ( stdin != "" ) {
                    string[] spl = stdin.Split( '&' );
                    foreach ( string s in spl ) {
                        string[] spl2 = s.Split( "=".ToCharArray(), 2 );
                        commands.Add( new KeyValuePair<string, string>( spl2[0], spl2[1] ) );
                        if ( spl2[0] == "author" ) {
                            author = spl2[1];
                        }
                        if ( spl2[0] == "rauthor" ) {
                            author = enc_b64( dec_url( spl2[1] ) );
                        }
                    }
                }
                Messaging.loadMessages( project_name );
#if DEBUG
                if ( commands.Count > 0 ) print( "commands[0].Key=" + commands[0].Key + "; commands[0].Value=" + commands[0].Value );
#endif
                if ( author == "" ) {
                    print( "<title>" + project_name + " localization</title>" );
                    print( "</head>" );
                    print( "<body>" );
                    print( "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" + cgi_name + "\">" + project_name + "</a></div>" );
                    print( "enter your nickname<br>" );
                    print( "<form method=\"post\" action=\"" + cgi_name + "?start=0\">" );
                    print( "<input type=\"text\" name=\"rauthor\" size=30 value=\"\">" );
                    print( "<input type=\"submit\" value=\"start\"></form>" );
                    print( "</body>" );
                    print( "</html>" );
                } else if ( command0 == "start" ) {
                    logger.WriteLine( DateTime.Now + " start; author=" + dec_b64( author ) );
                    // 現在編集可能な言語ファイルのリストを表示
                    print( "<title>" + project_name + " localization</title>" );
                    print( "</head>" );
                    print( "<body>" );
                    print( "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" + cgi_name + "?start=0&author=" + author + "\">" + project_name + "</a></div>" );
                    print( "<h4>List of language configuration</h4>" );
                    print( "  <table class=\"padleft\" border=0 cellspacing=0 width=\"100%\">" );
                    print( "    <tr>" );
                    print( "      <td class=\"header\">Language</td>" );
                    print( "      <td class=\"header\">Progress</td>" );
                    print( "      <td class=\"header\">Download language config</td>" );
                    print( "    </tr>" );
                    List<string> languages = new List<string>( Messaging.getRegisteredLanguage() );
                    int count = -1;
                    MessageBody mben = new MessageBody( "en", Path.Combine( project_name, "en.po" ) );
                    foreach ( string lang in languages ) {
                        if ( lang == "en" ) {
                            continue;
                        }
                        count++;
                        string class_kind = "\"even\"";
                        if ( count % 2 != 0 ) {
                            class_kind = "\"odd\"";
                        }
                        print( "    <tr>" );
                        string desc = "";
                        for ( int i = 0; i < _LANGS.GetUpperBound( 0 ) + 1; i++ ) {
                            if ( _LANGS[i, 1] == lang ) {
                                desc = _LANGS[i, 0];
                                break;
                            }
                        }

                        // 進捗率を計算。
                        MessageBody mb = new MessageBody( lang, Path.Combine( project_name, lang + ".po" ) );
                        int en_count = mben.list.Count;
                        int lang_count = 0;
                        foreach ( string id in mben.list.Keys ) {
                            if ( mb.list.ContainsKey( id ) ) {
                                if ( mb.list[id].message != id ) {
                                    lang_count++;
                                }
                            }
                        }
                        float prog = (float)lang_count / (float)en_count * 100.0f;
                        print( "      <td class=" + class_kind + ">" + desc + "&nbsp;[" + lang + "]&nbsp;&nbsp;<a href=\"" + cgi_name + "?target=" + lang + "&author=" + author + "\">edit</a></td>" );
                        print( "      <td class=" + class_kind + ">" + prog.ToString( "0.00" ) + "% translated</td>" );
                        print( "      <td class=" + class_kind + "><a href=\"" + project_name + "/" + lang + ".po\">Download</a></td>" );
                        print( "    </tr>" );
                    }
                    print( "  </table>" );
                    print( "  <br>" );
                    print( "<h4>If you want to create new language configuration, select language and press \"create\" button.</h4>" );
                    print( "  <div class=\"padleft\">" );
                    print( "  <form method=\"post\" action=\"" + cgi_name + "?create=0&author=" + author + "\">" );

                    // ブラウザの使用言語を取得
                    string http_accept_language = Environment.GetEnvironmentVariable( "HTTP_ACCEPT_LANGUAGE" );
                    string[] spl = http_accept_language.Split( ',' );
                    Dictionary<string, float> accept_language_list = new Dictionary<string, float>();
                    foreach ( string s in spl ) {
                        // ja,fr;q=0.7,de;q=0.3
                        if ( s.Contains( ";" ) ) {
                            string[] spl2 = s.Split( ';' ); //spl2 = { "fr", "q=0.7" }
                            if ( spl2.Length >= 2 ) {
                                string[] spl3 = spl2[1].Split( '=' ); //spl3 = { "q", "0.7" }
                                if ( spl3.Length >= 2 ) {
                                    float q = 0.0f;
                                    if ( float.TryParse( spl3[1], out q ) ) {
                                        accept_language_list.Add( spl2[0], q );
                                    }
                                }
                            }
                        } else {
                            accept_language_list.Add( s, 1.0f );
                        }
                    }
                    // 最も品質値の高い言語はどれか
                    string most_used = "en";
                    float most_used_q = 0.0f;
                    foreach ( string key in accept_language_list.Keys ) {
                        if ( most_used_q < accept_language_list[key] ) {
                            most_used = key;
                            most_used_q = accept_language_list[key];
                        }
                    }

                    // 未作成の言語設定ファイルを列挙
                    print( "  <select name=\"lang\">" );
                    int len = _LANGS.GetUpperBound( 0 ) + 1;
                    for ( int i = 0; i < len; i++ ) {
                        if ( languages.Contains( _LANGS[i, 1] ) ) {
                            print( "    <option value=\"" + _LANGS[i, 1] + "\" disabled>" + _LANGS[i, 0] );
                        } else if ( most_used == _LANGS[i, 1] ) {
                            print( "    <option value=\"" + _LANGS[i, 1] + "\" selected>" + _LANGS[i, 0] );
                        } else {
                            print( "    <option value=\"" + _LANGS[i, 1] + "\">" + _LANGS[i, 0] );
                        }
                    }
                    print( "  <input type=\"submit\" value=\"create\">" );
                    print( "  </form>" );
                    print( "  </div>" );
                    print( "</body>" );
                    print( "</html>" );
                } else if ( command0 == "target" || command0 == "update" || command0 == "create" ) {
                    // 指定された言語の現在の状態を表示。
                    string lang = commands[0].Value;

                    List<string> keys = new List<string>( Messaging.getKeys( "en" ) );
                    keys.Sort();

                    if ( command0 == "create" ) {
                        foreach ( KeyValuePair<string, string> v in commands ) {
                            if ( v.Key == "lang" ) {
                                lang = v.Value;
                                break;
                            }
                        }
                        string newpo = Path.Combine( project_name, lang + ".po" );
                        MessageBody mb0 = null;
                        if ( File.Exists( newpo ) ) {
                            // すでに存在するlangでcreateが指定された場合
                            mb0 = new MessageBody( lang, newpo );
                        } else {
                            mb0 = new MessageBody( lang );
                        }
                        Messaging.setLanguage( "en" );
                        foreach ( string id in keys ) {
                            if ( !mb0.list.ContainsKey( id ) ) {
                                MessageBodyEntry item = Messaging.getMessageDetail( id );
                                MessageBodyEntry add = new MessageBodyEntry( item.message, item.location.ToArray() );
                                mb0.list.Add( id, add );
                            }
                        }

                        // google language apiを使って訳せるところは訳す。en -> [lang]の結果が元のenと異なり、かつ、en -> [lang] -> enが元のenと同じなら翻訳できたとみなす
                        //url: http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=hello%20world&langpair=en%7Cit
                        //{"responseData": {"translatedText":"ciao mondo"}, "responseDetails": null, "responseStatus": 200}
                        foreach ( string en_query in keys ) {
                            if ( en_query == mb0.getMessage( en_query ) ) {
                                string lang_query = translate( en_query, "en", lang );
                                string en_revert = translate( lang_query, lang, "en" );
                                if ( en_query.ToLower() == en_revert.ToLower() ) {
                                    mb0.list[en_query].message = lang_query;
                                    logger.WriteLine( "    translated: {id,value}={" + en_query + "," + lang_query + "}" );
                                }
                            }
                        }

                        mb0.write( newpo );
                    }

                    bool is_rtl = Util.isRightToLeftLanguage( lang );
                    logger.WriteLine( DateTime.Now + " " + command0 + "; lang=" + lang + "; author=" + dec_b64( author ) );

                    print( "<title>" + project_name + "&nbsp;&gt;&gt;&nbsp;" + lang + "</title>" );
                    print( "</head>" );
                    if ( is_rtl ) {
                        print( "<body dir=\"rtl\">" );
                    } else {
                        print( "<body>" );
                    }
                    print( "<form method=\"post\" action=\"" + cgi_name + "?update=" + lang + "&author=" + author + "\">" );
                    print( "<div class=\"top\"><br>&nbsp;&nbsp;<a href=\"" + cgi_name + "?start=0&author=" + author + "\">" + project_name + "</a>&gt;&gt;" + lang + "</div>" );
                    print( "<div align=\"center\">" );
                    print( "<table border=0 cellspacing=0 width=\"100%\">" );
                    print( "  <tr>" );
                    print( "    <td class=\"header\">English</td>" );
                    print( "    <td class=\"header\">translation</td>" );
                    print( "  </tr>" );
                    Messaging.setLanguage( lang );
                    MessageBody mb = new MessageBody( lang, Path.Combine( project_name, lang + ".po" ) );
                    if ( command0 == "update" ) {
                        string[] spl = stdin.Split( '&' );
                        foreach ( string s in spl ) {
                            string[] spl2 = s.Split( '=' );
                            if ( spl2.Length >= 2 ) {
                                string id = dec_b64( spl2[0] );
                                string value = dec_url( spl2[1] ).Replace( "\\n", "\n" );
                                if ( mb.list.ContainsKey( id ) ) {
                                    if ( keys.Contains( id ) ) {
                                        string old = mb.list[id].message;
                                        mb.list[id].message = value;
                                        if ( old != value ) {
                                            logger.WriteLine( "    replaced: {id,value,old_value}={" + id + "," + value + "," + old + "}" );
                                        }
                                    } else {
                                        mb.list.Remove( id );
                                    }
                                } else if ( keys.Contains( id ) ) {
                                    mb.list.Add( id, new MessageBodyEntry( value, new string[]{} ) );
                                    logger.WriteLine( "    added: {id,value}={" + id + "," + value + "}" );
                                }
                            }
                        }
                        mb.write( Path.Combine( project_name, lang + ".po" ) );
                    }
                    int count = -1;
                    foreach ( string key in keys ) {
                        count++;
                        string class_kind = "\"even\"";
                        if ( count % 2 != 0 ) {
                            class_kind = "\"odd\"";
                        }
                        string id = enc_b64( key );
                        print( "  <tr>" );
                        print( "    <td class=" + class_kind + ">" + key.Replace( "\n", "\\" + "n" ) + "</td>" );
                        string msg = mb.getMessage( key );
                        print( "    <td nowrap class=" + class_kind + ">" );
                        if ( msg == key ) {
                            print( "      <input type=\"text\" name=\"" + id + "\" class=\"highlight\" size=60 value=\"" + msg.Replace( "\n", "\\" + "n" ) + "\">" );
                        } else {
                            print( "      <input type=\"text\" name=\"" + id + "\" size=60 value=\"" + msg.Replace( "\n", "\\" + "n" ) + "\">" );
                        }
                        print( "      <input type=\"submit\" value=\"submit\">" );
                        print( "    </td>" );
                        print( "  </tr>" );
                    }
                    print( "</table>" );
                    print( "</div>" );
                    print( "</form>" );
                    print( "</body>" );
                    print( "</html>" );
                }
            } catch ( Exception exm ) {
                logger.WriteLine( DateTime.Now + " error; exm=" + exm );
                print( "<head><title>" + app_name + ": Error</title></head>" );
                print( "<body>" );
                print( "</body>" );
                print( "</html>" );
            } finally {
                if ( logger != null ) {
                    logger.Close();
                }
            }
        }

        static string translate( string source, string source_language, string result_language ) {
            string url = "http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=" + System.Web.HttpUtility.UrlEncode( source ) + "&langpair=" + source_language + "%7C" + result_language;
            System.Net.WebRequest request = System.Net.HttpWebRequest.Create( url );
            string lang_query = "";
            using ( System.Net.WebResponse response = request.GetResponse() ) {
                System.IO.Stream responseStream = response.GetResponseStream();
                using ( System.IO.StreamReader reader = new System.IO.StreamReader( responseStream ) ) {
                    string json = reader.ReadToEnd();
                    //{"responseData": {"translatedText":"ciao mondo"}, "responseDetails": null, "responseStatus": 200}
                    int index = json.IndexOf( "translatedText" );
                    if ( index >= 0 ) {
                        json = json.Substring( index );
                        int index_collon = json.IndexOf( ":" );
                        int index_dquote = json.IndexOf( "\"", index_collon );
                        int index_dquote2 = json.IndexOf( "\"", index_dquote + 1 );
                        lang_query = json.Substring( index_dquote + 1, index_dquote2 - index_dquote - 1 );
                        int index_esc = lang_query.IndexOf( "\\u0026#" );
                        while ( index_esc >= 0 ) {
                            List<char> digits = new List<char>();
                            for ( int i = index_esc + 7; i < lang_query.Length; i++ ) {
                                char c = lang_query[i];
                                if ( char.IsDigit( c ) ) {
                                    digits.Add( c );
                                } else {
                                    break;
                                }
                            }
                            string s_digits = "";
                            for ( int i = 0; i < digits.Count; i++ ) {
                                s_digits += digits[i].ToString();
                            }
                            int num = int.Parse( s_digits );
                            Console.WriteLine( "num=" + num );
                            char newc = (char)num;
                            string s = lang_query.Substring( 0, index_esc ) + newc.ToString();
                            if ( index_esc + 7 + digits.Count < lang_query.Length ) {
                                s += lang_query.Substring( index_esc + 7 + digits.Count );
                            }
                            lang_query = s;
                            index_esc = lang_query.IndexOf( "\\u0026#" );
                        }
                    }
                }
            }
            return lang_query;
        }

        static string dec_url( string s ) {
            return System.Web.HttpUtility.UrlDecode( s, Encoding.UTF8 );
        }

        static string dec_b64( string s ) {
            s = s.Replace( '_', '=' );
            byte[] b = bocoree.Base64.decode( s );
            return Encoding.UTF8.GetString( b );
        }

        static string enc_b64( string s ) {
            byte[] b = Encoding.UTF8.GetBytes( s );
            string ret = bocoree.Base64.encode( b );
            return ret.Replace( '=', '_' );
        }

        static void print( string s ) {
            Console.WriteLine( s );
        }
    }

}
