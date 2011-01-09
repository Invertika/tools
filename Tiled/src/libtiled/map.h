/*
 * map.h
 * Copyright 2008-2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
 * Copyright 2008, Roderic Morris <roderic@ccs.neu.edu>
 * Copyright 2010, Andrew G. Crowell <overkill9999@gmail.com>
 *
 * This file is part of libtiled.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *    1. Redistributions of source code must retain the above copyright notice,
 *       this list of conditions and the following disclaimer.
 *
 *    2. Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#ifndef MAP_H
#define MAP_H

#include "object.h"

#include <QList>
#include <QSize>

namespace Tiled {

class Layer;
class Tile;
class Tileset;
class ObjectGroup;

/**
 * A tile map. Consists of a stack of layers, each can be either a TileLayer
 * or an ObjectGroup.
 *
 * It also keeps track of the list of referenced tilesets.
 */
class TILEDSHARED_EXPORT Map : public Object
{
public:
    /**
     * The orientation of the map determines how it should be rendered. An
     * Orthogonal map is using rectangular tiles that are aligned on a
     * straight grid. An Isometric map uses diamond shaped tiles that are
     * aligned on an isometric projected grid. A Hexagonal map uses hexagon
     * shaped tiles that fit into each other by shifting every other row.
     *
     * Only Orthogonal and Isometric maps are supported by this version of
     * Tiled.
     */
    enum Orientation {
        Unknown,
        Orthogonal,
        Isometric,
        Hexagonal
    };

    /**
     * Constructor, taking map orientation, size and tile size as parameters.
     */
    Map(Orientation orientation,
        int width, int height,
        int tileWidth, int tileHeight);

    /**
     * Destructor.
     */
    ~Map();

    /**
     * Returns the orientation of the map.
     */
    Orientation orientation() const { return mOrientation; }

    /**
     * Sets the orientation of the map.
     */
    void setOrientation(Orientation orientation)
    { mOrientation = orientation; }

    /**
     * Returns the width of this map.
     */
    int width() const { return mWidth; }

    /**
     * Sets the width of this map.
     */
    void setWidth(int width) { mWidth = width; }

    /**
     * Returns the height of this map.
     */
    int height() const { return mHeight; }

    /**
     * Sets the height of this map.
     */
    void setHeight(int height) { mHeight = height; }

    /**
     * Returns the size of this map. Provided for convenience.
     */
    QSize size() const { return QSize(mWidth, mHeight); }

    /**
     * Returns the tile width of this map.
     */
    int tileWidth() const { return mTileWidth; }

    /**
     * Returns the tile height used by this map.
     */
    int tileHeight() const { return mTileHeight; }

    /**
     * Returns the maximum tile size used by tile layers of this map.
     * @see TileLayer::extraTileSize()
     */
    QSize maxTileSize() const { return mMaxTileSize; }

    /**
     * Adjusts the maximum tile size to be at least as much as the given
     * size. Called from tile layers when their maximum tile size increases.
     */
    void adjustMaxTileSize(const QSize &size);

    /**
     * Convenience method for getting the extra tile size, which is the number
     * of pixels that tiles may extend beyond the size of the tile grid.
     *
     * @see maxTileSize()
     */
    QSize extraTileSize() const
    { return QSize(mMaxTileSize.width() - mTileWidth,
                   mMaxTileSize.height() - mTileHeight); }

    /**
     * Returns the number of layers of this map.
     */
    int layerCount() const
    { return mLayers.size(); }

    /**
     * Convenience function that returns the number of layers of this map that
     * are tile layers.
     */
    int tileLayerCount() const;

    /**
     * Convenience function that returns the number of layers of this map that
     * are object groups.
     */
    int objectGroupCount() const;

    /**
     * Returns the layer at the specified index.
     */
    Layer *layerAt(int index) const
    { return mLayers.at(index); }

    /**
     * Returns the list of layers of this map. This is useful when you want to
     * use foreach.
     */
    const QList<Layer*> &layers() const { return mLayers; }

    /**
     * Adds a layer to this map.
     */
    void addLayer(Layer *layer);

    /**
     * Returns the index of the layer given by \a layerName, or -1 if no
     * layer with that name is found.
     */
    int indexOfLayer(const QString &layerName) const;

    /**
     * Adds a layer to this map, inserting it at the given index.
     */
    void insertLayer(int index, Layer *layer);

    /**
     * Removes the layer at the given index from this map and returns it.
     * The caller becomes responsible for the lifetime of this layer.
     */
    Layer *takeLayerAt(int index);

    /**
     * Adds a tileset to this map. The map does not take ownership over its
     * tilesets, this is merely for keeping track of which tilesets are used by
     * the map, and their saving order.
     *
     * @param tileset the tileset to add
     */
    void addTileset(Tileset *tileset);

    /**
     * Inserts \a tileset at \a index in the list of tilesets used by this map.
     */
    void insertTileset(int index, Tileset *tileset);

    /**
     * Returns the index of the given \a tileset, or -1 if it is not used in
     * this map.
     */
    int indexOfTileset(Tileset *tileset) const;

    /**
     * Removes the tileset at \a index from this map.
     *
     * \warning Does not make sure that this map no longer refers to tiles from
     *          the removed tileset!
     *
     * \sa addTileset
     */
    void removeTilesetAt(int index);

    /**
     * Replaces all tiles from \a oldTileset with tiles from \a newTileset.
     * Also replaces the old tileset with the new tileset in the list of
     * tilesets.
     */
    void replaceTileset(Tileset *oldTileset, Tileset *newTileset);

    /**
     * Returns the tilesets that the tiles on this map are using.
     */
    const QList<Tileset*> &tilesets() const { return mTilesets; }

    /**
     * Returns whether the given \a tileset is used by any tile layer of this
     * map.
     */
    bool isTilesetUsed(Tileset *tileset) const;

    Map *clone() const;

private:
    void adoptLayer(Layer *layer);

    Orientation mOrientation;
    int mWidth;
    int mHeight;
    int mTileWidth;
    int mTileHeight;
    QSize mMaxTileSize;
    QList<Layer*> mLayers;
    QList<Tileset*> mTilesets;
};

} // namespace Tiled

#endif // MAP_H
