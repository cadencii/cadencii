package pp_cs2java;

import static org.junit.Assert.*;

import org.junit.Test;

public class EvaluatorTest extends Evaluator{

    @Test
    public void testEval() throws Exception{
        assertEquals( false, Evaluator.eval( "A", new String[]{} ) );
        assertEquals( true, Evaluator.eval( "A", new String[]{ "A" } ) );
        assertEquals( true, Evaluator.eval( "!!A", new String[]{ "A" } ) );
        assertEquals( true, Evaluator.eval( "A&&(B||C)", new String[]{ "A", "B" } ) );
        assertEquals( false, Evaluator.eval( "A&&!(B||C)", new String[]{ "A", "B" } ) );
        assertEquals( true, Evaluator.eval( "ABCDEFG", new String[]{ "ABCDEFG" } ) );
    }

    @Test
    public void testEvalWithInvalidCharacter(){
        try{
            Evaluator.eval( "わ", new String[]{} );
            fail();
        }catch( Exception ex ){
            return;
        }
    }

    @Test
    public void testEvalWithInvalidEquation(){
        try{
            Evaluator.eval( "(", new String[]{} );
        }catch( Exception ex ){
            return;
        }
        fail();
    }

    @Test
    public void isValidECharacters(){
        assertTrue( Evaluator.isValidCharacters( "(equation)" ) );
        assertFalse( Evaluator.isValidCharacters( "@" ) );
    }

    @Test
    public void isValidBooleanExpression(){
        assertTrue( Evaluator.isValidBooleanExpression( "A&&B||C" ) );
        assertFalse( Evaluator.isValidBooleanExpression( "A|B" ) );
    }

    @Test
    public void isValidBrackets(){
        assertTrue( Evaluator.isValidBrackets( "((A))" ) );
        assertFalse( Evaluator.isValidBrackets( "((A)" ) );
        assertFalse( Evaluator.isValidBrackets( "()" ) );
    }

    @Test
    public void isValidDirectiveName(){
        assertTrue( Evaluator.isValidDirectiveName( "(A)" ) );
        assertFalse( Evaluator.isValidDirectiveName( "0A" ) );
        assertTrue( Evaluator.isValidDirectiveName( "A!_0" ) );
    }
}
