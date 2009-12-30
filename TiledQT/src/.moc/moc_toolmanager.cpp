/****************************************************************************
** Meta object code from reading C++ file 'toolmanager.h'
**
** Created: Wed Dec 23 18:11:38 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../toolmanager.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'toolmanager.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__ToolManager[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       3,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      35,   30,   29,   29, 0x05,
      75,   70,   29,   29, 0x05,

 // slots: signature, parameters, type, tag, flags
     109,  102,   29,   29, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__ToolManager[] = {
    "Tiled::Internal::ToolManager\0\0tool\0"
    "selectedToolChanged(AbstractTool*)\0"
    "info\0statusInfoChanged(QString)\0action\0"
    "actionTriggered(QAction*)\0"
};

const QMetaObject Tiled::Internal::ToolManager::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_Tiled__Internal__ToolManager,
      qt_meta_data_Tiled__Internal__ToolManager, 0 }
};

const QMetaObject *Tiled::Internal::ToolManager::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::ToolManager::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__ToolManager))
        return static_cast<void*>(const_cast< ToolManager*>(this));
    return QObject::qt_metacast(_clname);
}

int Tiled::Internal::ToolManager::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: selectedToolChanged((*reinterpret_cast< AbstractTool*(*)>(_a[1]))); break;
        case 1: statusInfoChanged((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        case 2: actionTriggered((*reinterpret_cast< QAction*(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 3;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::ToolManager::selectedToolChanged(AbstractTool * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void Tiled::Internal::ToolManager::statusInfoChanged(const QString & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}
QT_END_MOC_NAMESPACE
