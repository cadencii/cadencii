package pp_cs2java;

class str
{
    public static boolean compare( String a, String b )
    {
        if ( a == null || b == null ) {
            return false;
        }
        return a.equals( b );
    }
}
