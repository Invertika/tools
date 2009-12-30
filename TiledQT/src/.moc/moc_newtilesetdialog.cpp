/****************************************************************************
** Meta object code from reading C++ file 'newtilesetdialog.h'
**
** Created: Wed Dec 23 18:11:26 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../newtilesetdialog.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'newtilesetdialog.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__NewTilesetDialog[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // slots: signature, parameters, type, tag, flags
      35,   34,   34,   34, 0x08,
      49,   44,   34,   34, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__NewTilesetDialog[] = {
    "Tiled::Internal::NewTilesetDialog\0\0"
    "browse()\0name\0nameEdited(QString)\0"
};

const QMetaObject Tiled::Internal::NewTilesetDialog::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_Tiled__Internal__NewTilesetDialog,
      qt_meta_data_Tiled__Internal__NewTilesetDialog, 0 }
};

const QMetaObject *Tiled::Internal::NewTilesetDialog::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::NewTilesetDialog::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__NewTilesetDialog))
        return static_cast<void*>(const_cast< NewTilesetDialog*>(this));
    return QDialog::qt_metacast(_clname);
}

int Tiled::Internal::NewTilesetDialog::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: browse(); break;
        case 1: nameEdited((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 2;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
