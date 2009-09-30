#include "libvsq.h"
#include <iostream>

using namespace std;
using namespace vsq;

int main(){
    TextMemoryStream tms;
    tms.WriteLine( "foo" );
    tms.WriteLine( "bar" );
    tms.Rewind();
    while( tms.Peek() >= 0 ){
        cout << tms.ReadLine() << endl;
    }
    tms.Close();
    return 0;
}
