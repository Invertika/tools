/****************************************************************************
** Meta object code from reading C++ file 'mapscene.h'
**
** Created: Wed Dec 23 18:10:53 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../mapscene.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'mapscene.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__MapScene[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
      13,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // slots: signature, parameters, type, tag, flags
      32,   27,   26,   26, 0x0a,
      69,   61,   26,   26, 0x0a,
      90,   26,   26,   26, 0x08,
     112,  105,   26,   26, 0x08,
     135,   26,   26,   26, 0x08,
     157,   26,   26,   26, 0x08,
     178,  170,   26,   26, 0x08,
     209,  203,   26,   26, 0x08,
     225,  203,   26,   26, 0x08,
     243,  203,   26,   26, 0x08,
     269,  261,   26,   26, 0x08,
     301,  261,   26,   26, 0x08,
     335,  261,   26,   26, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__MapScene[] = {
    "Tiled::Internal::MapScene\0\0tool\0"
    "setActiveTool(AbstractTool*)\0visible\0"
    "setGridVisible(bool)\0refreshScene()\0"
    "region\0repaintRegion(QRegion)\0"
    "currentLayerChanged()\0mapChanged()\0"
    "tileset\0tilesetChanged(Tileset*)\0index\0"
    "layerAdded(int)\0layerRemoved(int)\0"
    "layerChanged(int)\0objects\0"
    "objectsAdded(QList<MapObject*>)\0"
    "objectsRemoved(QList<MapObject*>)\0"
    "objectsChanged(QList<MapObject*>)\0"
};

const QMetaObject Tiled::Internal::MapScene::staticMetaObject = {
    { &QGraphicsScene::staticMetaObject, qt_meta_stringdata_Tiled__Internal__MapScene,
      qt_meta_data_Tiled__Internal__MapScene, 0 }
};

const QMetaObject *Tiled::Internal::MapScene::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::MapScene::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__MapScene))
        return static_cast<void*>(const_cast< MapScene*>(this));
    return QGraphicsScene::qt_metacast(_clname);
}

int Tiled::Internal::MapScene::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QGraphicsScene::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: setActiveTool((*reinterpret_cast< AbstractTool*(*)>(_a[1]))); break;
        case 1: setGridVisible((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 2: refreshScene(); break;
        case 3: repaintRegion((*reinterpret_cast< const QRegion(*)>(_a[1]))); break;
        case 4: currentLayerChanged(); break;
        case 5: mapChanged(); break;
        case 6: tilesetChanged((*reinterpret_cast< Tileset*(*)>(_a[1]))); break;
        case 7: layerAdded((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 8: layerRemoved((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 9: layerChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: objectsAdded((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        case 11: objectsRemoved((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        case 12: objectsChanged((*reinterpret_cast< const QList<MapObject*>(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 13;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
