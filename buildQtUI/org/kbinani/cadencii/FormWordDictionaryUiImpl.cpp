#include "FormWordDictionaryUiImpl.h"
#include "ui_FormWordDictionaryUiImpl.h"

FormWordDictionaryUiImpl::FormWordDictionaryUiImpl(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::FormWordDictionaryUiImpl)
{
    ui->setupUi(this);
}

FormWordDictionaryUiImpl::~FormWordDictionaryUiImpl()
{
    delete ui;
}

void FormWordDictionaryUiImpl::changeEvent(QEvent *e)
{
    QDialog::changeEvent(e);
    switch (e->type()) {
    case QEvent::LanguageChange:
        ui->retranslateUi(this);
        break;
    default:
        break;
    }
}

void FormWordDictionaryUiImpl::setTitle(string value)
{
    QDialog::setWindowTitle( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::setDialogResult(bool value)
{
    dialogResult = value;
    close();
}

void FormWordDictionaryUiImpl::setSize(int width, int height)
{
    QDialog::setGeometry( QDialog::x(), QDialog::y(), width, height );
}

int FormWordDictionaryUiImpl::getWidth()
{
    return QDialog::width();
}

int FormWordDictionaryUiImpl::getHeight()
{
    return QDialog::height();
}

void FormWordDictionaryUiImpl::setLocation(int x, int y)
{
    QDialog::setGeometry( x, y, QDialog::width(), QDialog::height() );
}

void FormWordDictionaryUiImpl::close()
{
    QDialog::close();
}

int FormWordDictionaryUiImpl::listDictionariesGetSelectedRow()
{
    return ui->listDictionaries->selectionModel()->currentIndex().row();
}

int FormWordDictionaryUiImpl::listDictionariesGetItemCountRow()
{
    return ui->listDictionaries->count();
}

void FormWordDictionaryUiImpl::listDictionariesClear()
{
    ui->listDictionaries->clear();
}

string FormWordDictionaryUiImpl::listDictionariesGetItemAt(int row)
{
    QListWidgetItem *item = ui->listDictionaries->item( row );
    return item->text().toStdString();
}

bool FormWordDictionaryUiImpl::listDictionariesIsRowChecked(int row)
{
    QListWidgetItem *item = ui->listDictionaries->item( row );
    return item->checkState() == Qt::Checked;
}

void FormWordDictionaryUiImpl::listDictionariesSetItemAt(int row, string value)
{
    QListWidgetItem *item = ui->listDictionaries->item( row );
    item->setText( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::listDictionariesSetRowChecked(int row, bool value)
{
    QListWidgetItem *item = ui->listDictionaries->item( row );
    item->setCheckState( value ? Qt::Checked : Qt::Unchecked );
}

void FormWordDictionaryUiImpl::listDictionariesSetSelectedRow(int row)
{
    ui->listDictionaries->setCurrentRow( row );
}

void FormWordDictionaryUiImpl::listDictionariesClearSelection()
{
    ui->listDictionaries->clearSelection();
}

void FormWordDictionaryUiImpl::listDictionariesAddRow(string value, bool selected)
{
    ui->listDictionaries->addItem( QString( value.c_str() ) );
    QListWidgetItem *item = ui->listDictionaries->item( ui->listDictionaries->count() - 1 );
    Qt::ItemFlags flags = item->flags() | Qt::ItemIsUserCheckable;
    item->setFlags( flags );
    item->setCheckState( selected ? Qt::Checked : Qt::Unchecked );
}

void FormWordDictionaryUiImpl::labelAvailableDictionariesSetText(string value)
{
    ui->labelAvailableDictionaries->setText( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::buttonOkSetText(string value)
{
    ui->buttonOk->setText( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::buttonCancelSetText(string value)
{
    ui->buttonCancel->setText( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::buttonUpSetText(string value)
{
    ui->buttonUp->setText( QString( value.c_str() ) );
}

void FormWordDictionaryUiImpl::buttonDownSetText(string value)
{
    ui->buttonDown->setText( QString( value.c_str() ) );
}
