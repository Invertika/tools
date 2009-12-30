/****************************************************************************
** Meta object code from reading C++ file 'mapview.h'
**
** Created: Wed Dec 23 18:10:57 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../mapview.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'mapview.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__MapView[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       4,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      32,   26,   25,   25, 0x05,

 // slots: signature, parameters, type, tag, flags
      52,   25,   25,   25, 0x0a,
      61,   25,   25,   25, 0x0a,
      71,   25,   25,   25, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__MapView[] = {
    "Tiled::Internal::MapView\0\0scale\0"
    "scaleChanged(qreal)\0zoomIn()\0zoomOut()\0"
    "resetZoom()\0"
};

const QMetaObject Tiled::Internal::MapView::staticMetaObject = {
    { &QGraphicsView::staticMetaObject, qt_meta_stringdata_Tiled__Internal__MapView,
      qt_meta_data_Tiled__Internal__MapView, 0 }
};

const QMetaObject *Tiled::Internal::MapView::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::MapView::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__MapView))
        return static_cast<void*>(const_cast< MapView*>(this));
    return QGraphicsView::qt_metacast(_clname);
}

int Tiled::Internal::MapView::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QGraphicsView::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: scaleChanged((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 1: zoomIn(); break;
        case 2: zoomOut(); break;
        case 3: resetZoom(); break;
        default: ;
        }
        _id -= 4;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::MapView::scaleChanged(qreal _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
