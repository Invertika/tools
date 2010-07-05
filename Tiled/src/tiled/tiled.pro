include(../../tiled.pri)
include(../libtiled/libtiled.pri)

TEMPLATE = app
TARGET = tiled
target.path = $${PREFIX}/bin
INSTALLS += target
win32 {
    DESTDIR = ../..
} else {
    DESTDIR = ../../bin
}

DEFINES += QT_NO_CAST_FROM_ASCII \
    QT_NO_CAST_TO_ASCII

macx {
    LIBS += -L$$OUT_PWD/../../bin/Tiled.app/Contents/Frameworks
} else {
    LIBS += -L$$OUT_PWD/../../lib
}

# Make sure the Tiled executable can find libtiled
!win32:!macx {
    QMAKE_RPATHDIR += \$\$ORIGIN/../lib

    # It is not possible to use ORIGIN in QMAKE_RPATHDIR, so a bit manually
    QMAKE_LFLAGS += -Wl,-z,origin \'-Wl,-rpath,$$join(QMAKE_RPATHDIR, ":")\'
    QMAKE_RPATHDIR =
}

MOC_DIR = .moc
UI_DIR = .uic
RCC_DIR = .rcc
OBJECTS_DIR = .obj

SOURCES += aboutdialog.cpp \
    brushitem.cpp \
    compression.cpp \
    languagemanager.cpp \
    layerdock.cpp \
    layermodel.cpp \
    main.cpp \
    mainwindow.cpp \
    mapdocument.cpp \
    mapdocumentactionhandler.cpp \
    mapobjectitem.cpp \
    mapscene.cpp \
    mapview.cpp \
    painttilelayer.cpp \
    pluginmanager.cpp \
    preferencesdialog.cpp \
    preferences.cpp \
    propertiesdialog.cpp \
    propertiesmodel.cpp \
    resizehelper.cpp \
    resizedialog.cpp \
    tileselectionitem.cpp \
    tilesetdock.cpp \
    tilesetmanager.cpp \
    tilesetmodel.cpp \
    tilesetview.cpp \
    tilelayeritem.cpp \
    tmxmapreader.cpp \
    tmxmapwriter.cpp \
    changeproperties.cpp \
    movelayer.cpp \
    tilepainter.cpp \
    newmapdialog.cpp \
    newtilesetdialog.cpp \
    objectgroupitem.cpp \
    movemapobject.cpp \
    resizemapobject.cpp \
    addremovemapobject.cpp \
    addremovelayer.cpp \
    propertiesview.cpp \
    renamelayer.cpp \
    resizelayer.cpp \
    resizemap.cpp \
    objectpropertiesdialog.cpp \
    changemapobject.cpp \
    stampbrush.cpp \
    toolmanager.cpp \
    eraser.cpp \
    erasetiles.cpp \
    saveasimagedialog.cpp \
    utils.cpp \
    colorbutton.cpp \
    undodock.cpp \
    selectiontool.cpp \
    abstracttiletool.cpp \
    abstracttool.cpp \
    changeselection.cpp \
    clipboardmanager.cpp \
    orthogonalrenderer.cpp \
    isometricrenderer.cpp \
    offsetlayer.cpp \
    offsetmapdialog.cpp \
    bucketfilltool.cpp \
    filltiles.cpp \
    objectgrouppropertiesdialog.cpp \
    changeobjectgroupproperties.cpp \
    zoomable.cpp \
    addremovetileset.cpp \
    movetileset.cpp
HEADERS += aboutdialog.h \
    brushitem.h \
    compression.h \
    languagemanager.h \
    layerdock.h \
    layermodel.h \
    mainwindow.h \
    mapreaderinterface.h \
    mapwriterinterface.h \
    mapdocument.h \
    mapdocumentactionhandler.h \
    mapobjectitem.h \
    mapscene.h \
    mapview.h \
    painttilelayer.h \
    pluginmanager.h \
    preferencesdialog.h \
    preferences.h \
    propertiesdialog.h \
    propertiesmodel.h \
    resizedialog.h \
    resizehelper.h \
    tileselectionitem.h \
    tilesetdock.h \
    tilesetmanager.h \
    tilesetmodel.h \
    tilesetview.h \
    tilelayeritem.h \
    tmxmapreader.h \
    tmxmapwriter.h \
    changeproperties.h \
    movelayer.h \
    tilepainter.h \
    newmapdialog.h \
    newtilesetdialog.h \
    objectgroupitem.h \
    movemapobject.h \
    resizemapobject.h \
    addremovemapobject.h \
    addremovelayer.h \
    propertiesview.h \
    renamelayer.h \
    resizelayer.h \
    resizemap.h \
    objectpropertiesdialog.h \
    changemapobject.h \
    maprenderer.h \
    abstracttool.h \
    stampbrush.h \
    toolmanager.h \
    eraser.h \
    erasetiles.h \
    saveasimagedialog.h \
    utils.h \
    colorbutton.h \
    undodock.h \
    selectiontool.h \
    abstracttiletool.h \
    changeselection.h \
    clipboardmanager.h \
    undocommands.h \
    orthogonalrenderer.h \
    isometricrenderer.h \
    offsetlayer.h \
    offsetmapdialog.h \
    bucketfilltool.h \
    filltiles.h \
    objectgrouppropertiesdialog.h \
    changeobjectgroupproperties.h \
    zoomable.h \
    addremovetileset.h \
    movetileset.h
FORMS += aboutdialog.ui \
    mainwindow.ui \
    resizedialog.ui \
    preferencesdialog.ui \
    propertiesdialog.ui \
    newmapdialog.ui \
    newtilesetdialog.ui \
    saveasimagedialog.ui \
    offsetmapdialog.ui \
    objectpropertiesdialog.ui
RESOURCES += tiled.qrc
mac {
    TARGET = Tiled
    LIBS += -lz
    QMAKE_INFO_PLIST = Info.plist
    ICON = images/tiled-icon-mac.icns
    contains(QT_CONFIG, ppc):CONFIG += x86 \
        ppc
    QMAKE_MAC_SDK = /Developer/SDKs/MacOSX10.5.sdk
}
win32:INCLUDEPATH += . $$(QTDIR)/src/3rdparty/zlib
contains(CONFIG, static) {
    DEFINES += STATIC_BUILD
    QTPLUGIN += qgif \
        qjpeg \
        qtiff
}
