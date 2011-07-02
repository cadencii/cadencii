#ifndef FORMBEATCONFIGUIIMPL_H
#define FORMBEATCONFIGUIIMPL_H

#include <QDialog>
#include <string>

#include "UiBase.h"
#include "FormBeatConfigUi.h"
#include "FormBeatConfigUiListener.h"

namespace Ui {
    class FormBeatConfigUiImpl;
}

using namespace std;
using namespace org::kbinani::cadencii;

class FormBeatConfigUiImpl : public QDialog, public FormBeatConfigUi {
    Q_OBJECT
public:
    FormBeatConfigUiImpl(FormBeatConfigUiListener *listener = 0, QWidget *parent = 0);
    ~FormBeatConfigUiImpl();

    void setFont( string fontName, float fontSize );

    void setTitle( string value );

    void setDialogResult( bool value );

    void setLocation( int x, int y );

    int getWidth();

    int getHeight();

    void close();

    void setTextBar1Label( string value );

    void setTextBar2Label( string value );

    void setTextStartLabel( string value );

    void setTextOkButton( string value );

    void setTextCancelButton( string value );

    void setTextBeatGroup( string value );

    void setTextPositionGroup( string value );

    void setEnabledStartNum( bool value );

    void setMinimumStartNum( int value );

    void setMaximumStartNum( int value );

    int getMaximumStartNum();

    int getMinimumStartNum();

    void setValueStartNum( int value );

    int getValueStartNum();

    void setEnabledEndNum( bool value );

    void setMinimumEndNum( int value );

    void setMaximumEndNum( int value );

    int getMaximumEndNum();

    int getMinimumEndNum();

    void setValueEndNum( int value );

    int getValueEndNum();

    bool isCheckedEndCheckbox();

    void setEnabledEndCheckbox( bool value );

    bool isEnabledEndCheckbox();

    void setTextEndCheckbox( string value );

    void removeAllItemsDenominatorCombobox();

    void addItemDenominatorCombobox( string value );

    void setSelectedIndexDenominatorCombobox( int value );

    int getSelectedIndexDenominatorCombobox();

    int getMaximumNumeratorNum();

    int getMinimumNumeratorNum();

    void setValueNumeratorNum( int value );

    int getValueNumeratorNum();

    int showDialog( QObject *parentForm );

public slots:
    void receiveButtonOkClicked();

    void receiveButtonCancelClicked();

    void receiveCheckboxEndStateChanged( int value );

protected:
    void changeEvent(QEvent *e);


private:
    Ui::FormBeatConfigUiImpl *ui;
    FormBeatConfigUiListener *listener;
    bool dialogResult;
};

#endif // FORMBEATCONFIGUIIMPL_H
