/*
 * Tiled Map Editor (Qt)
 * Copyright 2008 Tiled (Qt) developers (see AUTHORS file)
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

#include "mapdocument.h"

#include "addremovelayer.h"
#include "isometricrenderer.h"
#include "layermodel.h"
#include "map.h"
#include "movelayer.h"
#include "objectgroup.h"
#include "offsetlayer.h"
#include "orthogonalrenderer.h"
#include "resizelayer.h"
#include "resizemap.h"
#include "tilelayer.h"
#include "tilesetmanager.h"

#include <QRect>
#include <QUndoStack>

using namespace Tiled;
using namespace Tiled::Internal;

MapDocument::MapDocument(Map *map):
    mMap(map),
    mLayerModel(new LayerModel(this)),
    mUndoStack(new QUndoStack(this))
{
    switch (map->orientation()) {
    case Map::Isometric:
        mRenderer = new IsometricRenderer(map);
        break;
    default:
        mRenderer = new OrthogonalRenderer(map);
        break;
    }

    mCurrentLayer = (map->layerCount() == 0) ? -1 : 0;
    mLayerModel->setMapDocument(this);

    // Forward signals emitted from the layer model
    connect(mLayerModel, SIGNAL(layerAdded(int)), SLOT(onLayerAdded(int)));
    connect(mLayerModel, SIGNAL(layerRemoved(int)), SLOT(onLayerRemoved(int)));
    connect(mLayerModel, SIGNAL(layerChanged(int)), SIGNAL(layerChanged(int)));

    // Register tileset references
    TilesetManager *tilesetManager = TilesetManager::instance();
    foreach (Tileset *tileset, mMap->tilesets())
        tilesetManager->addReference(tileset);
}

MapDocument::~MapDocument()
{
    // Unregister tileset references
    TilesetManager *tilesetManager = TilesetManager::instance();
    foreach (Tileset *tileset, mMap->tilesets())
        tilesetManager->removeReference(tileset);

    delete mRenderer;
    delete mMap;
}

void MapDocument::setCurrentLayer(int index)
{
    Q_ASSERT(index >= -1 && index < mMap->layerCount());
    mCurrentLayer = index;

    /* This function always sends the following signal, even if the index
     * didn't actually change. This is because the selected index in the layer
     * table view might be out of date anyway, and would otherwise not be
     * properly updated.
     *
     * This problem happens due to the selection model not sending signals
     * about changes to its current index when it is due to insertion/removal
     * of other items. The selected item doesn't change in that case, but our
     * layer index does.
     */
    emit currentLayerChanged(mCurrentLayer);
}

int MapDocument::currentLayer() const
{
    return mCurrentLayer;
}

void MapDocument::resizeMap(const QSize &size, const QPoint &offset)
{
    // Resize the map and each layer
    mUndoStack->beginMacro(tr("Resize Map"));
    for (int i = 0; i < mMap->layerCount(); ++i)
        mUndoStack->push(new ResizeLayer(this, i, size, offset));
    mUndoStack->push(new ResizeMap(this, size));
    mUndoStack->endMacro();

    // TODO: Handle layers that don't match the map size correctly
    // TODO: Objects that fall outside of the map should be deleted
}

void MapDocument::offsetMap(const QList<int> &layerIndexes,
                            const QPoint &offset,
                            const QRect &bounds,
                            bool wrapX, bool wrapY)
{
    if (layerIndexes.empty())
        return;

    if (layerIndexes.size() == 1) {
        mUndoStack->push(new OffsetLayer(this, layerIndexes.first(), offset,
                                         bounds, wrapX, wrapY));
    } else {
        mUndoStack->beginMacro(tr("Offset Map"));
        foreach (int layerIndex, layerIndexes) {
            mUndoStack->push(new OffsetLayer(this, layerIndex, offset,
                                             bounds, wrapX, wrapY));
        }
        mUndoStack->endMacro();
    }
}

/**
 * Adds a layer of the given type to the top of the layer stack.
 */
void MapDocument::addLayer(LayerType layerType, const QString &name)
{
    Layer *layer;
    switch (layerType) {
    case TileLayerType:
        layer = new TileLayer(name, 0, 0, mMap->width(), mMap->height());
        break;
    case ObjectLayerType:
        layer = new ObjectGroup(name, 0, 0, mMap->width(), mMap->height());
        break;
    }

    const int index = mMap->layerCount();
    mUndoStack->push(new AddLayer(this, index, layer));
    setCurrentLayer(index);
}

/**
 * Duplicates the currently selected layer.
 */
void MapDocument::duplicateLayer()
{
    if (mCurrentLayer == -1)
        return;

    Layer *duplicate = mMap->layerAt(mCurrentLayer)->clone();
    duplicate->setName(tr("Copy of %1").arg(duplicate->name()));

    const int index = mCurrentLayer + 1;
    QUndoCommand *cmd = new AddLayer(this, index, duplicate);
    cmd->setText(tr("Duplicate Layer"));
    mUndoStack->push(cmd);
    setCurrentLayer(index);
}

/**
 * Moves the given layer up. Does nothing when no valid layer index is
 * given.
 */
void MapDocument::moveLayerUp(int index)
{
    if (index < 0 || index >= mMap->layerCount() - 1)
        return;

    mUndoStack->push(new MoveLayer(this, index, MoveLayer::Up));
}

/**
 * Moves the given layer down. Does nothing when no valid layer index is
 * given.
 */
void MapDocument::moveLayerDown(int index)
{
    if (index < 1 || index >= mMap->layerCount())
        return;

    mUndoStack->push(new MoveLayer(this, index, MoveLayer::Down));
}

/**
 * Removes the given layer.
 */
void MapDocument::removeLayer(int index)
{
    if (index < 0 || index >= mMap->layerCount())
        return;

    mUndoStack->push(new RemoveLayer(this, index));
}

void MapDocument::addTileset(Tileset *tileset)
{
    mMap->addTileset(tileset);
    TilesetManager *tilesetManager = TilesetManager::instance();
    tilesetManager->addReference(tileset);
    emit tilesetAdded(tileset);
}

void MapDocument::setTileSelection(const QRegion &selection)
{
    if (mTileSelection != selection) {
        const QRegion oldTileSelection = mTileSelection;
        mTileSelection = selection;
        emit tileSelectionChanged(mTileSelection, oldTileSelection);
    }
}

/**
 * Emits the map changed signal. This signal should be emitted after changing
 * the map size or its tile size.
 */
void MapDocument::emitMapChanged()
{
    emit mapChanged();
}

void MapDocument::emitRegionChanged(const QRegion &region)
{
    emit regionChanged(region);
}

/**
 * Emits the objects added signal with the specified list of objects.
 * This will cause the scene to insert the related items.
 */
void MapDocument::emitObjectsAdded(const QList<MapObject*> &objects)
{
    emit objectsAdded(objects);
}

/**
 * Emits the objects removed signal with the specified list of objects.
 * This will cause the scene to remove the related items.
 */
void MapDocument::emitObjectsRemoved(const QList<MapObject*> &objects)
{
    emit objectsRemoved(objects);
}

/**
 * Emits the objects changed signal with the specified list of objects.
 * This will cause the scene to update the related items.
 */
void MapDocument::emitObjectsChanged(const QList<MapObject*> &objects)
{
    emit objectsChanged(objects);
}

void MapDocument::onLayerAdded(int index)
{
    emit layerAdded(index);

    // Select the first layer that gets added to the map
    if (mMap->layerCount() == 1)
        setCurrentLayer(0);
}

void MapDocument::onLayerRemoved(int index)
{
    // Bring the current layer index to safety
    if (mCurrentLayer == mMap->layerCount())
        setCurrentLayer(mCurrentLayer - 1);

    emit layerRemoved(index);
}
