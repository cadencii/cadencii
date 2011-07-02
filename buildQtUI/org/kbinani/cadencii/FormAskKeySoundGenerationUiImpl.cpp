#include "FormAskKeySoundGenerationUiImpl.h"
#include "ui_FormAskKeySoundGenerationUiImpl.h"

FormAskKeySoundGenerationUiImpl::FormAskKeySoundGenerationUiImpl(QWidget *parent) :
    QDialog(parent),
    ui( new Ui::FormAskKeySoundGenerationUiImpl ){
    ui->setupUi( this );
    mDialogResult = 0;

    connect( ui->btnYes, SIGNAL(clicked()), this, SLOT(receiveButtonOkClicked()) );
    connect( ui->btnNo, SIGNAL(clicked()), this, SLOT(receiveButtonCancelClicked()) );
}

FormAskKeySoundGenerationUiImpl::~FormAskKeySoundGenerationUiImpl(){
    delete ui;
}

void FormAskKeySoundGenerationUiImpl::changeEvent( QEvent *e ){
    QDialog::changeEvent(e);
    switch (e->type()) {
        case QEvent::LanguageChange:{
            ui->retranslateUi(this);
            break;
        }
        default:{
            break;
        }
    }
}

void FormAskKeySoundGenerationUiImpl::setAlwaysPerformThisCheck( bool value ){
    ui->chkAlwaysPerformThisCheck->setChecked( value );
}

bool FormAskKeySoundGenerationUiImpl::isAlwaysPerformThisCheck(){
    return ui->chkAlwaysPerformThisCheck->isChecked();
}

void FormAskKeySoundGenerationUiImpl::close( bool value ){
    mDialogResult = value ? 0 : 1;
    this->QDialog::close();
}

void FormAskKeySoundGenerationUiImpl::setMessageLabelText( QString value ){
    ui->lblMessage->setText( value );
}

void FormAskKeySoundGenerationUiImpl::setAlwaysPerformThisCheckCheckboxText( QString value ){
    ui->chkAlwaysPerformThisCheck->setText( value );
}

void FormAskKeySoundGenerationUiImpl::setYesButtonText( QString value ){
    ui->btnYes->setText( value );
}

void FormAskKeySoundGenerationUiImpl::setNoButtonText( QString value ){
    ui->btnNo->setText( value );
}

int FormAskKeySoundGenerationUiImpl::showDialog( QObject *parent_form ){
    if( parent_form ){
        QWidget *newParent = dynamic_cast<QWidget *>( parent_form );
        if( newParent ){
            this->setParent( newParent );
        }
    }
    return this->exec();
}

void FormAskKeySoundGenerationUiImpl::receiveButtonCancelClicked(){

}

void FormAskKeySoundGenerationUiImpl::receiveButtonOkClicked(){
}
