/*
 * mapscene.h
 * Copyright 2008-2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
 * Copyright 2008, Roderic Morris <roderic@ccs.neu.edu>
 * Copyright 2009, Edward Hutchins <eah1@yahoo.com>
 * Copyright 2010, Jeff Bland <jksb@member.fsf.org>
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

#ifndef MAPSCENE_H
#define MAPSCENE_H

#include <QGraphicsScene>
#include <QMap>
#include <QSet>

namespace Tiled {

class Layer;
class MapObject;
class Tileset;

namespace Internal {

class AbstractTool;
class MapDocument;
class MapObjectItem;
class MapScene;
class ObjectGroupItem;

/**
 * A graphics scene that represents the contents of a map.
 */
class MapScene : public QGraphicsScene
{
    Q_OBJECT

public:
    /**
     * Constructor.
     */
    MapScene(QObject *parent);

    /**
     * Destructor.
     */
    ~MapScene();

    /**
     * Returns the map document this scene is displaying.
     */
    MapDocument *mapDocument() const { return mMapDocument; }

    /**
     * Sets the map this scene displays.
     */
    void setMapDocument(MapDocument *map);

    /**
     * Returns whether the tile grid is visible.
     */
    bool isGridVisible() const { return mGridVisible; }

    /**
     * Returns the selected object group item, or 0 if no object group is
     * selected.
     */
    ObjectGroupItem *selectedObjectGroupItem() const
    { return mSelectedObjectGroupItem; }

    /**
     * Returns the set of selected map object items.
     */
    const QSet<MapObjectItem*> &selectedObjectItems() const
    { return mSelectedObjectItems; }

    /**
     * Sets the set of selected map object items.
     */
    void setSelectedObjectItems(const QSet<MapObjectItem*> &items);

    /**
     * Enables the selected tool at this map scene.
     * Therefore it tells that tool, that this is the active map scene.
     */
    void enableSelectedTool();
    void disableSelectedTool();

    /**
     * Sets the currently selected tool.
     */
    void setSelectedTool(AbstractTool *tool);

public slots:
    /**
     * Sets whether the tile grid is visible.
     */
    void setGridVisible(bool visible);

protected:
    /**
     * QGraphicsScene::drawForeground override that draws the tile grid.
     */
    void drawForeground(QPainter *painter, const QRectF &rect);

    /**
     * Override for handling enter and leave events.
     */
    bool event(QEvent *event);

    void mouseMoveEvent(QGraphicsSceneMouseEvent *mouseEvent);
    void mousePressEvent(QGraphicsSceneMouseEvent *mouseEvent);
    void mouseReleaseEvent(QGraphicsSceneMouseEvent *mouseEvent);

    void dragEnterEvent(QGraphicsSceneDragDropEvent *event);

private slots:
    /**
     * Refreshes the map scene.
     */
    void refreshScene();

    /**
     * Repaints the specified region. The region is in tile coordinates.
     */
    void repaintRegion(const QRegion &region);

    void currentLayerChanged();

    void mapChanged();
    void tilesetChanged(Tileset *tileset);

    void layerAdded(int index);
    void layerRemoved(int index);
    void layerChanged(int index);

    void objectsAdded(const QList<MapObject*> &objects);
    void objectsRemoved(const QList<MapObject*> &objects);
    void objectsChanged(const QList<MapObject*> &objects);

private:
    QGraphicsItem *createLayerItem(Layer *layer);

    void updateInteractionMode();

    bool eventFilter(QObject *object, QEvent *event);

    MapDocument *mMapDocument;
    ObjectGroupItem *mSelectedObjectGroupItem;
    AbstractTool *mSelectedTool;
    AbstractTool *mActiveTool;
    bool mGridVisible;
    bool mUnderMouse;
    Qt::KeyboardModifiers mCurrentModifiers;
    QPointF mLastMousePos;
    QVector<QGraphicsItem*> mLayerItems;

    typedef QMap<MapObject*, MapObjectItem*> ObjectItems;
    ObjectItems mObjectItems;
    QSet<MapObjectItem*> mSelectedObjectItems;
};

} // namespace Internal
} // namespace Tiled

#endif // MAPSCENE_H
