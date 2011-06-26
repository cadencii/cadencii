#include <QObject>
#include <QString>
#include <QPainter>
#include <QtGui/QApplication>
#include <typeinfo>
#include <iostream>

#include "org/kbinani/cadencii/UiBase.h"
using namespace org::kbinani::cadencii;
#include "org/kbinani/cadencii/ControllerBase.h"
#include "org/kbinani/cadencii/FormAskKeySoundGenerationUiListener.h"
#include "org/kbinani/cadencii/FormAskKeySoundGenerationUiImpl.h"
#include "org/kbinani/cadencii/WaveformZoomUiListener.h"
#include "org/kbinani/cadencii/WaveformZoomUi.h"

int main( int argc, char*argv[] )
{
    QApplication a( argc, argv );
    FormAskKeySoundGenerationUiImpl view;
    view.show();
    view.setAlwaysPerformThisCheck( false );
    return a.exec();
}
