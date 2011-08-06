#ifndef FORMWORDDICTIONARYUIIMPL_H
#define FORMWORDDICTIONARYUIIMPL_H

#include <QDialog>

#include "UiBase.h"
#include "FormWordDictionaryUi.h"
#include "FormWordDictionaryUiListener.h"
#include <string>

namespace Ui {
    class FormWordDictionaryUiImpl;
}

using namespace org::kbinani::cadencii;
using namespace std;

class FormWordDictionaryUiImpl : public QDialog, public FormWordDictionaryUi {
    Q_OBJECT
public:
    FormWordDictionaryUiImpl( QWidget *parent = 0 );
    ~FormWordDictionaryUiImpl();
    void setTitle( string value );
    void setDialogResult( bool value );
    void setSize( int width, int height );
    int getWidth();
    int getHeight();
    void setLocation( int x, int y );
    void close();
    int listDictionariesGetSelectedRow();
    int listDictionariesGetItemCountRow();
    void listDictionariesClear();
    string listDictionariesGetItemAt( int row );
    bool listDictionariesIsRowChecked( int row );
    void listDictionariesSetItemAt( int row, string value );
    void listDictionariesSetRowChecked( int row, bool value );
    void listDictionariesSetSelectedRow( int row );
    void listDictionariesClearSelection();
    void listDictionariesAddRow( string value, bool selected );
    void labelAvailableDictionariesSetText( string value );
    void buttonOkSetText( string value );
    void buttonCancelSetText( string value );
    void buttonUpSetText( string value );
    void buttonDownSetText( string value );

protected:
    void changeEvent( QEvent *e );

private:
    Ui::FormWordDictionaryUiImpl *ui;
    bool dialogResult;
};

#endif // FORMWORDDICTIONARYUIIMPL_H
