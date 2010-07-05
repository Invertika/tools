/*
 * resizelayer.h
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

#ifndef RESIZELAYER_H
#define RESIZELAYER_H

#include <QPoint>
#include <QSize>
#include <QUndoCommand>

namespace Tiled {

class Layer;

namespace Internal {

class MapDocument;

/**
 * Undo command that resizes a map layer.
 */
class ResizeLayer : public QUndoCommand
{
public:
    /**
     * Creates an undo command that resizes the layer at \a index to \a size,
     * shifting the tiles by \a offset.
     */
    ResizeLayer(MapDocument *mapDocument,
                int index,
                const QSize &size,
                const QPoint &offset);

    ~ResizeLayer();

    void undo();
    void redo();

private:
    Layer *swapLayer(Layer *layer);

    MapDocument *mMapDocument;
    int mIndex;
    Layer *mOriginalLayer;
    Layer *mResizedLayer;
};

} // namespace Internal
} // namespace Tiled

#endif // RESIZELAYER_H
