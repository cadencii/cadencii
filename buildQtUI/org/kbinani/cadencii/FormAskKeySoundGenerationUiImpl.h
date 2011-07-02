#ifndef FORMASKKEYSOUNDGENERATIONUIIMPL_H
#define FORMASKKEYSOUNDGENERATIONUIIMPL_H

#include <QDialog>
#include <string>

#include "UiBase.h"
#include "FormAskKeySoundGenerationUi.h"
#include "FormAskKeySoundGenerationUiListener.h"

namespace Ui {
    class FormAskKeySoundGenerationUiImpl;
}

using namespace std;
using namespace org::kbinani::cadencii;

class FormAskKeySoundGenerationUiImpl : public QDialog, public org::kbinani::cadencii::FormAskKeySoundGenerationUi
{
    Q_OBJECT
public:
    FormAskKeySoundGenerationUiImpl(FormAskKeySoundGenerationUiListener *listener, QWidget *parent = 0);
    ~FormAskKeySoundGenerationUiImpl();

    void setAlwaysPerformThisCheck( bool value );

    bool isAlwaysPerformThisCheck();

    void close( bool value );

    void setMessageLabelText( string value );

    void setAlwaysPerformThisCheckCheckboxText( string value );

    void setYesButtonText( string value );

    void setNoButtonText( string value );

    int showDialog( QObject *parentForm );

public slots:
    void receiveButtonCancelClicked();

    void receiveButtonOkClicked();

protected:
    void changeEvent(QEvent *e);

private:
    int mDialogResult;
    FormAskKeySoundGenerationUiListener *listener;
    Ui::FormAskKeySoundGenerationUiImpl *ui;
};

#endif // FORMASKKEYSOUNDGENERATIONUIIMPL_H
