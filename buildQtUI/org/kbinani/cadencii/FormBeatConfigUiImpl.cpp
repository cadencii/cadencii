#include "FormBeatConfigUiImpl.h"
#include "ui_FormBeatConfigUiImpl.h"

FormBeatConfigUiImpl::FormBeatConfigUiImpl(FormBeatConfigUiListener *listener, QWidget *parent) :
    QDialog(parent),
    ui(new Ui::FormBeatConfigUiImpl)
{
    this->listener = listener;
    ui->setupUi(this);
    connect( ui->btnOK, SIGNAL(clicked()), this, SLOT(receiveButtonOkClicked()) );
    connect( ui->btnCancel, SIGNAL(clicked()), this, SLOT(receiveButtonCancelClicked()) );
    connect( ui->chkEnd, SIGNAL(stateChanged(int)), this, SLOT(receiveCheckboxEndStateChanged(int)) );
}

FormBeatConfigUiImpl::~FormBeatConfigUiImpl()
{
    delete ui;
}

void FormBeatConfigUiImpl::changeEvent(QEvent *e)
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

void FormBeatConfigUiImpl::setFont( string fontName, float fontSize ){
    //TODO:
}

void FormBeatConfigUiImpl::setTitle( string value ){
    QDialog::setWindowTitle( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setDialogResult( bool value ){
    dialogResult = value;
    close();
}

void FormBeatConfigUiImpl::setLocation( int x, int y ){
    QDialog::setGeometry( x, y, QDialog::width(), QDialog::height() );
}

int FormBeatConfigUiImpl::getWidth(){
    return QDialog::width();
}

int FormBeatConfigUiImpl::getHeight(){
    return QDialog::height();
}

void FormBeatConfigUiImpl::close(){
    QDialog::close();
}

void FormBeatConfigUiImpl::setTextBar1Label( string value ){
    ui->lblBar1->setText( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setTextBar2Label( string value ){
    ui->lblBar2->setText( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setTextStartLabel( string value ){
    ui->lblStart->setText( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setTextOkButton( string value ){
    ui->btnOK->setText( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setTextCancelButton( string value ){
    ui->btnCancel->setText( QString( value.c_str() ) ) ;
}

void FormBeatConfigUiImpl::setTextBeatGroup( string value ){
    ui->groupBeat->setTitle( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setTextPositionGroup( string value ){
    ui->groupPosition->setTitle( QString( value.c_str() ) );
}

void FormBeatConfigUiImpl::setEnabledStartNum( bool value ){
    ui->numStart->setEnabled( value );
}

void FormBeatConfigUiImpl::setMinimumStartNum( int value ){
    ui->numStart->setMinimum( value );
}

void FormBeatConfigUiImpl::setMaximumStartNum( int value ){
    ui->numStart->setMaximum( value );
}

int FormBeatConfigUiImpl::getMaximumStartNum(){
    return ui->numStart->maximum();
}

int FormBeatConfigUiImpl::getMinimumStartNum(){
    return ui->numStart->minimum();
}

void FormBeatConfigUiImpl::setValueStartNum( int value ){
    ui->numStart->setValue( value );
}

int FormBeatConfigUiImpl::getValueStartNum(){
    return ui->numStart->value();
}

void FormBeatConfigUiImpl::setEnabledEndNum( bool value ){
    ui->numEnd->setEnabled( value );
}

void FormBeatConfigUiImpl::setMinimumEndNum( int value ){
    ui->numEnd->setMinimum( value );
}

void FormBeatConfigUiImpl::setMaximumEndNum( int value ){
    ui->numEnd->setMaximum( value );
}

int FormBeatConfigUiImpl::getMaximumEndNum(){
    return ui->numEnd->maximum();
}

int FormBeatConfigUiImpl::getMinimumEndNum(){
    return ui->numEnd->minimum();
}

void FormBeatConfigUiImpl::setValueEndNum( int value ){
    ui->numEnd->setValue( value );
}

int FormBeatConfigUiImpl::getValueEndNum(){
    return ui->numEnd->value();
}

bool FormBeatConfigUiImpl::isCheckedEndCheckbox(){
    return (ui->chkEnd->checkState() == Qt::Checked);
}

void FormBeatConfigUiImpl::setEnabledEndCheckbox( bool value ){
    ui->chkEnd->setEnabled( value );
}

bool FormBeatConfigUiImpl::isEnabledEndCheckbox(){
    return ui->chkEnd->isEnabled();
}

void FormBeatConfigUiImpl::setTextEndCheckbox( string value ){
    ui->chkEnd->setText( QString( value.c_str() ) ) ;
}

void FormBeatConfigUiImpl::removeAllItemsDenominatorCombobox(){
    //TODO:これあってる？
    ui->comboDenominator->clear();
}

void FormBeatConfigUiImpl::addItemDenominatorCombobox( string value ){
    //TODO:これであってる？
    ui->comboDenominator->addItem( QString( value.c_str() ), QVariant( value.c_str() ) );
}

void FormBeatConfigUiImpl::setSelectedIndexDenominatorCombobox( int value ){
    ui->comboDenominator->setCurrentIndex( value );
}

int FormBeatConfigUiImpl::getSelectedIndexDenominatorCombobox(){
    return ui->comboDenominator->currentIndex();
}

int FormBeatConfigUiImpl::getMaximumNumeratorNum(){
    return ui->numNumerator->maximum();
}

int FormBeatConfigUiImpl::getMinimumNumeratorNum(){
    return ui->numNumerator->minimum();
}

void FormBeatConfigUiImpl::setValueNumeratorNum( int value ){
    ui->numNumerator->setValue( value );
}

int FormBeatConfigUiImpl::getValueNumeratorNum(){
    return ui->numNumerator->value();
}

int FormBeatConfigUiImpl::showDialog( QObject *parentForm ){
    if( parentForm ){
        QWidget *newParent = dynamic_cast<QWidget *>( parentForm );
        if( newParent ){
            this->setParent( newParent );
        }
    }
    return QDialog::exec();
}

void FormBeatConfigUiImpl::receiveButtonOkClicked(){
    if( listener ){
        listener->buttonOkClickedSlot();
    }
}

void FormBeatConfigUiImpl::receiveButtonCancelClicked(){
    if( listener ){
        listener->buttonCancelClickedSlot();
    }
}

void FormBeatConfigUiImpl::receiveCheckboxEndStateChanged(int value){
    if( listener ){
        listener->checkboxEndCheckedChangedSlot();
    }
}
