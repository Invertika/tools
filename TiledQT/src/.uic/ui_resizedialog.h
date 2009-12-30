/********************************************************************************
** Form generated from reading ui file 'resizedialog.ui'
**
** Created: Wed Dec 23 18:06:07 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_RESIZEDIALOG_H
#define UI_RESIZEDIALOG_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QDialog>
#include <QtGui/QDialogButtonBox>
#include <QtGui/QFrame>
#include <QtGui/QGridLayout>
#include <QtGui/QGroupBox>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QSpinBox>
#include <QtGui/QVBoxLayout>
#include "resizehelper.h"

QT_BEGIN_NAMESPACE

class Ui_ResizeDialog
{
public:
    QVBoxLayout *verticalLayout;
    QGroupBox *groupBox;
    QGridLayout *_6;
    QLabel *label_3;
    QSpinBox *widthSpinBox;
    QLabel *label_4;
    QSpinBox *heightSpinBox;
    QGroupBox *groupBox_2;
    QGridLayout *_4;
    QLabel *label_2;
    QSpinBox *offsetXSpinBox;
    QLabel *label;
    QSpinBox *offsetYSpinBox;
    QFrame *frame;
    QVBoxLayout *_5;
    Tiled::Internal::ResizeHelper *resizeHelper;
    QDialogButtonBox *buttonBox;

    void setupUi(QDialog *ResizeDialog)
    {
        if (ResizeDialog->objectName().isEmpty())
            ResizeDialog->setObjectName(QString::fromUtf8("ResizeDialog"));
        ResizeDialog->resize(242, 402);
        verticalLayout = new QVBoxLayout(ResizeDialog);
        verticalLayout->setObjectName(QString::fromUtf8("verticalLayout"));
        groupBox = new QGroupBox(ResizeDialog);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        _6 = new QGridLayout(groupBox);
        _6->setObjectName(QString::fromUtf8("_6"));
        label_3 = new QLabel(groupBox);
        label_3->setObjectName(QString::fromUtf8("label_3"));

        _6->addWidget(label_3, 0, 0, 1, 1);

        widthSpinBox = new QSpinBox(groupBox);
        widthSpinBox->setObjectName(QString::fromUtf8("widthSpinBox"));
        widthSpinBox->setMinimum(1);
        widthSpinBox->setMaximum(999);

        _6->addWidget(widthSpinBox, 0, 1, 1, 1);

        label_4 = new QLabel(groupBox);
        label_4->setObjectName(QString::fromUtf8("label_4"));

        _6->addWidget(label_4, 1, 0, 1, 1);

        heightSpinBox = new QSpinBox(groupBox);
        heightSpinBox->setObjectName(QString::fromUtf8("heightSpinBox"));
        heightSpinBox->setMinimum(1);
        heightSpinBox->setMaximum(999);

        _6->addWidget(heightSpinBox, 1, 1, 1, 1);


        verticalLayout->addWidget(groupBox);

        groupBox_2 = new QGroupBox(ResizeDialog);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        _4 = new QGridLayout(groupBox_2);
        _4->setObjectName(QString::fromUtf8("_4"));
        label_2 = new QLabel(groupBox_2);
        label_2->setObjectName(QString::fromUtf8("label_2"));

        _4->addWidget(label_2, 0, 0, 1, 1);

        offsetXSpinBox = new QSpinBox(groupBox_2);
        offsetXSpinBox->setObjectName(QString::fromUtf8("offsetXSpinBox"));
        offsetXSpinBox->setMinimum(-999);
        offsetXSpinBox->setMaximum(999);

        _4->addWidget(offsetXSpinBox, 0, 1, 1, 1);

        label = new QLabel(groupBox_2);
        label->setObjectName(QString::fromUtf8("label"));

        _4->addWidget(label, 1, 0, 1, 1);

        offsetYSpinBox = new QSpinBox(groupBox_2);
        offsetYSpinBox->setObjectName(QString::fromUtf8("offsetYSpinBox"));
        offsetYSpinBox->setMinimum(-999);
        offsetYSpinBox->setMaximum(999);

        _4->addWidget(offsetYSpinBox, 1, 1, 1, 1);


        verticalLayout->addWidget(groupBox_2);

        frame = new QFrame(ResizeDialog);
        frame->setObjectName(QString::fromUtf8("frame"));
        QSizePolicy sizePolicy(QSizePolicy::Preferred, QSizePolicy::Preferred);
        sizePolicy.setHorizontalStretch(0);
        sizePolicy.setVerticalStretch(1);
        sizePolicy.setHeightForWidth(frame->sizePolicy().hasHeightForWidth());
        frame->setSizePolicy(sizePolicy);
        frame->setFrameShape(QFrame::StyledPanel);
        frame->setFrameShadow(QFrame::Raised);
        _5 = new QVBoxLayout(frame);
        _5->setObjectName(QString::fromUtf8("_5"));
        resizeHelper = new Tiled::Internal::ResizeHelper(frame);
        resizeHelper->setObjectName(QString::fromUtf8("resizeHelper"));

        _5->addWidget(resizeHelper);


        verticalLayout->addWidget(frame);

        buttonBox = new QDialogButtonBox(ResizeDialog);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Cancel|QDialogButtonBox::Ok);

        verticalLayout->addWidget(buttonBox);

#ifndef QT_NO_SHORTCUT
        label_3->setBuddy(widthSpinBox);
        label_4->setBuddy(heightSpinBox);
        label_2->setBuddy(offsetXSpinBox);
        label->setBuddy(offsetYSpinBox);
#endif // QT_NO_SHORTCUT
        QWidget::setTabOrder(widthSpinBox, heightSpinBox);
        QWidget::setTabOrder(heightSpinBox, offsetXSpinBox);
        QWidget::setTabOrder(offsetXSpinBox, offsetYSpinBox);
        QWidget::setTabOrder(offsetYSpinBox, buttonBox);

        retranslateUi(ResizeDialog);
        QObject::connect(buttonBox, SIGNAL(accepted()), ResizeDialog, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), ResizeDialog, SLOT(reject()));
        QObject::connect(offsetXSpinBox, SIGNAL(valueChanged(int)), resizeHelper, SLOT(setOffsetX(int)));
        QObject::connect(offsetYSpinBox, SIGNAL(valueChanged(int)), resizeHelper, SLOT(setOffsetY(int)));
        QObject::connect(widthSpinBox, SIGNAL(valueChanged(int)), resizeHelper, SLOT(setNewWidth(int)));
        QObject::connect(heightSpinBox, SIGNAL(valueChanged(int)), resizeHelper, SLOT(setNewHeight(int)));
        QObject::connect(resizeHelper, SIGNAL(offsetXChanged(int)), offsetXSpinBox, SLOT(setValue(int)));
        QObject::connect(resizeHelper, SIGNAL(offsetYChanged(int)), offsetYSpinBox, SLOT(setValue(int)));

        QMetaObject::connectSlotsByName(ResizeDialog);
    } // setupUi

    void retranslateUi(QDialog *ResizeDialog)
    {
        ResizeDialog->setWindowTitle(QApplication::translate("ResizeDialog", "Resize", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("ResizeDialog", "Size", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("ResizeDialog", "Width:", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("ResizeDialog", "Height:", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("ResizeDialog", "Offset", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("ResizeDialog", "X:", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("ResizeDialog", "Y:", 0, QApplication::UnicodeUTF8));
        Q_UNUSED(ResizeDialog);
    } // retranslateUi

};

namespace Ui {
    class ResizeDialog: public Ui_ResizeDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_RESIZEDIALOG_H
