package pp_cs2java;

import static org.junit.Assert.*;

import org.junit.Test;

public class ReplacementTest {

	@Test
	public void testNotWord() {
		Replacement replacement = new Replacement( "string", "String", false );
	    assertEquals( 0, replacement.findFrom( "string", 0 ) );
	    assertEquals( -1, replacement.findFrom( "string", 1 ) );
	}

	@Test
	public void testWordMatch()
	{
		Replacement replacement = new Replacement( "bool", "boolean", true );
		assertEquals( 0, replacement.findFrom( "bool", 0 ) );
		assertEquals( -1, replacement.findFrom( "boolean", 0 ) );
		assertEquals( 1, replacement.findFrom( "(bool)", 0 ) );
		
		replacement = new Replacement( "bool", "boolean", true );
		assertEquals( -1, replacement.findFrom( "        public boolean BandNewRowTool = false;", 0 ) );
	}
	
}
