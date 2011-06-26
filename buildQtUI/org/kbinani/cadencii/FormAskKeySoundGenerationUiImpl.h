#ifndef FORMASKKEYSOUNDGENERATIONUIIMPL_H
#define FORMASKKEYSOUNDGENERATIONUIIMPL_H

#include <QDialog>

#include "UiBase.h"
#include "FormAskKeySoundGenerationUi.h"

namespace Ui {
    class FormAskKeySoundGenerationUiImpl;
}

class FormAskKeySoundGenerationUiImpl : public QDialog, public org::kbinani::cadencii::FormAskKeySoundGenerationUi
{
    Q_OBJECT
public:
    FormAskKeySoundGenerationUiImpl(QWidget *parent = 0);
    ~FormAskKeySoundGenerationUiImpl();

    void setAlwaysPerformThisCheck( bool value );

    bool isAlwaysPerformThisCheck();

    void close( bool value );

    void setMessageLabelText( QString value );

    void setAlwaysPerformThisCheckCheckboxText( QString value );

    void setYesButtonText( QString value );

    void setNoButtonText( QString value );

    int showDialog( QObject *parent_form );

protected:
    void changeEvent(QEvent *e);

private:
    int mDialogResult;
    Ui::FormAskKeySoundGenerationUiImpl *ui;
};

#endif // FORMASKKEYSOUNDGENERATIONUIIMPL_H
