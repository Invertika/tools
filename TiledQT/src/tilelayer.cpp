/*
 * Tiled Map Editor (Qt)
 * Copyright 2008-2009 Tiled (Qt) developers (see AUTHORS file)
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

#include "tilelayer.h"

#include "map.h"
#include "tile.h"

using namespace Tiled;

TileLayer::TileLayer(const QString &name, int x, int y, int width, int height):
    Layer(name, x, y, width, height),
    mMaxTileHeight(0),
    mTiles(width * height)
{
}

void TileLayer::setTile(int x, int y, Tile *tile)
{
    if (tile && tile->height() > mMaxTileHeight) {
        mMaxTileHeight = tile->height();
        if (mMap)
            mMap->adjustMaxTileHeight(mMaxTileHeight);
    }

    mTiles[x + y * mWidth] = tile;
}

TileLayer *TileLayer::copy(const QRegion &region) const
{
    const QRegion area = region.intersected(QRect(0, 0, width(), height()));
    const QRect bounds = region.boundingRect();
    const QRect areaBounds = area.boundingRect();
    const int offsetX = qMax(0, areaBounds.x() - bounds.x());
    const int offsetY = qMax(0, areaBounds.y() - bounds.y());

    TileLayer *copied = new TileLayer(QString(),
                                      0, 0,
                                      bounds.width(), bounds.height());

    foreach (const QRect &rect, area.rects())
        for (int x = rect.left(); x <= rect.right(); ++x)
            for (int y = rect.top(); y <= rect.bottom(); ++y)
                copied->setTile(x - areaBounds.x() + offsetX,
                                y - areaBounds.y() + offsetY,
                                tileAt(x, y));

    return copied;
}

void TileLayer::merge(const QPoint &pos, const TileLayer *layer)
{
    // Determine the overlapping area
    QRect area = QRect(pos, QSize(layer->width(), layer->height()));
    area &= QRect(0, 0, width(), height());

    for (int y = area.top(); y <= area.bottom(); ++y)
        for (int x = area.left(); x <= area.right(); ++x)
            if (Tile *tile = layer->tileAt(x - area.left(), y - area.top()))
                setTile(x, y, tile);
}

void TileLayer::resize(const QSize &size, const QPoint &offset)
{
    QVector<Tile*> newTiles(size.width() * size.height());

    // Copy over the preserved part
    const int startX = qMax(0, -offset.x());
    const int startY = qMax(0, -offset.y());
    const int endX = qMin(mWidth, size.width() - offset.x());
    const int endY = qMin(mHeight, size.height() - offset.y());

    for (int y = startY; y < endY; ++y) {
        for (int x = startX; x < endX; ++x) {
            const int index = x + offset.x() + (y + offset.y()) * size.width();
            newTiles[index] = tileAt(x, y);
        }
    }

    mTiles = newTiles;
    Layer::resize(size, offset);
}

void TileLayer::offset(const QPoint &offset,
                       const QRect &bounds,
                       bool wrapX, bool wrapY)
{
    QVector<Tile*> newTiles(mWidth * mHeight);

    for (int y = 0; y < mHeight; ++y) {
        for (int x = 0; x < mWidth; ++x) {
            // Skip out of bounds tiles
            if (!bounds.contains(x, y)) {
                newTiles[x + y * mWidth] = tileAt(x, y);
                continue;
            }

            // Get position to pull tile value from
            int oldX = x - offset.x();
            int oldY = y - offset.y();

            // Wrap x value that will be pulled from
            if (wrapX && bounds.width() > 0) {
                while (oldX < bounds.left())
                    oldX += bounds.width();
                while (oldX > bounds.right())
                    oldX -= bounds.width();
            }

            // Wrap y value that will be pulled from
            if (wrapY && bounds.height() > 0) {
                while (oldY < bounds.top())
                    oldY += bounds.height();
                while (oldY > bounds.bottom())
                    oldY -= bounds.height();
            }

            // Set the new tile
            if (contains(oldX, oldY) && bounds.contains(oldX, oldY))
                newTiles[x + y * mWidth] = tileAt(oldX, oldY);
            else
                newTiles[x + y * mWidth] = 0;
        }
    }

    mTiles = newTiles;
}

bool TileLayer::isEmpty() const
{
    for (int y = 0; y < mHeight; ++y)
        for (int x = 0; x < mWidth; ++x)
            if (tileAt(x, y))
                return false;

    return true;
}

/**
 * Returns a duplicate of this TileLayer.
 *
 * \sa Layer::clone()
 */
Layer *TileLayer::clone() const
{
    return initializeClone(new TileLayer(mName, mX, mY, mWidth, mHeight));
}

TileLayer *TileLayer::initializeClone(TileLayer *clone) const
{
    Layer::initializeClone(clone);
    clone->mTiles = mTiles;
    clone->mMaxTileHeight = mMaxTileHeight;
    return clone;
}
