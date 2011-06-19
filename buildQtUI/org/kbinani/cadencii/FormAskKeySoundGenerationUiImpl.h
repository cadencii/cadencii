#ifndef FORMASKKEYSOUNDGENERATIONUIIMPL_H
#define FORMASKKEYSOUNDGENERATIONUIIMPL_H

#include <QFrame>

#include "FormAskKeySoundGenerationUi.h"

namespace Ui {
    class FormAskKeySoundGenerationUiImpl;
}

class FormAskKeySoundGenerationUiImpl : public QFrame, public org::kbinani::cadencii::FormAskKeySoundGenerationUi
{
    Q_OBJECT
public:
    FormAskKeySoundGenerationUiImpl(QWidget *parent = 0);
    ~FormAskKeySoundGenerationUiImpl();

    void setAlwaysPerformThisCheck( bool value )
    {
    }

    bool isAlwaysPerformThisCheck()
    {
    }

    void close( bool value )
    {
    }

    void setMessageLabelText( QString value )
    {
    }

    void setAlwaysPerformThisCheckCheckboxText( QString value )
    {
    }

    void setYesButtonText( QString value )
    {
    }

    void setNoButtonText( QString value )
    {
    }

protected:
    void changeEvent(QEvent *e);

private:
    Ui::FormAskKeySoundGenerationUiImpl *ui;
};

#endif // FORMASKKEYSOUNDGENERATIONUIIMPL_H
