/****************************************************************************
** Meta object code from reading C++ file 'colorbutton.h'
**
** Created: Wed Dec 23 18:11:47 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../colorbutton.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'colorbutton.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__ColorButton[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      36,   30,   29,   29, 0x05,

 // slots: signature, parameters, type, tag, flags
      57,   29,   29,   29, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__ColorButton[] = {
    "Tiled::Internal::ColorButton\0\0color\0"
    "colorChanged(QColor)\0pickColor()\0"
};

const QMetaObject Tiled::Internal::ColorButton::staticMetaObject = {
    { &QToolButton::staticMetaObject, qt_meta_stringdata_Tiled__Internal__ColorButton,
      qt_meta_data_Tiled__Internal__ColorButton, 0 }
};

const QMetaObject *Tiled::Internal::ColorButton::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::ColorButton::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__ColorButton))
        return static_cast<void*>(const_cast< ColorButton*>(this));
    return QToolButton::qt_metacast(_clname);
}

int Tiled::Internal::ColorButton::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QToolButton::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: colorChanged((*reinterpret_cast< const QColor(*)>(_a[1]))); break;
        case 1: pickColor(); break;
        default: ;
        }
        _id -= 2;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::ColorButton::colorChanged(const QColor & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
