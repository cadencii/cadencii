#include <QObject>
#include <QString>
#include <QPainter>
#include <QtGui/QApplication>
#include <typeinfo>
#include <iostream>
#include <stdio.h>

#include "org/kbinani/cadencii/UiBase.h"
using namespace org::kbinani::cadencii;
#include "org/kbinani/cadencii/ControllerBase.h"

#include "org/kbinani/cadencii/FormAskKeySoundGenerationUiListener.h"
#include "org/kbinani/cadencii/FormAskKeySoundGenerationUiImpl.h"
void FormAskKeySoundGenerationUiListener::buttonCancelClickedSlot(){}
void FormAskKeySoundGenerationUiListener::buttonOkClickedSlot(){}

#include "org/kbinani/cadencii/WaveformZoomUiListener.h"
#include "org/kbinani/cadencii/WaveformZoomUi.h"

#include "org/kbinani/cadencii/FormBeatConfigUiListener.h"
#include "org/kbinani/cadencii/FormBeatConfigUiImpl.h"
void FormBeatConfigUiListener::buttonCancelClickedSlot(){}
void FormBeatConfigUiListener::buttonOkClickedSlot(){}
void FormBeatConfigUiListener::checkboxEndCheckedChangedSlot(){}

#include "org/kbinani/cadencii/FormWordDictionaryUiListener.h"
#include "org/kbinani/cadencii/FormWordDictionaryUiImpl.h"

int UiBase::showDialog(QObject *parent_form){}

int main( int argc, char*argv[] )
{
    QApplication a( argc, argv );

    FormAskKeySoundGenerationUiImpl view( 0 );
    view.setAlwaysPerformThisCheck( false );
    //view.show();

    FormBeatConfigUiImpl formBeatConfigUiImpl( 0 );
    formBeatConfigUiImpl.addItemDenominatorCombobox( "foo" );
    //formBeatConfigUiImpl.show();

    FormWordDictionaryUiImpl formWordDictionaryUiImpl( 0 );
    formWordDictionaryUiImpl.listDictionariesAddRow( "foo", false );
    formWordDictionaryUiImpl.listDictionariesAddRow( "bar", true );
    formWordDictionaryUiImpl.listDictionariesSetSelectedRow( 1 );
    formWordDictionaryUiImpl.show();

    int ret = formBeatConfigUiImpl.showDialog( &view );
    string s = "";
    char buff[1024];
    sprintf( buff, "%d", ret );
    s = buff;
    s = "buf=" + s;
    qDebug( s.c_str() );

    return a.exec();
}
