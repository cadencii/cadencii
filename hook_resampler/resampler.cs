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
        string path = Application.StartupPath;
        string log = Path.Combine( path, "resampler.log" );
        using( StreamWriter sw = new StreamWriter( log, true ) ){
            sw.WriteLine( arg );
            Process p = null;
            try{
                p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Path.Combine( path, "_resampler.exe" );
                psi.Arguments = arg;
                psi.CreateNoWindow = true;
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
