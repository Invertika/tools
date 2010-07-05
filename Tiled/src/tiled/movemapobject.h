/*
 * movemapobject.h
 * Copyright 2009, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
 *
 * This file is part of Tiled.
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
 * this program. If not, see <http://www.gnu.org/licenses/>.
 */

#ifndef MOVEMAPOBJECT_H
#define MOVEMAPOBJECT_H

#include <QUndoCommand>
#include <QPointF>

namespace Tiled {

class MapObject;

namespace Internal {

class MapDocument;

class MoveMapObject : public QUndoCommand
{
public:
    MoveMapObject(MapDocument *mapDocument,
                  MapObject *mapObject,
                  const QPointF &oldPos);

    void undo();
    void redo();

private:
    MapDocument *mMapDocument;
    MapObject *mMapObject;
    QPointF mOldPos;
    QPointF mNewPos;
};

} // namespace Internal
} // namespace Tiled

#endif // MOVEMAPOBJECT_H
