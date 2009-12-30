/****************************************************************************
** Meta object code from reading C++ file 'stampbrush.h'
**
** Created: Wed Dec 23 18:11:35 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../stampbrush.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'stampbrush.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__StampBrush[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       0,    0, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__StampBrush[] = {
    "Tiled::Internal::StampBrush\0"
};

const QMetaObject Tiled::Internal::StampBrush::staticMetaObject = {
    { &AbstractTileTool::staticMetaObject, qt_meta_stringdata_Tiled__Internal__StampBrush,
      qt_meta_data_Tiled__Internal__StampBrush, 0 }
};

const QMetaObject *Tiled::Internal::StampBrush::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::StampBrush::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__StampBrush))
        return static_cast<void*>(const_cast< StampBrush*>(this));
    return AbstractTileTool::qt_metacast(_clname);
}

int Tiled::Internal::StampBrush::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = AbstractTileTool::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    return _id;
}
QT_END_MOC_NAMESPACE
