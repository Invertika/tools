/********************************************************************************
** Form generated from reading ui file 'saveasimagedialog.ui'
**
** Created: Wed Dec 23 18:06:07 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_SAVEASIMAGEDIALOG_H
#define UI_SAVEASIMAGEDIALOG_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QCheckBox>
#include <QtGui/QDialog>
#include <QtGui/QDialogButtonBox>
#include <QtGui/QGroupBox>
#include <QtGui/QHBoxLayout>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QLineEdit>
#include <QtGui/QPushButton>
#include <QtGui/QSpacerItem>
#include <QtGui/QVBoxLayout>

QT_BEGIN_NAMESPACE

class Ui_SaveAsImageDialog
{
public:
    QVBoxLayout *verticalLayout_2;
    QGroupBox *groupBox;
    QHBoxLayout *horizontalLayout;
    QLabel *label;
    QLineEdit *fileNameEdit;
    QPushButton *browseButton;
    QGroupBox *groupBox_2;
    QVBoxLayout *verticalLayout;
    QCheckBox *visibleLayersOnly;
    QCheckBox *currentZoomLevel;
    QSpacerItem *verticalSpacer;
    QDialogButtonBox *buttonBox;

    void setupUi(QDialog *SaveAsImageDialog)
    {
        if (SaveAsImageDialog->objectName().isEmpty())
            SaveAsImageDialog->setObjectName(QString::fromUtf8("SaveAsImageDialog"));
        SaveAsImageDialog->resize(337, 215);
        verticalLayout_2 = new QVBoxLayout(SaveAsImageDialog);
        verticalLayout_2->setObjectName(QString::fromUtf8("verticalLayout_2"));
        groupBox = new QGroupBox(SaveAsImageDialog);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        horizontalLayout = new QHBoxLayout(groupBox);
        horizontalLayout->setObjectName(QString::fromUtf8("horizontalLayout"));
        label = new QLabel(groupBox);
        label->setObjectName(QString::fromUtf8("label"));

        horizontalLayout->addWidget(label);

        fileNameEdit = new QLineEdit(groupBox);
        fileNameEdit->setObjectName(QString::fromUtf8("fileNameEdit"));

        horizontalLayout->addWidget(fileNameEdit);

        browseButton = new QPushButton(groupBox);
        browseButton->setObjectName(QString::fromUtf8("browseButton"));

        horizontalLayout->addWidget(browseButton);


        verticalLayout_2->addWidget(groupBox);

        groupBox_2 = new QGroupBox(SaveAsImageDialog);
        groupBox_2->setObjectName(QString::fromUtf8("groupBox_2"));
        verticalLayout = new QVBoxLayout(groupBox_2);
        verticalLayout->setObjectName(QString::fromUtf8("verticalLayout"));
        visibleLayersOnly = new QCheckBox(groupBox_2);
        visibleLayersOnly->setObjectName(QString::fromUtf8("visibleLayersOnly"));
        visibleLayersOnly->setChecked(true);

        verticalLayout->addWidget(visibleLayersOnly);

        currentZoomLevel = new QCheckBox(groupBox_2);
        currentZoomLevel->setObjectName(QString::fromUtf8("currentZoomLevel"));
        currentZoomLevel->setChecked(true);

        verticalLayout->addWidget(currentZoomLevel);


        verticalLayout_2->addWidget(groupBox_2);

        verticalSpacer = new QSpacerItem(20, 0, QSizePolicy::Minimum, QSizePolicy::Expanding);

        verticalLayout_2->addItem(verticalSpacer);

        buttonBox = new QDialogButtonBox(SaveAsImageDialog);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Cancel|QDialogButtonBox::Save);

        verticalLayout_2->addWidget(buttonBox);

#ifndef QT_NO_SHORTCUT
        label->setBuddy(fileNameEdit);
#endif // QT_NO_SHORTCUT

        retranslateUi(SaveAsImageDialog);
        QObject::connect(buttonBox, SIGNAL(accepted()), SaveAsImageDialog, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), SaveAsImageDialog, SLOT(reject()));

        QMetaObject::connectSlotsByName(SaveAsImageDialog);
    } // setupUi

    void retranslateUi(QDialog *SaveAsImageDialog)
    {
        SaveAsImageDialog->setWindowTitle(QApplication::translate("SaveAsImageDialog", "Save As Image", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("SaveAsImageDialog", "Location", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("SaveAsImageDialog", "Name:", 0, QApplication::UnicodeUTF8));
        browseButton->setText(QApplication::translate("SaveAsImageDialog", "&Browse...", 0, QApplication::UnicodeUTF8));
        groupBox_2->setTitle(QApplication::translate("SaveAsImageDialog", "Settings", 0, QApplication::UnicodeUTF8));
        visibleLayersOnly->setText(QApplication::translate("SaveAsImageDialog", "Only include &visible layers", 0, QApplication::UnicodeUTF8));
        currentZoomLevel->setText(QApplication::translate("SaveAsImageDialog", "Use current &zoom level", 0, QApplication::UnicodeUTF8));
        Q_UNUSED(SaveAsImageDialog);
    } // retranslateUi

};

namespace Ui {
    class SaveAsImageDialog: public Ui_SaveAsImageDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_SAVEASIMAGEDIALOG_H
