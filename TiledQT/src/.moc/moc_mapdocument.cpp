/****************************************************************************
** Meta object code from reading C++ file 'mapdocument.h'
**
** Created: Wed Dec 23 18:10:52 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../mapdocument.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'mapdocument.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__MapDocument[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
      12,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      30,   29,   29,   29, 0x05,
      49,   43,   29,   29, 0x05,
      65,   43,   29,   29, 0x05,
      83,   43,   29,   29, 0x05,
     101,   43,   29,   29, 0x05,
     133,  126,   29,   29, 0x05,
     164,  156,   29,   29, 0x05,
     195,  187,   29,   29, 0x05,
     227,  187,   29,   29, 0x05,
     261,  187,   29,   29, 0x05,

 // slots: signature, parameters, type, tag, flags
     295,   43,   29,   29, 0x08,
     313,   43,   29,   29, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__MapDocument[] = {
    "Tiled::Internal::MapDocument\0\0"
    "mapChanged()\0index\0layerAdded(int)\0"
    "layerRemoved(int)\0layerChanged(int)\0"
    "currentLayerChanged(int)\0region\0"
    "regionChanged(QRegion)\0tileset\0"
    "tilesetAdded(Tileset*)\0objects\0"
    "objectsAdded(QList<MapObject*>)\0"
    "objectsRemoved(QList<MapObject*>)\0"
    "objectsChanged(QList<MapObject*>)\0"
    "onLayerAdded(int)\0onLayerRemoved(int)\0"
};

const QMetaObject Tiled::Internal::MapDocument::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_Tiled__Internal__MapDocument,
      qt_meta_data_Tiled__Internal__MapDocument, 0 }
};

const QMetaObject *Tiled::Internal::MapDocument::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::MapDocument::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__MapDocument))
        return static_cast<void*>(const_cast< MapDocument*>(this));
    return QObject::qt_metacast(_clname);
}

int Tiled::Internal::MapDocument::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: mapChanged(); break;
        case 1: layerAdded((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 2: layerRemoved((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 3: layerChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 4: currentLayerChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 5: regionChanged((*reinterpret_cast< const QRegion(*)>(_a[1]))); break;
        case 6: tilesetAdded((*reinterpret_cast< Tileset*(*)>(_a[1]))); break;
        case 7: objectsAdded((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        case 8: objectsRemoved((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        case 9: objectsChanged((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        case 10: onLayerAdded((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 11: onLayerRemoved((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 12;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::MapDocument::mapChanged()
{
    QMetaObject::activate(this, &staticMetaObject, 0, 0);
}

// SIGNAL 1
void Tiled::Internal::MapDocument::layerAdded(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void Tiled::Internal::MapDocument::layerRemoved(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void Tiled::Internal::MapDocument::layerChanged(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void Tiled::Internal::MapDocument::currentLayerChanged(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}

// SIGNAL 5
void Tiled::Internal::MapDocument::regionChanged(const QRegion & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 5, _a);
}

// SIGNAL 6
void Tiled::Internal::MapDocument::tilesetAdded(Tileset * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 6, _a);
}

// SIGNAL 7
void Tiled::Internal::MapDocument::objectsAdded(const QList<MapObject*> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 7, _a);
}

// SIGNAL 8
void Tiled::Internal::MapDocument::objectsRemoved(const QList<MapObject*> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 8, _a);
}

// SIGNAL 9
void Tiled::Internal::MapDocument::objectsChanged(const QList<MapObject*> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 9, _a);
}
QT_END_MOC_NAMESPACE
