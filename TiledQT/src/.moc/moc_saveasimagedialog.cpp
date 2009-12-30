/****************************************************************************
** Meta object code from reading C++ file 'saveasimagedialog.h'
**
** Created: Wed Dec 23 18:11:43 2009
**      by: The Qt Meta Object Compiler version 61 (Qt 4.5.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../saveasimagedialog.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'saveasimagedialog.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 61
#error "This file was generated using the moc from 4.5.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Tiled__Internal__SaveAsImageDialog[] = {

 // content:
       2,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   12, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors

 // slots: signature, parameters, type, tag, flags
      36,   35,   35,   35, 0x08,
      45,   35,   35,   35, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_Tiled__Internal__SaveAsImageDialog[] = {
    "Tiled::Internal::SaveAsImageDialog\0\0"
    "browse()\0updateAcceptEnabled()\0"
};

const QMetaObject Tiled::Internal::SaveAsImageDialog::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_Tiled__Internal__SaveAsImageDialog,
      qt_meta_data_Tiled__Internal__SaveAsImageDialog, 0 }
};

const QMetaObject *Tiled::Internal::SaveAsImageDialog::metaObject() const
{
    return &staticMetaObject;
}

void *Tiled::Internal::SaveAsImageDialog::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Tiled__Internal__SaveAsImageDialog))
        return static_cast<void*>(const_cast< SaveAsImageDialog*>(this));
    return QDialog::qt_metacast(_clname);
}

int Tiled::Internal::SaveAsImageDialog::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: browse(); break;
        case 1: updateAcceptEnabled(); break;
        default: ;
        }
        _id -= 2;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
