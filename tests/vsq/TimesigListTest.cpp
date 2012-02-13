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

    CPPUNIT_TEST_SUITE( TimesigListTest );
    CPPUNIT_TEST( testUpdateTimesigInfo );
    CPPUNIT_TEST_SUITE_END();
};

REGISTER_TEST_SUITE( TimesigListTest );
