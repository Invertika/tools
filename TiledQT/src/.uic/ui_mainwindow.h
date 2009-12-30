/********************************************************************************
** Form generated from reading ui file 'mainwindow.ui'
**
** Created: Wed Dec 23 18:06:07 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QHeaderView>
#include <QtGui/QMainWindow>
#include <QtGui/QMenu>
#include <QtGui/QMenuBar>
#include <QtGui/QStatusBar>
#include <QtGui/QToolBar>
#include <QtGui/QVBoxLayout>
#include <QtGui/QWidget>
#include "mapview.h"

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QAction *actionOpen;
    QAction *actionSave;
    QAction *actionQuit;
    QAction *actionCopy;
    QAction *actionPaste;
    QAction *actionAbout;
    QAction *actionAboutQt;
    QAction *actionResizeMap;
    QAction *actionMapProperties;
    QAction *actionRecentFiles;
    QAction *actionShowGrid;
    QAction *actionSaveAs;
    QAction *actionMoveLayerUp;
    QAction *actionMoveLayerDown;
    QAction *actionSelectAll;
    QAction *actionSelectNone;
    QAction *actionNew;
    QAction *actionNewTileset;
    QAction *actionRemoveLayer;
    QAction *actionClose;
    QAction *actionAddTileLayer;
    QAction *actionAddObjectLayer;
    QAction *actionDuplicateLayer;
    QAction *actionLayerProperties;
    QAction *actionZoomIn;
    QAction *actionZoomOut;
    QAction *actionZoomNormal;
    QAction *actionSaveAsImage;
    QAction *actionCut;
    QAction *actionOffsetMap;
    QWidget *centralWidget;
    QVBoxLayout *verticalLayout;
    Tiled::Internal::MapView *mapView;
    QMenuBar *menuBar;
    QMenu *menuFile;
    QMenu *menuEdit;
    QMenu *menuHelp;
    QMenu *menuMap;
    QMenu *menuView;
    QMenu *menuLayer;
    QToolBar *mainToolBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QString::fromUtf8("MainWindow"));
        MainWindow->resize(553, 367);
        actionOpen = new QAction(MainWindow);
        actionOpen->setObjectName(QString::fromUtf8("actionOpen"));
        QIcon icon;
        icon.addFile(QString::fromUtf8(":/images/16x16/document-open.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionOpen->setIcon(icon);
        actionSave = new QAction(MainWindow);
        actionSave->setObjectName(QString::fromUtf8("actionSave"));
        QIcon icon1;
        icon1.addFile(QString::fromUtf8(":/images/16x16/document-save.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionSave->setIcon(icon1);
        actionQuit = new QAction(MainWindow);
        actionQuit->setObjectName(QString::fromUtf8("actionQuit"));
        QIcon icon2;
        icon2.addFile(QString::fromUtf8(":/images/16x16/application-exit.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionQuit->setIcon(icon2);
        actionCopy = new QAction(MainWindow);
        actionCopy->setObjectName(QString::fromUtf8("actionCopy"));
        actionCopy->setEnabled(false);
        QIcon icon3;
        icon3.addFile(QString::fromUtf8(":/images/16x16/edit-copy.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionCopy->setIcon(icon3);
        actionPaste = new QAction(MainWindow);
        actionPaste->setObjectName(QString::fromUtf8("actionPaste"));
        actionPaste->setEnabled(false);
        QIcon icon4;
        icon4.addFile(QString::fromUtf8(":/images/16x16/edit-paste.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionPaste->setIcon(icon4);
        actionAbout = new QAction(MainWindow);
        actionAbout->setObjectName(QString::fromUtf8("actionAbout"));
        QIcon icon5;
        icon5.addFile(QString::fromUtf8(":/images/16x16/help-about.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionAbout->setIcon(icon5);
        actionAboutQt = new QAction(MainWindow);
        actionAboutQt->setObjectName(QString::fromUtf8("actionAboutQt"));
        actionResizeMap = new QAction(MainWindow);
        actionResizeMap->setObjectName(QString::fromUtf8("actionResizeMap"));
        QIcon icon6;
        icon6.addFile(QString::fromUtf8(":/images/16x16/document-page-setup.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionResizeMap->setIcon(icon6);
        actionMapProperties = new QAction(MainWindow);
        actionMapProperties->setObjectName(QString::fromUtf8("actionMapProperties"));
        QIcon icon7;
        icon7.addFile(QString::fromUtf8(":/images/16x16/document-properties.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionMapProperties->setIcon(icon7);
        actionRecentFiles = new QAction(MainWindow);
        actionRecentFiles->setObjectName(QString::fromUtf8("actionRecentFiles"));
        QIcon icon8;
        icon8.addFile(QString::fromUtf8(":/images/16x16/document-open-recent.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionRecentFiles->setIcon(icon8);
        actionShowGrid = new QAction(MainWindow);
        actionShowGrid->setObjectName(QString::fromUtf8("actionShowGrid"));
        actionShowGrid->setCheckable(true);
        actionSaveAs = new QAction(MainWindow);
        actionSaveAs->setObjectName(QString::fromUtf8("actionSaveAs"));
        QIcon icon9;
        icon9.addFile(QString::fromUtf8(":/images/16x16/document-save-as.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionSaveAs->setIcon(icon9);
        actionMoveLayerUp = new QAction(MainWindow);
        actionMoveLayerUp->setObjectName(QString::fromUtf8("actionMoveLayerUp"));
        QIcon icon10;
        icon10.addFile(QString::fromUtf8(":/images/16x16/go-up.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionMoveLayerUp->setIcon(icon10);
        actionMoveLayerDown = new QAction(MainWindow);
        actionMoveLayerDown->setObjectName(QString::fromUtf8("actionMoveLayerDown"));
        QIcon icon11;
        icon11.addFile(QString::fromUtf8(":/images/16x16/go-down.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionMoveLayerDown->setIcon(icon11);
        actionSelectAll = new QAction(MainWindow);
        actionSelectAll->setObjectName(QString::fromUtf8("actionSelectAll"));
        actionSelectNone = new QAction(MainWindow);
        actionSelectNone->setObjectName(QString::fromUtf8("actionSelectNone"));
        actionNew = new QAction(MainWindow);
        actionNew->setObjectName(QString::fromUtf8("actionNew"));
        QIcon icon12;
        icon12.addFile(QString::fromUtf8(":/images/16x16/document-new.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionNew->setIcon(icon12);
        actionNewTileset = new QAction(MainWindow);
        actionNewTileset->setObjectName(QString::fromUtf8("actionNewTileset"));
        actionNewTileset->setIcon(icon12);
        actionRemoveLayer = new QAction(MainWindow);
        actionRemoveLayer->setObjectName(QString::fromUtf8("actionRemoveLayer"));
        QIcon icon13;
        icon13.addFile(QString::fromUtf8(":/images/16x16/edit-delete.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionRemoveLayer->setIcon(icon13);
        actionClose = new QAction(MainWindow);
        actionClose->setObjectName(QString::fromUtf8("actionClose"));
        QIcon icon14;
        icon14.addFile(QString::fromUtf8(":/images/16x16/window-close.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionClose->setIcon(icon14);
        actionAddTileLayer = new QAction(MainWindow);
        actionAddTileLayer->setObjectName(QString::fromUtf8("actionAddTileLayer"));
        actionAddObjectLayer = new QAction(MainWindow);
        actionAddObjectLayer->setObjectName(QString::fromUtf8("actionAddObjectLayer"));
        actionDuplicateLayer = new QAction(MainWindow);
        actionDuplicateLayer->setObjectName(QString::fromUtf8("actionDuplicateLayer"));
        QIcon icon15;
        icon15.addFile(QString::fromUtf8(":/images/16x16/stock-duplicate-16.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionDuplicateLayer->setIcon(icon15);
        actionLayerProperties = new QAction(MainWindow);
        actionLayerProperties->setObjectName(QString::fromUtf8("actionLayerProperties"));
        actionLayerProperties->setIcon(icon7);
        actionZoomIn = new QAction(MainWindow);
        actionZoomIn->setObjectName(QString::fromUtf8("actionZoomIn"));
        QIcon icon16;
        icon16.addFile(QString::fromUtf8(":/images/16x16/zoom-in.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionZoomIn->setIcon(icon16);
        actionZoomOut = new QAction(MainWindow);
        actionZoomOut->setObjectName(QString::fromUtf8("actionZoomOut"));
        QIcon icon17;
        icon17.addFile(QString::fromUtf8(":/images/16x16/zoom-out.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionZoomOut->setIcon(icon17);
        actionZoomNormal = new QAction(MainWindow);
        actionZoomNormal->setObjectName(QString::fromUtf8("actionZoomNormal"));
        actionZoomNormal->setEnabled(false);
        QIcon icon18;
        icon18.addFile(QString::fromUtf8(":/images/16x16/zoom-original.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionZoomNormal->setIcon(icon18);
        actionSaveAsImage = new QAction(MainWindow);
        actionSaveAsImage->setObjectName(QString::fromUtf8("actionSaveAsImage"));
        actionCut = new QAction(MainWindow);
        actionCut->setObjectName(QString::fromUtf8("actionCut"));
        actionCut->setEnabled(false);
        QIcon icon19;
        icon19.addFile(QString::fromUtf8(":/images/16x16/edit-cut.png"), QSize(), QIcon::Normal, QIcon::Off);
        actionCut->setIcon(icon19);
        actionOffsetMap = new QAction(MainWindow);
        actionOffsetMap->setObjectName(QString::fromUtf8("actionOffsetMap"));
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QString::fromUtf8("centralWidget"));
        verticalLayout = new QVBoxLayout(centralWidget);
        verticalLayout->setSpacing(6);
        verticalLayout->setMargin(0);
        verticalLayout->setObjectName(QString::fromUtf8("verticalLayout"));
        mapView = new Tiled::Internal::MapView(centralWidget);
        mapView->setObjectName(QString::fromUtf8("mapView"));
        QSizePolicy sizePolicy(QSizePolicy::Preferred, QSizePolicy::Expanding);
        sizePolicy.setHorizontalStretch(1);
        sizePolicy.setVerticalStretch(1);
        sizePolicy.setHeightForWidth(mapView->sizePolicy().hasHeightForWidth());
        mapView->setSizePolicy(sizePolicy);
        mapView->setAlignment(Qt::AlignLeading|Qt::AlignLeft|Qt::AlignTop);

        verticalLayout->addWidget(mapView);

        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QString::fromUtf8("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 553, 25));
        menuFile = new QMenu(menuBar);
        menuFile->setObjectName(QString::fromUtf8("menuFile"));
        menuEdit = new QMenu(menuBar);
        menuEdit->setObjectName(QString::fromUtf8("menuEdit"));
        menuHelp = new QMenu(menuBar);
        menuHelp->setObjectName(QString::fromUtf8("menuHelp"));
        menuMap = new QMenu(menuBar);
        menuMap->setObjectName(QString::fromUtf8("menuMap"));
        menuView = new QMenu(menuBar);
        menuView->setObjectName(QString::fromUtf8("menuView"));
        menuLayer = new QMenu(menuBar);
        menuLayer->setObjectName(QString::fromUtf8("menuLayer"));
        MainWindow->setMenuBar(menuBar);
        mainToolBar = new QToolBar(MainWindow);
        mainToolBar->setObjectName(QString::fromUtf8("mainToolBar"));
        MainWindow->addToolBar(Qt::TopToolBarArea, mainToolBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QString::fromUtf8("statusBar"));
        MainWindow->setStatusBar(statusBar);

        menuBar->addAction(menuFile->menuAction());
        menuBar->addAction(menuEdit->menuAction());
        menuBar->addAction(menuView->menuAction());
        menuBar->addAction(menuMap->menuAction());
        menuBar->addAction(menuLayer->menuAction());
        menuBar->addAction(menuHelp->menuAction());
        menuFile->addAction(actionNew);
        menuFile->addAction(actionOpen);
        menuFile->addAction(actionRecentFiles);
        menuFile->addSeparator();
        menuFile->addAction(actionSave);
        menuFile->addAction(actionSaveAs);
        menuFile->addAction(actionSaveAsImage);
        menuFile->addSeparator();
        menuFile->addAction(actionClose);
        menuFile->addAction(actionQuit);
        menuEdit->addAction(actionCut);
        menuEdit->addAction(actionCopy);
        menuEdit->addAction(actionPaste);
        menuEdit->addSeparator();
        menuEdit->addAction(actionSelectAll);
        menuEdit->addAction(actionSelectNone);
        menuHelp->addAction(actionAbout);
        menuHelp->addAction(actionAboutQt);
        menuMap->addAction(actionNewTileset);
        menuMap->addSeparator();
        menuMap->addAction(actionResizeMap);
        menuMap->addAction(actionOffsetMap);
        menuMap->addSeparator();
        menuMap->addAction(actionMapProperties);
        menuView->addAction(actionShowGrid);
        menuView->addSeparator();
        menuView->addAction(actionZoomIn);
        menuView->addAction(actionZoomOut);
        menuView->addAction(actionZoomNormal);
        menuLayer->addAction(actionAddTileLayer);
        menuLayer->addAction(actionAddObjectLayer);
        menuLayer->addAction(actionDuplicateLayer);
        menuLayer->addAction(actionRemoveLayer);
        menuLayer->addSeparator();
        menuLayer->addAction(actionMoveLayerUp);
        menuLayer->addAction(actionMoveLayerDown);
        menuLayer->addSeparator();
        menuLayer->addAction(actionLayerProperties);
        mainToolBar->addAction(actionNew);
        mainToolBar->addAction(actionOpen);
        mainToolBar->addAction(actionSave);
        mainToolBar->addSeparator();

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "Tiled", 0, QApplication::UnicodeUTF8));
        actionOpen->setText(QApplication::translate("MainWindow", "&Open...", 0, QApplication::UnicodeUTF8));
        actionSave->setText(QApplication::translate("MainWindow", "&Save", 0, QApplication::UnicodeUTF8));
        actionQuit->setText(QApplication::translate("MainWindow", "&Quit", 0, QApplication::UnicodeUTF8));
        actionCopy->setText(QApplication::translate("MainWindow", "&Copy", 0, QApplication::UnicodeUTF8));
        actionPaste->setText(QApplication::translate("MainWindow", "&Paste", 0, QApplication::UnicodeUTF8));
        actionAbout->setText(QApplication::translate("MainWindow", "&About Tiled", 0, QApplication::UnicodeUTF8));
        actionAboutQt->setText(QApplication::translate("MainWindow", "About Qt", 0, QApplication::UnicodeUTF8));
        actionResizeMap->setText(QApplication::translate("MainWindow", "&Resize Map...", 0, QApplication::UnicodeUTF8));
        actionMapProperties->setText(QApplication::translate("MainWindow", "Map &Properties...", 0, QApplication::UnicodeUTF8));
        actionRecentFiles->setText(QApplication::translate("MainWindow", "&Recent Files", 0, QApplication::UnicodeUTF8));
        actionShowGrid->setText(QApplication::translate("MainWindow", "Show &Grid", 0, QApplication::UnicodeUTF8));
        actionShowGrid->setShortcut(QApplication::translate("MainWindow", "Ctrl+G", 0, QApplication::UnicodeUTF8));
        actionSaveAs->setText(QApplication::translate("MainWindow", "Save &As...", 0, QApplication::UnicodeUTF8));
        actionMoveLayerUp->setText(QApplication::translate("MainWindow", "Move Layer &Up", 0, QApplication::UnicodeUTF8));
        actionMoveLayerUp->setShortcut(QApplication::translate("MainWindow", "Ctrl+Shift+Up", 0, QApplication::UnicodeUTF8));
        actionMoveLayerDown->setText(QApplication::translate("MainWindow", "Move Layer Dow&n", 0, QApplication::UnicodeUTF8));
        actionMoveLayerDown->setShortcut(QApplication::translate("MainWindow", "Ctrl+Shift+Down", 0, QApplication::UnicodeUTF8));
        actionSelectAll->setText(QApplication::translate("MainWindow", "Select &All", 0, QApplication::UnicodeUTF8));
        actionSelectNone->setText(QApplication::translate("MainWindow", "Select &None", 0, QApplication::UnicodeUTF8));
        actionNew->setText(QApplication::translate("MainWindow", "New...", 0, QApplication::UnicodeUTF8));
        actionNewTileset->setText(QApplication::translate("MainWindow", "New &Tileset...", 0, QApplication::UnicodeUTF8));
        actionRemoveLayer->setText(QApplication::translate("MainWindow", "&Remove Layer", 0, QApplication::UnicodeUTF8));
        actionClose->setText(QApplication::translate("MainWindow", "&Close", 0, QApplication::UnicodeUTF8));
        actionAddTileLayer->setText(QApplication::translate("MainWindow", "Add &Tile Layer...", 0, QApplication::UnicodeUTF8));
        actionAddObjectLayer->setText(QApplication::translate("MainWindow", "Add &Object Layer...", 0, QApplication::UnicodeUTF8));
        actionDuplicateLayer->setText(QApplication::translate("MainWindow", "&Duplicate Layer", 0, QApplication::UnicodeUTF8));
        actionDuplicateLayer->setShortcut(QApplication::translate("MainWindow", "Ctrl+Shift+D", 0, QApplication::UnicodeUTF8));
        actionLayerProperties->setText(QApplication::translate("MainWindow", "Layer &Properties...", 0, QApplication::UnicodeUTF8));
        actionZoomIn->setText(QApplication::translate("MainWindow", "Zoom In", 0, QApplication::UnicodeUTF8));
        actionZoomOut->setText(QApplication::translate("MainWindow", "Zoom Out", 0, QApplication::UnicodeUTF8));
        actionZoomNormal->setText(QApplication::translate("MainWindow", "Normal Size", 0, QApplication::UnicodeUTF8));
        actionZoomNormal->setShortcut(QApplication::translate("MainWindow", "Ctrl+0", 0, QApplication::UnicodeUTF8));
        actionSaveAsImage->setText(QApplication::translate("MainWindow", "Save As Image...", 0, QApplication::UnicodeUTF8));
        actionCut->setText(QApplication::translate("MainWindow", "Cu&t", 0, QApplication::UnicodeUTF8));
        actionOffsetMap->setText(QApplication::translate("MainWindow", "&Offset Map...", 0, QApplication::UnicodeUTF8));
#ifndef QT_NO_TOOLTIP
        actionOffsetMap->setToolTip(QApplication::translate("MainWindow", "Offsets everything in a layer", 0, QApplication::UnicodeUTF8));
#endif // QT_NO_TOOLTIP
        menuFile->setTitle(QApplication::translate("MainWindow", "&File", 0, QApplication::UnicodeUTF8));
        menuEdit->setTitle(QApplication::translate("MainWindow", "&Edit", 0, QApplication::UnicodeUTF8));
        menuHelp->setTitle(QApplication::translate("MainWindow", "&Help", 0, QApplication::UnicodeUTF8));
        menuMap->setTitle(QApplication::translate("MainWindow", "&Map", 0, QApplication::UnicodeUTF8));
        menuView->setTitle(QApplication::translate("MainWindow", "&View", 0, QApplication::UnicodeUTF8));
        menuLayer->setTitle(QApplication::translate("MainWindow", "&Layer", 0, QApplication::UnicodeUTF8));
        mainToolBar->setWindowTitle(QApplication::translate("MainWindow", "Main Toolbar", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
