/*
 * Tiled Map Editor (Qt)
 * Copyright 2009 Tiled (Qt) developers (see AUTHORS file)
 *
 * This file is part of Tiled (Qt).
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
 * more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * this program; if not, write to the Free Software Foundation, Inc., 59 Temple
 * Place, Suite 330, Boston, MA 02111-1307, USA.
 */

#include "objectpropertiesdialog.h"

#include "changemapobject.h"
#include "mapdocument.h"
#include "mapobject.h"
#include "movemapobject.h"
#include "resizemapobject.h"

#include <QGridLayout>
#include <QLabel>
#include <QLineEdit>
#include <QUndoStack>

using namespace Tiled;
using namespace Tiled::Internal;

ObjectPropertiesDialog::ObjectPropertiesDialog(MapDocument *mapDocument,
                                               MapObject *mapObject,
                                               QWidget *parent)
    : PropertiesDialog(tr("Object"),
                       mapObject->properties(),
                       mapDocument->undoStack(),
                       parent)
    , mMapDocument(mapDocument)
    , mMapObject(mapObject)
    , mUi(new Ui::ObjectPropertiesDialog)
{
    QWidget *widget = new QWidget;
    mUi->setupUi(widget);

    // Initialize UI with values from the map-object
    mUi->name->setText(mMapObject->name());
    mUi->type->setText(mMapObject->type());
    mUi->x->setValue(mMapObject->x());
    mUi->y->setValue(mMapObject->y());
    mUi->width->setValue(mMapObject->width());
    mUi->height->setValue(mMapObject->height());

    qobject_cast<QBoxLayout*>(layout())->insertWidget(0, widget);

    mUi->name->setFocus();

    // Resize the dialog to its recommended size
    resize(sizeHint());
}

void ObjectPropertiesDialog::accept()
{
    const QString newName = mUi->name->text();
    const QString newType = mUi->type->text();

    const qreal newPosX = mUi->x->value();
    const qreal newPosY = mUi->y->value();
    const qreal newWidth = mUi->width->value();
    const qreal newHeight = mUi->height->value();

    bool changed = false;
    changed |= mMapObject->name() != newName;
    changed |= mMapObject->type() != newType;
    changed |= mMapObject->x() != newPosX;
    changed |= mMapObject->y() != newPosY;
    changed |= mMapObject->width() != newWidth;
    changed |= mMapObject->height() != newHeight;

    if (changed) {
        QUndoStack *undo = mMapDocument->undoStack();
        undo->beginMacro(tr("Change Object"));
        undo->push(new ChangeMapObject(mMapDocument, mMapObject,
                                       newName, newType));

        const QPointF oldPos = mMapObject->position();
        mMapObject->setX(newPosX);
        mMapObject->setY(newPosY);
        undo->push(new MoveMapObject(mMapDocument, mMapObject, oldPos));

        const QSizeF oldSize = mMapObject->size();
        mMapObject->setWidth(newWidth);
        mMapObject->setHeight(newHeight);
        undo->push(new ResizeMapObject(mMapDocument, mMapObject, oldSize));

        PropertiesDialog::accept(); // Let PropertiesDialog add its command
        undo->endMacro();
    } else {
        PropertiesDialog::accept();
    }
}
