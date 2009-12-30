/****************************************************************************
** Meta object code from reading C++ file 'abstracttiletool.h'
**
** Created: Wed Dec 23 18:11:58 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../abstracttiletool.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'abstracttiletool.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__AbstractTileTool[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       1,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // slots: signature, parameters, type, tag, flags
      35,   34,   34,   34, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__AbstractTileTool[] = {
    "Tiled::Internal::AbstractTileTool\0\0"
    "updateBrushVisibility()\0"
};

const QMetaObject Tiled::Internal::AbstractTileTool::staticMetaObject = {
    { &AbstractTool::staticMetaObject, qt_meta_stringdata_Tiled__Internal__AbstractTileTool,
      qt_meta_data_Tiled__Internal__AbstractTileTool, 0 }
};

const QMetaObject *Tiled::Internal::AbstractTileTool::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::AbstractTileTool::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__AbstractTileTool))
        return static_cast<void*>(const_cast< AbstractTileTool*>(this));
    return AbstractTool::qt_metacast(_clname);
}

int Tiled::Internal::AbstractTileTool::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = AbstractTool::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: updateBrushVisibility(); break;
        default: ;
        }
        _id -= 1;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
