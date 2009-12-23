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

#include "mapview.h"

#include <QWheelEvent>
#include <QScrollBar>

using namespace Tiled::Internal;

MapView::MapView(QWidget *parent):
    QGraphicsView(parent),
    mScale(1),
    mHandScrolling(false)
{
    setTransformationAnchor(QGraphicsView::AnchorViewCenter);

    QWidget *v = viewport();

    /* Since Qt 4.5, setting this attribute yields significant repaint
     * reduction when the view is being resized. */
    v->setAttribute(Qt::WA_StaticContents);

    /* Since Qt 4.6, mouse tracking is disabled when no graphics item uses
     * hover events. We need to set it since our scene wants the events. */
    v->setMouseTracking(true);
}

void MapView::setScale(qreal scale)
{
    if (scale == mScale)
        return;

    mScale = scale;
    setTransform(QTransform::fromScale(mScale, mScale));
    setRenderHint(QPainter::SmoothPixmapTransform, mScale < qreal(1));
    emit scaleChanged(mScale);
}

static const int zoomFactorCount = 10;
static const qreal zoomFactors[zoomFactorCount] = {
    0.0625,
    0.125,
    0.25,
    0.5,
    0.75,
    1.0,
    1.5,
    2.0,
    3.0,
    4.0
};

bool MapView::canZoomIn() const
{
    return mScale < zoomFactors[zoomFactorCount - 1];
}

bool MapView::canZoomOut() const
{
    return mScale > zoomFactors[0];
}

void MapView::zoomIn()
{
    for (int i = 0; i < zoomFactorCount; ++i) {
        if (zoomFactors[i] > mScale) {
            setScale(zoomFactors[i]);
            break;
        }
    }
}

void MapView::zoomOut()
{
    for (int i = zoomFactorCount - 1; i >= 0; --i) {
        if (zoomFactors[i] < mScale) {
            setScale(zoomFactors[i]);
            break;
        }
    }
}

void MapView::resetZoom()
{
    setScale(1);
}

/**
 * Override to support zooming in and out using the mouse wheel.
 */
void MapView::wheelEvent(QWheelEvent *event)
{
    if (event->modifiers() & Qt::ControlModifier
        && event->orientation() == Qt::Vertical)
    {
        setTransformationAnchor(QGraphicsView::AnchorUnderMouse);
        if (event->delta() > 0)
            zoomIn();
        else
            zoomOut();
        setTransformationAnchor(QGraphicsView::AnchorViewCenter);
        return;
    }

    QGraphicsView::wheelEvent(event);
}

/**
 * Activates hand scrolling when the middle mouse button is pressed.
 */
void MapView::mousePressEvent(QMouseEvent *event)
{
    if (event->button() == Qt::MidButton) {
        viewport()->setCursor(Qt::ClosedHandCursor);
        setInteractive(false);
        mHandScrolling = true;
        return;
    }

    QGraphicsView::mousePressEvent(event);
}

/**
 * Deactivates hand scrolling when the middle mouse button is released.
 */
void MapView::mouseReleaseEvent(QMouseEvent *event)
{
    if (event->button() == Qt::MidButton) {
        viewport()->setCursor(QCursor());
        setInteractive(true);
        mHandScrolling = false;
        return;
    }

    QGraphicsView::mouseReleaseEvent(event);
}

/**
 * Moves the view with the mouse while hand scrolling.
 */
void MapView::mouseMoveEvent(QMouseEvent *event)
{
    if (mHandScrolling) {
        QScrollBar *hBar = horizontalScrollBar();
        QScrollBar *vBar = verticalScrollBar();
        const QPoint d = event->pos() - mLastMousePos;
        hBar->setValue(hBar->value() + (isRightToLeft() ? d.x() : -d.x()));
        vBar->setValue(vBar->value() - d.y());

        mLastMousePos = event->pos();
        return;
    }

    QGraphicsView::mouseMoveEvent(event);
    mLastMousePos = event->pos();
}
