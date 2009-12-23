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

#ifndef TILESELECTIONMODEL_H
#define TILESELECTIONMODEL_H

#include <QObject>
#include <QRegion>

namespace Tiled {
namespace Internal {

class MapDocument;

/**
 * A selection model for tiles. This basically means it is grid based.
 */
class TileSelectionModel : public QObject
{
    Q_OBJECT

public:
    /**
     * Constructs an empty tile selection model.
     */
    TileSelectionModel(MapDocument *mapDocument, QObject *parent = 0);

    const QRegion &selection() const { return mSelection; }

    void setSelection(const QRegion &selection);

signals:
    /**
     * Emitted when the selected region changes. Sends the currently selected
     * region and the previously selected region.
     */
    void selectionChanged(const QRegion &newSelection,
                          const QRegion &oldSelection);

private:
    QRegion mSelection;
    MapDocument *mMapDocument;
};

} // namespace Internal
} // namespace Tiled

#endif // TILESELECTIONMODEL_H
