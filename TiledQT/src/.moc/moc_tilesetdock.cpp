/****************************************************************************
** Meta object code from reading C++ file 'tilesetdock.h'
**
** Created: Wed Dec 23 18:11:17 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../tilesetdock.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'tilesetdock.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__TilesetDock[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       4,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      36,   30,   29,   29, 0x05,

 // slots: signature, parameters, type, tag, flags
      82,   74,   29,   29, 0x08,
     107,   29,   29,   29, 0x08,
     126,   74,   29,   29, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__TilesetDock[] = {
    "Tiled::Internal::TilesetDock\0\0tiles\0"
    "currentTilesChanged(const TileLayer*)\0"
    "tileset\0addTilesetView(Tileset*)\0"
    "selectionChanged()\0tilesetChanged(Tileset*)\0"
};

const QMetaObject Tiled::Internal::TilesetDock::staticMetaObject = {
    { &QDockWidget::staticMetaObject, qt_meta_stringdata_Tiled__Internal__TilesetDock,
      qt_meta_data_Tiled__Internal__TilesetDock, 0 }
};

const QMetaObject *Tiled::Internal::TilesetDock::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::TilesetDock::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__TilesetDock))
        return static_cast<void*>(const_cast< TilesetDock*>(this));
    return QDockWidget::qt_metacast(_clname);
}

int Tiled::Internal::TilesetDock::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDockWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: currentTilesChanged((*reinterpret_cast< const TileLayer*(*)>(_a[1]))); break;
        case 1: addTilesetView((*reinterpret_cast< Tileset*(*)>(_a[1]))); break;
        case 2: selectionChanged(); break;
        case 3: tilesetChanged((*reinterpret_cast< Tileset*(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 4;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::TilesetDock::currentTilesChanged(const TileLayer * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
