/****************************************************************************
** Meta object code from reading C++ file 'mainwindow.h'
**
** Created: Wed Dec 23 18:10:48 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../mainwindow.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'mainwindow.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__MainWindow[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
      30,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // slots: signature, parameters, type, tag, flags
      29,   28,   28,   28, 0x08,
      38,   28,   28,   28, 0x08,
      54,   28,   49,   28, 0x08,
      65,   28,   49,   28, 0x08,
      78,   28,   28,   28, 0x08,
      92,   28,   28,   28, 0x08,
     104,   28,   28,   28, 0x08,
     110,   28,   28,   28, 0x08,
     117,   28,   28,   28, 0x08,
     125,   28,   28,   28, 0x08,
     138,   28,   28,   28, 0x08,
     150,   28,   28,   28, 0x08,
     162,   28,   28,   28, 0x08,
     182,   28,   28,   28, 0x08,
     199,   28,   28,   28, 0x08,
     221,  215,   28,   28, 0x08,
     244,   28,   28,   28, 0x08,
     257,   28,   28,   28, 0x08,
     274,   28,   28,   28, 0x08,
     293,   28,   28,   28, 0x08,
     305,   28,   28,   28, 0x08,
     318,   28,   28,   28, 0x08,
     333,   28,   28,   28, 0x08,
     350,   28,   28,   28, 0x08,
     367,   28,   28,   28, 0x08,
     381,   28,   28,   28, 0x08,
     397,   28,   28,   28, 0x08,
     411,   28,   28,   28, 0x08,
     439,  433,   28,   28, 0x08,
     482,  471,   28,   28, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__MainWindow[] = {
    "Tiled::Internal::MainWindow\0\0newMap()\0"
    "openFile()\0bool\0saveFile()\0saveFileAs()\0"
    "saveAsImage()\0closeFile()\0cut()\0copy()\0"
    "paste()\0newTileset()\0resizeMap()\0"
    "offsetMap()\0editMapProperties()\0"
    "updateModified()\0updateActions()\0scale\0"
    "updateZoomLabel(qreal)\0aboutTiled()\0"
    "openRecentFile()\0clearRecentFiles()\0"
    "selectAll()\0selectNone()\0addTileLayer()\0"
    "addObjectLayer()\0duplicateLayer()\0"
    "moveLayerUp()\0moveLayerDown()\0"
    "removeLayer()\0editLayerProperties()\0"
    "tiles\0setStampBrush(const TileLayer*)\0"
    "statusInfo\0updateStatusInfoLabel(QString)\0"
};

const QMetaObject Tiled::Internal::MainWindow::staticMetaObject = {
    { &QMainWindow::staticMetaObject, qt_meta_stringdata_Tiled__Internal__MainWindow,
      qt_meta_data_Tiled__Internal__MainWindow, 0 }
};

const QMetaObject *Tiled::Internal::MainWindow::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::MainWindow::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__MainWindow))
        return static_cast<void*>(const_cast< MainWindow*>(this));
    return QMainWindow::qt_metacast(_clname);
}

int Tiled::Internal::MainWindow::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QMainWindow::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: newMap(); break;
        case 1: openFile(); break;
        case 2: { bool _r = saveFile();
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 3: { bool _r = saveFileAs();
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 4: saveAsImage(); break;
        case 5: closeFile(); break;
        case 6: cut(); break;
        case 7: copy(); break;
        case 8: paste(); break;
        case 9: newTileset(); break;
        case 10: resizeMap(); break;
        case 11: offsetMap(); break;
        case 12: editMapProperties(); break;
        case 13: updateModified(); break;
        case 14: updateActions(); break;
        case 15: updateZoomLabel((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 16: aboutTiled(); break;
        case 17: openRecentFile(); break;
        case 18: clearRecentFiles(); break;
        case 19: selectAll(); break;
        case 20: selectNone(); break;
        case 21: addTileLayer(); break;
        case 22: addObjectLayer(); break;
        case 23: duplicateLayer(); break;
        case 24: moveLayerUp(); break;
        case 25: moveLayerDown(); break;
        case 26: removeLayer(); break;
        case 27: editLayerProperties(); break;
        case 28: setStampBrush((*reinterpret_cast< const TileLayer*(*)>(_a[1]))); break;
        case 29: updateStatusInfoLabel((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 30;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
