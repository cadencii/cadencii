using System;
using System.IO;
using System.Text.RegularExpressions;

// cMenuTrackSelectorDelete.setText( _( "Delete" ) + "(&D)" );
// のような行を、
// cMenuTrackSelectorDelete.setText( _( "Delete" ) );
// cMenuTrackSelectorDelete.setMnemonic( KeyEvent.VK_D );
// みたいに分解するツール
// テキストデータは標準入力から読み込む
//
// AutoMnemonic<in.txt>out.txt
// のように使用
class AutoMnemonic {
    public static void Main( string[] args ) {
        TextReader input = Console.In;

        string line = "";
        Regex r = new Regex( @"(?<header>.*)[.]setText\(\s(?<text>.*)\s\+\s""\(\&(?<code>.)\)"".*" );
        while ( (line = input.ReadLine()) != null ) {
            Match m = r.Match( line );
            if ( m.Success ) {
                string header = m.Groups["header"].Value;
                string text = m.Groups["text"].Value;
                string code = m.Groups["code"].Value;
                Console.WriteLine( header + ".setText( " + text + " );" );
                Console.WriteLine( header + ".setMnemonic( KeyEvent.VK_" + code + " );" );
            }else{
                Console.WriteLine( line );
            }
        }
    }
}

