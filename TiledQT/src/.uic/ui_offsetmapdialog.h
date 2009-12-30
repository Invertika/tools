/********************************************************************************
** Form generated from reading ui file 'offsetmapdialog.ui'
**
** Created: Wed Dec 23 18:06:07 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_OFFSETMAPDIALOG_H
#define UI_OFFSETMAPDIALOG_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QCheckBox>
#include <QtGui/QComboBox>
#include <QtGui/QDialog>
#include <QtGui/QDialogButtonBox>
#include <QtGui/QGridLayout>
#include <QtGui/QGroupBox>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QSpacerItem>
#include <QtGui/QSpinBox>
#include <QtGui/QVBoxLayout>

QT_BEGIN_NAMESPACE

class Ui_OffsetMapDialog
{
public:
    QVBoxLayout *verticalLayout_5;
    QGroupBox *offsetGroup;
    QGridLayout *gridLayout;
    QLabel *labelX;
    QSpinBox *xOffset;
    QCheckBox *wrapX;
    QLabel *labelY;
    QSpacerItem *horizontalSpacer;
    QSpinBox *yOffset;
    QCheckBox *wrapY;
    QLabel *labelLayers;
    QComboBox *layerSelection;
    QLabel *labelBounds;
    QComboBox *boundsSelection;
    QSpacerItem *verticalSpacer;
    QDialogButtonBox *buttonBox;

    void setupUi(QDialog *OffsetMapDialog)
    {
        if (OffsetMapDialog->objectName().isEmpty())
            OffsetMapDialog->setObjectName(QString::fromUtf8("OffsetMapDialog"));
        OffsetMapDialog->setEnabled(true);
        OffsetMapDialog->resize(238, 212);
        OffsetMapDialog->setAutoFillBackground(false);
        verticalLayout_5 = new QVBoxLayout(OffsetMapDialog);
        verticalLayout_5->setObjectName(QString::fromUtf8("verticalLayout_5"));
        verticalLayout_5->setSizeConstraint(QLayout::SetFixedSize);
        offsetGroup = new QGroupBox(OffsetMapDialog);
        offsetGroup->setObjectName(QString::fromUtf8("offsetGroup"));
        gridLayout = new QGridLayout(offsetGroup);
        gridLayout->setObjectName(QString::fromUtf8("gridLayout"));
        labelX = new QLabel(offsetGroup);
        labelX->setObjectName(QString::fromUtf8("labelX"));

        gridLayout->addWidget(labelX, 2, 0, 1, 1);

        xOffset = new QSpinBox(offsetGroup);
        xOffset->setObjectName(QString::fromUtf8("xOffset"));
        xOffset->setEnabled(true);
        xOffset->setMinimum(-999);
        xOffset->setMaximum(999);
        xOffset->setValue(0);

        gridLayout->addWidget(xOffset, 2, 2, 1, 1);

        wrapX = new QCheckBox(offsetGroup);
        wrapX->setObjectName(QString::fromUtf8("wrapX"));

        gridLayout->addWidget(wrapX, 2, 3, 1, 1);

        labelY = new QLabel(offsetGroup);
        labelY->setObjectName(QString::fromUtf8("labelY"));

        gridLayout->addWidget(labelY, 4, 0, 1, 1);

        horizontalSpacer = new QSpacerItem(10, 20, QSizePolicy::Fixed, QSizePolicy::Minimum);

        gridLayout->addItem(horizontalSpacer, 4, 1, 1, 1);

        yOffset = new QSpinBox(offsetGroup);
        yOffset->setObjectName(QString::fromUtf8("yOffset"));
        yOffset->setMinimum(-999);
        yOffset->setMaximum(999);
        yOffset->setValue(0);

        gridLayout->addWidget(yOffset, 4, 2, 1, 1);

        wrapY = new QCheckBox(offsetGroup);
        wrapY->setObjectName(QString::fromUtf8("wrapY"));

        gridLayout->addWidget(wrapY, 4, 3, 1, 1);

        labelLayers = new QLabel(offsetGroup);
        labelLayers->setObjectName(QString::fromUtf8("labelLayers"));

        gridLayout->addWidget(labelLayers, 5, 0, 1, 1);

        layerSelection = new QComboBox(offsetGroup);
        layerSelection->setObjectName(QString::fromUtf8("layerSelection"));

        gridLayout->addWidget(layerSelection, 5, 2, 1, 2);

        labelBounds = new QLabel(offsetGroup);
        labelBounds->setObjectName(QString::fromUtf8("labelBounds"));

        gridLayout->addWidget(labelBounds, 6, 0, 1, 1);

        boundsSelection = new QComboBox(offsetGroup);
        boundsSelection->setObjectName(QString::fromUtf8("boundsSelection"));

        gridLayout->addWidget(boundsSelection, 6, 2, 1, 2);


        verticalLayout_5->addWidget(offsetGroup);

        verticalSpacer = new QSpacerItem(20, 5, QSizePolicy::Minimum, QSizePolicy::MinimumExpanding);

        verticalLayout_5->addItem(verticalSpacer);

        buttonBox = new QDialogButtonBox(OffsetMapDialog);
        buttonBox->setObjectName(QString::fromUtf8("buttonBox"));
        buttonBox->setOrientation(Qt::Horizontal);
        buttonBox->setStandardButtons(QDialogButtonBox::Cancel|QDialogButtonBox::Ok);

        verticalLayout_5->addWidget(buttonBox);


        retranslateUi(OffsetMapDialog);
        QObject::connect(buttonBox, SIGNAL(accepted()), OffsetMapDialog, SLOT(accept()));
        QObject::connect(buttonBox, SIGNAL(rejected()), OffsetMapDialog, SLOT(reject()));

        QMetaObject::connectSlotsByName(OffsetMapDialog);
    } // setupUi

    void retranslateUi(QDialog *OffsetMapDialog)
    {
        OffsetMapDialog->setWindowTitle(QApplication::translate("OffsetMapDialog", "Offset Map", 0, QApplication::UnicodeUTF8));
        offsetGroup->setTitle(QApplication::translate("OffsetMapDialog", "Offset Contents of Map", 0, QApplication::UnicodeUTF8));
        labelX->setText(QApplication::translate("OffsetMapDialog", "X:", 0, QApplication::UnicodeUTF8));
        wrapX->setText(QApplication::translate("OffsetMapDialog", "Wrap", 0, QApplication::UnicodeUTF8));
        labelY->setText(QApplication::translate("OffsetMapDialog", "Y:", 0, QApplication::UnicodeUTF8));
        wrapY->setText(QApplication::translate("OffsetMapDialog", "Wrap", 0, QApplication::UnicodeUTF8));
        labelLayers->setText(QApplication::translate("OffsetMapDialog", "Layers:", 0, QApplication::UnicodeUTF8));
        layerSelection->clear();
        layerSelection->insertItems(0, QStringList()
         << QApplication::translate("OffsetMapDialog", "All Visible Layers", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("OffsetMapDialog", "All Layers", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("OffsetMapDialog", "Selected Layer", 0, QApplication::UnicodeUTF8)
        );
        labelBounds->setText(QApplication::translate("OffsetMapDialog", "Bounds:", 0, QApplication::UnicodeUTF8));
        boundsSelection->clear();
        boundsSelection->insertItems(0, QStringList()
         << QApplication::translate("OffsetMapDialog", "Whole Map", 0, QApplication::UnicodeUTF8)
         << QApplication::translate("OffsetMapDialog", "Current Selection", 0, QApplication::UnicodeUTF8)
        );
        Q_UNUSED(OffsetMapDialog);
    } // retranslateUi

};

namespace Ui {
    class OffsetMapDialog: public Ui_OffsetMapDialog {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_OFFSETMAPDIALOG_H
