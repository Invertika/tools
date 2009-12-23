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

#ifndef MAPVIEW_H
#define MAPVIEW_H

#include <QGraphicsView>

namespace Tiled {
namespace Internal {

/**
 * The map view shows the map scene. This class sets some MapScene specific
 * properties on the viewport and implements zooming. It also allows the view
 * to be scrolled with the middle mouse button.
 *
 * @see MapScene
 */
class MapView : public QGraphicsView
{
    Q_OBJECT

public:
    /**
     * Constructor.
     */
    MapView(QWidget *parent = 0);

    void setScale(qreal scale);
    qreal scale() const { return mScale; }

    bool canZoomIn() const;
    bool canZoomOut() const;

signals:
    void scaleChanged(qreal scale);

public slots:
    void zoomIn();
    void zoomOut();
    void resetZoom();

protected:
    void wheelEvent(QWheelEvent *event);

    void mousePressEvent(QMouseEvent *event);
    void mouseReleaseEvent(QMouseEvent *event);
    void mouseMoveEvent(QMouseEvent *event);

private:
    qreal mScale;
    QPoint mLastMousePos;
    bool mHandScrolling;
};

} // namespace Internal
} // namespace Tiled

#endif // MAPVIEW_H
