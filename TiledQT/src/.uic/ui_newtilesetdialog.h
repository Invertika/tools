/********************************************************************************
** Form generated from reading ui file 'newtilesetdialog.ui'
**
** Created: Wed Dec 23 18:06:07 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_NEWTILESETDIALOG_H
#define UI_NEWTILESETDIALOG_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QCheckBox>
#include <QtGui/QDialog>
#include <QtGui/QDialogButtonBox>
#include <QtGui/QGridLayout>
#include <QtGui/QGroupBox>
#include <QtGui/QHBoxLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QLineEdit>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>
#include <QtGui/QSpinBox>
#include "colorbutton.h"

QT_BEGIN_NAMESPACE

class Ui_NewTilesetDialog
{
public:
    QGridLayout *gridLayout_3;
    QGroupBox *groupBox_2;
    QGridLayout *gridLayout_2;
    QLabel *label;
    QLineEdit *name;
    QLabel *label_2;
    QLineEdit *image;
    QPushButton *browseButton;
    QHBoxLayout *horizontalLayout;
    QCheckBox *useTransparentColor;
    Tiled::Internal::ColorButton *colorButton;
    QSpacerItem *horizontalSpacer_2;
    QGroupBox *groupBox;
    QGridLayout *gridLayout;
    QLabel *label_3;
    QSpinBox *tileWidth;
    QLabel *label_5;
    QSpinBox *margin;
    QLabel *label_4;
    QSpinBox *tileHeight;
    QLabel *label_6;
    QSpinBox *spacing;
    QSpacerItem *horizontalSpacer;
    QSpacerItem *verticalSpacer;
    QDialogButtonBox *buttonBox;

    void setupUi(QDialog *NewTilesetDialog)
    {
        if (NewTilesetDialog->objectName().isEmpty())
            NewTilesetDialog->setObjectName(QString::fromUtf8("NewTilesetDialog"));
        gridLayout_3 = new QGridLayout(NewTilesetDialog);
        gridLayout_3->setObjectName(QString::fromUtf8("gridLayout_3"));
        groupBox_2 = new QGroupBox(NewTilesetDialog);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        gridLayout_2 = new QGridLayout(groupBox_2);
        gridLayout_2->setObjectName(QString::fromUtf8("gridLayout_2"));
        label = new QLabel(groupBox_2);
        label->setObjectName(QString::fromUtf8("label"));

        gridLayout_2->addWidget(label, 0, 0, 1, 1);

        name = new QLineEdit(groupBox_2);
        name->setObjectName(QString::fromUtf8("name"));

        gridLayout_2->addWidget(name, 0, 1, 1, 2);

        label_2 = new QLabel(groupBox_2);
        label_2->setObjectName(QString::fromUtf8("label_2"));

        gridLayout_2->addWidget(label_2, 1, 0, 1, 1);

        image = new QLineEdit(groupBox_2);
        image->setObjectName(QString::fromUtf8("image"));

        gridLayout_2->addWidget(image, 1, 1, 1, 1);

        browseButton = new QPushButton(groupBox_2);
        browseButton->setObjectName(QString::fromUtf8("browseButton"));

        gridLayout_2->addWidget(browseButton, 1, 2, 1, 1);

        horizontalLayout = new QHBoxLayout();
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        useTransparentColor = new QCheckBox(groupBox_2);
        useTransparentColor->setObjectName(QString::fromUtf8("useTransparentColor"));

        horizontalLayout->addWidget(useTransparentColor);

        colorButton = new Tiled::Internal::ColorButton(groupBox_2);
        colorButton->setObjectName(QString::fromUtf8("colorButton"));
        colorButton->setEnabled(false);

        horizontalLayout->addWidget(colorButton);

        horizontalSpacer_2 = new QSpacerItem(0, 0, QSizePolicy::Expanding, QSizePolicy::Minimum);

        horizontalLayout->addItem(horizontalSpacer_2);


        gridLayout_2->addLayout(horizontalLayout, 2, 1, 1, 2);


        gridLayout_3->addWidget(groupBox_2, 0, 0, 1, 1);

        groupBox = new QGroupBox(NewTilesetDialog);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        gridLayout = new QGridLayout(groupBox);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        label_3 = new QLabel(groupBox);
        label_3->setObjectName(QString::fromUtf8("label_3"));

        gridLayout->addWidget(label_3, 0, 0, 1, 1);

        tileWidth = new QSpinBox(groupBox);
        tileWidth->setObjectName(QString::fromUtf8("tileWidth"));
        tileWidth->setEnabled(true);
        tileWidth->setMinimum(1);
        tileWidth->setMaximum(999);
        tileWidth->setValue(32);

        gridLayout->addWidget(tileWidth, 0, 1, 1, 1);

        label_5 = new QLabel(groupBox);
        label_5->setObjectName(QString::fromUtf8("label_5"));

        gridLayout->addWidget(label_5, 0, 3, 1, 1);

        margin = new QSpinBox(groupBox);
        margin->setObjectName(QString::fromUtf8("margin"));
        margin->setMinimum(0);
        margin->setMaximum(99);
        margin->setValue(0);

        gridLayout->addWidget(margin, 0, 4, 1, 1);

        label_4 = new QLabel(groupBox);
        label_4->setObjectName(QString::fromUtf8("label_4"));

        gridLayout->addWidget(label_4, 1, 0, 1, 1);

        tileHeight = new QSpinBox(groupBox);
        tileHeight->setObjectName(QString::fromUtf8("tileHeight"));
        tileHeight->setMinimum(1);
        tileHeight->setMaximum(999);
        tileHeight->setValue(32);

        gridLayout->addWidget(tileHeight, 1, 1, 1, 1);

        label_6 = new QLabel(groupBox);
        label_6->setObjectName(QString::fromUtf8("label_6"));

        gridLayout->addWidget(label_6, 1, 3, 1, 1);

        spacing = new QSpinBox(groupBox);
        spacing->setObjectName(QString::fromUtf8("spacing"));
        spacing->setMinimum(0);
        spacing->setMaximum(99);
        spacing->setValue(0);

        gridLayout->addWidget(spacing, 1, 4, 1, 1);

        horizontalSpacer = new QSpacerItem(20, 20, QSizePolicy::MinimumExpanding, QSizePolicy::Minimum);

        gridLayout->addItem(horizontalSpacer, 0, 2, 1, 1);


        gridLayout_3->addWidget(groupBox, 1, 0, 1, 1);

        verticalSpacer = new QSpacerItem(20, 0, QSizePolicy::Minimum, QSizePolicy::Expanding);

        gridLayout_3->addItem(verticalSpacer, 2, 0, 1, 1);

        buttonBox = new QDialogButtonBox(NewTilesetDialog);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Cancel|QDialogButtonBox::Ok);

        gridLayout_3->addWidget(buttonBox, 3, 0, 1, 1);

#ifndef QT_NO_SHORTCUT
        label->setBuddy(name);
#endif // QT_NO_SHORTCUT

        retranslateUi(NewTilesetDialog);
        QObject::connect(buttonBox, SIGNAL(accepted()), NewTilesetDialog, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), NewTilesetDialog, SLOT(reject()));
        QObject::connect(useTransparentColor, SIGNAL(toggled(bool)), colorButton, SLOT(setEnabled(bool)));

        QMetaObject::connectSlotsByName(NewTilesetDialog);
    } // setupUi

    void retranslateUi(QDialog *NewTilesetDialog)
    {
        NewTilesetDialog->setWindowTitle(QApplication::translate("NewTilesetDialog", "New Tileset", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("NewTilesetDialog", "Tileset", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("NewTilesetDialog", "&Name:", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("NewTilesetDialog", "Image:", 0, QApplication::UnicodeUTF8));
        browseButton->setText(QApplication::translate("NewTilesetDialog", "&Browse...", 0, QApplication::UnicodeUTF8));
        useTransparentColor->setText(QApplication::translate("NewTilesetDialog", "Use transparent color:", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("NewTilesetDialog", "Tiles", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("NewTilesetDialog", "Tile width:", 0, QApplication::UnicodeUTF8));
        label_5->setText(QApplication::translate("NewTilesetDialog", "Margin:", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("NewTilesetDialog", "Tile height:", 0, QApplication::UnicodeUTF8));
        label_6->setText(QApplication::translate("NewTilesetDialog", "Spacing:", 0, QApplication::UnicodeUTF8));
        Q_UNUSED(NewTilesetDialog);
    } // retranslateUi

};

namespace Ui {
    class NewTilesetDialog: public Ui_NewTilesetDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_NEWTILESETDIALOG_H
