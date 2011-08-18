#include "ExceptionNotifyFormUiImpl.h"
#include "ui_ExceptionNotifyFormUiImpl.h"

ExceptionNotifyFormUiImpl::ExceptionNotifyFormUiImpl(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::ExceptionNotifyFormUiImpl)
{
    ui->setupUi(this);
}

ExceptionNotifyFormUiImpl::~ExceptionNotifyFormUiImpl()
{
    delete ui;
}
