#include "Util.h"
#include "../../vsq/TimesigList.h"

using namespace VSQ_NS;

class TimesigListTest : public CppUnit::TestCase
{
public:
    void testUpdateTimesigInfo()
    {
        TimesigList table;
        table.push( Timesig( 4, 4, 2 ) );
        table.push( Timesig( 4, 4, 0 ) );
        table.push( Timesig( 3, 4, 1 ) );
        table.updateTimesigInfo();

        CPPUNIT_ASSERT_EQUAL( (tick_t)0, table.get( 0 ).clock );
        CPPUNIT_ASSERT_EQUAL( 0, table.get( 0 ).barCount );
        CPPUNIT_ASSERT_EQUAL( (tick_t)1920, table.get( 1 ).clock );
        CPPUNIT_ASSERT_EQUAL( 1, table.get( 1 ).barCount );
        CPPUNIT_ASSERT_EQUAL( (tick_t)3360, table.get( 2 ).clock );
        CPPUNIT_ASSERT_EQUAL( 2, table.get( 2 ).barCount );
    }

    void testGetTimesigAt()
    {
        TimesigList table;
        table.push( Timesig( 4, 8, 2 ) );
        table.push( Timesig( 4, 4, 0 ) );
        table.push( Timesig( 3, 4, 1 ) );

        /*
         0                   1               2            3           4           5           6           7           8           9
         ‖4   |    |    |    ‖3    |    |    ‖4  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖
         ‖4   |    |    |    ‖4    |    |    ‖8  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖  |  |  |  ‖
         ^                   ^               ^            ^           ^           ^           ^           ^           ^           ^
         |                   |               |            |           |           |           |           |           |           |
         0                   1920            3360         4320        5280        6240        7200        8160        9120        10080
         */

        Timesig timesig = table.getTimesigAt( 480 );
        CPPUNIT_ASSERT_EQUAL( 4, timesig.numerator );
        CPPUNIT_ASSERT_EQUAL( 4, timesig.denominator );
        CPPUNIT_ASSERT_EQUAL( 0, timesig.barCount );

        timesig = table.getTimesigAt( 1920 );
        CPPUNIT_ASSERT_EQUAL( 3, timesig.numerator );
        CPPUNIT_ASSERT_EQUAL( 4, timesig.denominator );
        CPPUNIT_ASSERT_EQUAL( 1, timesig.barCount );

        timesig = table.getTimesigAt( 10000 );
        CPPUNIT_ASSERT_EQUAL( 4, timesig.numerator );
        CPPUNIT_ASSERT_EQUAL( 8, timesig.denominator );
        CPPUNIT_ASSERT_EQUAL( 8, timesig.barCount );
    }

    void testPushDuplicateKey()
    {
        TimesigList table;
        table.push( Timesig( 3, 4, 0 ) );
        table.push( Timesig( 4, 8, 0 ) );

        CPPUNIT_ASSERT_EQUAL( 1, table.size() );
        CPPUNIT_ASSERT_EQUAL( 4, table.get( 0 ).numerator );
        CPPUNIT_ASSERT_EQUAL( 8, table.get( 0 ).denominator );
    }

    void testGetClockFromBarCount()
    {
        TimesigList table;
        table.push( Timesig( 4, 6, 2 ) ); // 3360 clock開始
        table.push( Timesig( 4, 4, 0 ) ); //    0 clock開始
        table.push( Timesig( 3, 4, 1 ) ); // 1920 clock開始

        CPPUNIT_ASSERT_EQUAL( (tick_t)0, table.getClockFromBarCount( 0 ) );
        CPPUNIT_ASSERT_EQUAL( (tick_t)1920, table.getClockFromBarCount( 1 ) );
        CPPUNIT_ASSERT_EQUAL( (tick_t)3360, table.getClockFromBarCount( 2 ) );
        CPPUNIT_ASSERT_EQUAL( (tick_t)9760, table.getClockFromBarCount( 7 ) );
    }

    CPPUNIT_TEST_SUITE( TimesigListTest );
    CPPUNIT_TEST( testUpdateTimesigInfo );
    CPPUNIT_TEST( testGetTimesigAt );
    CPPUNIT_TEST( testPushDuplicateKey );
    CPPUNIT_TEST( testGetClockFromBarCount );
    CPPUNIT_TEST_SUITE_END();
};

REGISTER_TEST_SUITE( TimesigListTest );
