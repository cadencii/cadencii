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
