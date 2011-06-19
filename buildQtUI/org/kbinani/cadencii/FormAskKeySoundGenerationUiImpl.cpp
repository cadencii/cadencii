#include "FormAskKeySoundGenerationUiImpl.h"
#include "ui_FormAskKeySoundGenerationUiImpl.h"

FormAskKeySoundGenerationUiImpl::FormAskKeySoundGenerationUiImpl(QWidget *parent) :
    QFrame(parent),
    ui(new Ui::FormAskKeySoundGenerationUiImpl)
{
    ui->setupUi(this);
}

FormAskKeySoundGenerationUiImpl::~FormAskKeySoundGenerationUiImpl()
{
    delete ui;
}

void FormAskKeySoundGenerationUiImpl::changeEvent(QEvent *e)
{
    QFrame::changeEvent(e);
    switch (e->type()) {
    case QEvent::LanguageChange:
        ui->retranslateUi(this);
        break;
    default:
        break;
    }
}
