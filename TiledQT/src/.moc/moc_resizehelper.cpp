/****************************************************************************
** Meta object code from reading C++ file 'resizehelper.h'
**
** Created: Wed Dec 23 18:11:08 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../resizehelper.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'resizehelper.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__ResizeHelper[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
      11,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      38,   31,   30,   30, 0x05,
      66,   60,   30,   30, 0x05,
      86,   60,   30,   30, 0x05,
     113,  106,   30,   30, 0x05,

 // slots: signature, parameters, type, tag, flags
     145,  140,   30,   30, 0x0a,
     163,  140,   30,   30, 0x0a,
     181,   31,   30,   30, 0x0a,
     201,  199,   30,   30, 0x0a,
     219,  217,   30,   30, 0x0a,
     241,  235,   30,   30, 0x0a,
     265,  258,   30,   30, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__ResizeHelper[] = {
    "Tiled::Internal::ResizeHelper\0\0offset\0"
    "offsetChanged(QPoint)\0value\0"
    "offsetXChanged(int)\0offsetYChanged(int)\0"
    "bounds\0offsetBoundsChanged(QRect)\0"
    "size\0setOldSize(QSize)\0setNewSize(QSize)\0"
    "setOffset(QPoint)\0x\0setOffsetX(int)\0"
    "y\0setOffsetY(int)\0width\0setNewWidth(int)\0"
    "height\0setNewHeight(int)\0"
};

const QMetaObject Tiled::Internal::ResizeHelper::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_Tiled__Internal__ResizeHelper,
      qt_meta_data_Tiled__Internal__ResizeHelper, 0 }
};

const QMetaObject *Tiled::Internal::ResizeHelper::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::ResizeHelper::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__ResizeHelper))
        return static_cast<void*>(const_cast< ResizeHelper*>(this));
    return QWidget::qt_metacast(_clname);
}

int Tiled::Internal::ResizeHelper::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: offsetChanged((*reinterpret_cast< const QPoint(*)>(_a[1]))); break;
        case 1: offsetXChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 2: offsetYChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 3: offsetBoundsChanged((*reinterpret_cast< const QRect(*)>(_a[1]))); break;
        case 4: setOldSize((*reinterpret_cast< const QSize(*)>(_a[1]))); break;
        case 5: setNewSize((*reinterpret_cast< const QSize(*)>(_a[1]))); break;
        case 6: setOffset((*reinterpret_cast< const QPoint(*)>(_a[1]))); break;
        case 7: setOffsetX((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 8: setOffsetY((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 9: setNewWidth((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: setNewHeight((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 11;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::ResizeHelper::offsetChanged(const QPoint & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void Tiled::Internal::ResizeHelper::offsetXChanged(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void Tiled::Internal::ResizeHelper::offsetYChanged(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void Tiled::Internal::ResizeHelper::offsetBoundsChanged(const QRect & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}
QT_END_MOC_NAMESPACE
