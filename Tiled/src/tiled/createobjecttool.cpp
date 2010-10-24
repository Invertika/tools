/*
 * createobjecttool.cpp
 * Copyright 2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
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

#include "createobjecttool.h"

#include "addremovemapobject.h"
#include "map.h"
#include "mapdocument.h"
#include "mapobject.h"
#include "mapobjectitem.h"
#include "maprenderer.h"
#include "mapscene.h"
#include "objectgroup.h"
#include "objectgroupitem.h"
#include "tile.h"

using namespace Tiled;
using namespace Tiled::Internal;

CreateObjectTool::CreateObjectTool(CreationMode mode, QObject *parent)
    : AbstractTool(tr("Create Objects"),
                   QIcon(QLatin1String(
                           ":images/22x22/tool-create-object.png")),
                   QKeySequence(tr("O")),
                   parent)
    , mMapScene(0)
    , mNewMapObjectItem(0)
    , mTile(0)
    , mMode(mode)
{
    if (mMode == TileObjects) {
        setIcon(QIcon(QLatin1String(
                          ":images/22x22/tool-create-tile-object.png")));
        setName(tr("Create Tile Objects"));
        setShortcut(QKeySequence(tr("T")));
    }
}

void CreateObjectTool::activate(MapScene *scene)
{
    mMapScene = scene;
}

void CreateObjectTool::deactivate(MapScene *)
{
    mMapScene = 0;
}

void CreateObjectTool::mouseEntered()
{
}

void CreateObjectTool::mouseLeft()
{
}

void CreateObjectTool::mouseMoved(const QPointF &pos,
                                  Qt::KeyboardModifiers modifiers)
{
    if (!mNewMapObjectItem)
        return;

    const MapRenderer *renderer = mMapScene->mapDocument()->renderer();
    QPointF tileCoords = renderer->pixelToTileCoords(pos);

    if (mMode == AreaObjects) {
        // Update the size of the new map object
        const QPointF objectPos = mNewMapObjectItem->mapObject()->position();
        QSizeF newSize(qMax(qreal(0), tileCoords.x() - objectPos.x()),
                       qMax(qreal(0), tileCoords.y() - objectPos.y()));

        if (modifiers & Qt::ControlModifier)
            newSize = newSize.toSize();

        mNewMapObjectItem->resize(newSize);
    } else {
        if (modifiers & Qt::ControlModifier)
            tileCoords = tileCoords.toPoint();

        mNewMapObjectItem->mapObject()->setPosition(tileCoords);
        mNewMapObjectItem->syncWithMapObject();
    }
}

void CreateObjectTool::mousePressed(const QPointF &pos,
                                    Qt::MouseButton button,
                                    Qt::KeyboardModifiers modifiers)
{
    // Check if we are already creating a new map object
    if (mNewMapObjectItem) {
        if (button == Qt::RightButton)
            cancelNewMapObject();
        return;
    }

    if (button != Qt::LeftButton)
        return;

    ObjectGroup *objectGroup = currentObjectGroup();
    if (objectGroup && objectGroup->isVisible() && !mNewMapObjectItem) {
        const MapRenderer *renderer = mMapScene->mapDocument()->renderer();

        QPointF tileCoords = renderer->pixelToTileCoords(pos);
        if (modifiers & Qt::ControlModifier)
            tileCoords = tileCoords.toPoint();

        startNewMapObject(tileCoords, objectGroup);
    }
}

void CreateObjectTool::mouseReleased(const QPointF &pos,
                                     Qt::MouseButton button)
{
    Q_UNUSED(pos)

    if (button == Qt::LeftButton && mNewMapObjectItem)
        finishNewMapObject();
}

void CreateObjectTool::languageChanged()
{
    setName(tr("Create Objects"));
    setShortcut(QKeySequence(tr("O")));
}

void CreateObjectTool::updateEnabledState()
{
    setEnabled(currentObjectGroup() != 0);
}

void CreateObjectTool::startNewMapObject(const QPointF &pos,
                                         ObjectGroup *objectGroup)
{
    Q_ASSERT(!mNewMapObjectItem);

    if (mMode == TileObjects && !mTile)
        return;

    MapObject *newMapObject = new MapObject;
    newMapObject->setPosition(pos);

    if (mMode == TileObjects)
        newMapObject->setTile(mTile);

    objectGroup->addObject(newMapObject);

    // It is currently necessary to disable editing of the currently selected
    // object group, because its objects may otherwise steal mouse events
    // during creation.
    mMapScene->selectedObjectGroupItem()->setEditable(false);

    mNewMapObjectItem = new MapObjectItem(newMapObject,
                                          mMapScene->mapDocument());
    mNewMapObjectItem->setZValue(10000);
    mMapScene->addItem(mNewMapObjectItem);
}

MapObject *CreateObjectTool::clearNewMapObjectItem()
{
    Q_ASSERT(mNewMapObjectItem);

    MapObject *newMapObject = mNewMapObjectItem->mapObject();

    ObjectGroup *objectGroup = newMapObject->objectGroup();
    objectGroup->removeObject(newMapObject);

    delete mNewMapObjectItem;
    mNewMapObjectItem = 0;

    mMapScene->selectedObjectGroupItem()->setEditable(true);

    return newMapObject;
}

void CreateObjectTool::cancelNewMapObject()
{
    MapObject *newMapObject = clearNewMapObjectItem();
    delete newMapObject;
}

void CreateObjectTool::finishNewMapObject()
{
    Q_ASSERT(mNewMapObjectItem);
    MapObject *newMapObject = mNewMapObjectItem->mapObject();
    ObjectGroup *objectGroup = newMapObject->objectGroup();
    clearNewMapObjectItem();

    MapDocument *mapDocument = mMapScene->mapDocument();
    mapDocument->undoStack()->push(new AddMapObject(mapDocument,
                                                    objectGroup,
                                                    newMapObject));
}

ObjectGroup *CreateObjectTool::currentObjectGroup() const
{
    if (!mapDocument())
        return 0;

    const int currentLayerIndex = mapDocument()->currentLayer();
    if (currentLayerIndex < 0)
        return 0;
    Layer *currentLayer = mapDocument()->map()->layerAt(currentLayerIndex);
    return dynamic_cast<ObjectGroup*>(currentLayer);
}
