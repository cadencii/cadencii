#include "Util.h"
#include "../../vsq/MeasureLineIterator.h"

using namespace VSQ_NS;

class MeasureLineIteratorTest : public CppUnit::TestCase
{
public:
    void test()
    {
        TimesigList list;
        list.push( Timesig( 4, 4, 0 ) );
        list.push( Timesig( 3, 4, 1 ) );
        list.updateTimesigInfo();
        MeasureLineIterator i( &list, 3360 );

        CPPUNIT_ASSERT( i.hasNext() );
        MeasureLine actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)0, actual.tick );
        CPPUNIT_ASSERT_EQUAL( true, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)480, actual.tick );
        CPPUNIT_ASSERT_EQUAL( false, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)960, actual.tick );
        CPPUNIT_ASSERT_EQUAL( false, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)1440, actual.tick );
        CPPUNIT_ASSERT_EQUAL( false, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)1920, actual.tick );
        CPPUNIT_ASSERT_EQUAL( true, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)2400, actual.tick );
        CPPUNIT_ASSERT_EQUAL( false, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)2880, actual.tick );
        CPPUNIT_ASSERT_EQUAL( false, actual.isBorder );

        CPPUNIT_ASSERT( i.hasNext() );
        actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)3360, actual.tick );
        CPPUNIT_ASSERT_EQUAL( true, actual.isBorder );

        CPPUNIT_ASSERT_EQUAL( false, i.hasNext() );
    }

    void testWithoutAnyBar()
    {
        TimesigList list;
        list.push( Timesig( 4, 4, 0 ) );
        list.push( Timesig( 3, 4, 1 ) );
        list.updateTimesigInfo();
        MeasureLineIterator i( &list, 479 );

        CPPUNIT_ASSERT_EQUAL( true, i.hasNext() );
        MeasureLine actual = i.next();
        CPPUNIT_ASSERT_EQUAL( (tick_t)0, actual.tick );
        CPPUNIT_ASSERT_EQUAL( true, actual.isBorder );

        CPPUNIT_ASSERT_EQUAL( false, i.hasNext() );
    }

    CPPUNIT_TEST_SUITE( MeasureLineIteratorTest );
    CPPUNIT_TEST( test );
    CPPUNIT_TEST( testWithoutAnyBar );
    CPPUNIT_TEST_SUITE_END();
};

REGISTER_TEST_SUITE( MeasureLineIteratorTest );
