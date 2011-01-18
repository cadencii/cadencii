using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

class Resampler{
    public static void Main( string[] args ){
        string arg = "";
        foreach( string s in args ){
            arg += "\"" + s + "\" ";
        }
        string exe = Environment.GetCommandLineArgs()[0];
        string exe_name = Path.GetFileNameWithoutExtension( exe );
        string path = Application.StartupPath;
        if( exe_name == "resampler" && args.Length >= 12 ){
            string outpath = args[1];
            string out_name = Path.GetFileNameWithoutExtension( outpath );
            string out_dir = Path.GetDirectoryName( outpath );
            string logname = Path.Combine( out_dir, out_name + ".log" );
            using( StreamWriter sw = new StreamWriter( logname ) ){
                const int PBTYPE = 5;
                int indx_q = args[11].IndexOf( "Q" );
                int count = 0;
                if( indx_q > 0 ){
                    string pit = args[11].Substring( 0, indx_q );
                    if( args[11].Length >= indx_q + 1 ){
                        string tempo = args[11].Substring( indx_q + 1 );
                        sw.WriteLine( "# " + tempo );
                    }
                    sw.WriteLine( (count * PBTYPE) + "\t" + pit );
                    count++;
                }
                for( int i = 12; i < args.Length; i++ ){
                    sw.WriteLine( (count * PBTYPE) + "\t" + args[i] );
                    count++;
                }
            }
        }
        using( StreamWriter sw = new StreamWriter( Path.Combine( path, exe_name + ".log" ), true ) ){
            sw.WriteLine( arg );
            Process p = null;
            try{
                p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Path.Combine( path, "_" + exe_name + ".exe" );
                psi.Arguments = arg;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                psi.UseShellExecute = false;
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
            }catch( Exception ex ){
                sw.WriteLine( "Resampler.Main(string[]); ex=" + ex );
            }finally{
                if( p != null ){
                    try{
                        p.Dispose();
                    }catch{
                    }
                }
            }
        }
    }
}
