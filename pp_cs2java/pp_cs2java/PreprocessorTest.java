package pp_cs2java;

import static org.junit.Assert.*;

import java.io.File;
import java.io.FileInputStream;

import org.junit.Test;

public class PreprocessorTest{

    @Test
    public void testAppManager() throws Exception{
        System.out.println( "PreprocessorTest#testAppManager" );
        String[] args =
            new String[]{ "-i", "test/data/Preprocessor_AppManager.cs", "-o",
                "/tmp/Preprocessor.tmp", "-DCLIPBOARD_AS_TEXT" };
        Preprocessor.main( args );

        byte[] actual = loadFile( "/tmp/Preprocessor.tmp" );
        byte[] expected = loadFile( "test/expected/Preprocessor_AppManager.cs" );
        assertArrayEquals( expected, actual );
    }

    @Test
    public void testNested() throws Exception{
        System.out.println( "PreprocessorTest#testNested" );
        String[] args =
            new String[]{ "-i", "test/data/Preprocessor_Nested.cs", "-o",
                "/tmp/Preprocessor.tmp" };
        Preprocessor.main( args );

        byte[] actual = loadFile( "/tmp/Preprocessor.tmp" );
        byte[] expected = loadFile( "test/expected/Preprocessor_Nested.cs" );
        assertArrayEquals( expected, actual );
    }

    @Test
    public void testSimpleA() throws Exception{
        System.out.println( "PreprocessorTest#testSimpleA" );
        String[] args =
            new String[]{ "-i", "test/data/Preprocessor_Simple.cs", "-o",
                "/tmp/Preprocessor.tmp" };
        Preprocessor.main( args );

        byte[] actual = loadFile( "/tmp/Preprocessor.tmp" );
        byte[] expected = loadFile( "test/expected/Preprocessor_Simple_A.cs" );
        assertArrayEquals( expected, actual );
    }

    @Test
    public void testSimpleB() throws Exception{
        System.out.println( "PreprocessorTest#testSimpleB" );
        String[] args =
            new String[]{ "-i", "test/data/Preprocessor_Simple.cs", "-o",
                "/tmp/Preprocessor.tmp", "-DD" };
        Preprocessor.main( args );

        byte[] actual = loadFile( "/tmp/Preprocessor.tmp" );
        byte[] expected = loadFile( "test/expected/Preprocessor_Simple_B.cs" );
        assertArrayEquals( expected, actual );
    }

    /**
     * 指定されたファイルを読み込み，その内容をバイト列として返します． ファイルが存在しないなどの理由で読み込みに失敗した場合，例外をスローします．
     * 
     * @param path
     *            読み込むファイルのパス．
     * @return ファイルの内容を格納した配列．
     */
    private static byte[] loadFile(
        String path ) throws Exception{
        File f = new File( path );
        byte[] ret = new byte[(int)f.length()];
        FileInputStream fs = new FileInputStream( path );
        fs.read( ret );
        fs.close();
        return ret;
    }

}
