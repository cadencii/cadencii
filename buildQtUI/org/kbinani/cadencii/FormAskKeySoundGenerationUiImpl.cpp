#include "FormAskKeySoundGenerationUiImpl.h"
#include "ui_FormAskKeySoundGenerationUiImpl.h"

void FormAskKeySoundGenerationUiListener::buttonCancelClickedSlot(){}
void FormAskKeySoundGenerationUiListener::buttonOkClickedSlot(){}

FormAskKeySoundGenerationUiImpl::FormAskKeySoundGenerationUiImpl(FormAskKeySoundGenerationUiListener *listener, QWidget *parent) :
    QDialog(parent),
    ui( new Ui::FormAskKeySoundGenerationUiImpl )
{
    this->listener = listener;
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

void FormAskKeySoundGenerationUiImpl::setMessageLabelText( string value ){
    ui->lblMessage->setText( QString( value.c_str() ) );
}

void FormAskKeySoundGenerationUiImpl::setAlwaysPerformThisCheckCheckboxText( string value ){
    ui->chkAlwaysPerformThisCheck->setText( QString( value.c_str() ) );
}

void FormAskKeySoundGenerationUiImpl::setYesButtonText( string value ){
    ui->btnYes->setText( QString( value.c_str() ) );
}

void FormAskKeySoundGenerationUiImpl::setNoButtonText( string value ){
    ui->btnNo->setText( QString( value.c_str() ) );
}

int FormAskKeySoundGenerationUiImpl::showDialog( QObject *parent_form ){
    if( parent_form ){
        QWidget *newParent = dynamic_cast<QWidget *>( parent_form );
        if( newParent ){
            this->setParent( newParent );
        }
    }
    return QDialog::exec();
}

void FormAskKeySoundGenerationUiImpl::receiveButtonCancelClicked(){
    if( listener ){
        listener->buttonCancelClickedSlot();
    }
}

void FormAskKeySoundGenerationUiImpl::receiveButtonOkClicked(){
    if( listener ){
        listener->buttonOkClickedSlot();
    }
}
