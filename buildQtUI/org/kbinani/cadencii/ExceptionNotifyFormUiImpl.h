#ifndef EXCEPTIONNOTIFYFORMUIIMPL_H
#define EXCEPTIONNOTIFYFORMUIIMPL_H

#include <QDialog>

namespace Ui {
    class ExceptionNotifyFormUiImpl;
}

class ExceptionNotifyFormUiImpl : public QDialog
{
    Q_OBJECT

public:
    explicit ExceptionNotifyFormUiImpl(QWidget *parent = 0);
    ~ExceptionNotifyFormUiImpl();

private:
    Ui::ExceptionNotifyFormUiImpl *ui;
};

#endif // EXCEPTIONNOTIFYFORMUIIMPL_H
