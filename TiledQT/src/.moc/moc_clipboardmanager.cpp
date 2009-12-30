/****************************************************************************
** Meta object code from reading C++ file 'clipboardmanager.h'
**
** Created: Wed Dec 23 18:12:02 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../clipboardmanager.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'clipboardmanager.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__ClipboardManager[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // signals: signature, parameters, type, tag, flags
      35,   34,   34,   34, 0x05,

 // slots: signature, parameters, type, tag, flags
      51,   34,   34,   34, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__ClipboardManager[] = {
    "Tiled::Internal::ClipboardManager\0\0"
    "hasMapChanged()\0updateHasMap()\0"
};

const QMetaObject Tiled::Internal::ClipboardManager::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_Tiled__Internal__ClipboardManager,
      qt_meta_data_Tiled__Internal__ClipboardManager, 0 }
};

const QMetaObject *Tiled::Internal::ClipboardManager::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::ClipboardManager::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__ClipboardManager))
        return static_cast<void*>(const_cast< ClipboardManager*>(this));
    return QObject::qt_metacast(_clname);
}

int Tiled::Internal::ClipboardManager::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: hasMapChanged(); break;
        case 1: updateHasMap(); break;
        default: ;
        }
        _id -= 2;
    }
    return _id;
}

// SIGNAL 0
void Tiled::Internal::ClipboardManager::hasMapChanged()
{
    QMetaObject::activate(this, &staticMetaObject, 0, 0);
}
QT_END_MOC_NAMESPACE
