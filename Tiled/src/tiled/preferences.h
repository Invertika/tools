/*
 * preferences.h
 * Copyright 2009-2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
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

#ifndef PREFERENCES_H
#define PREFERENCES_H

#include <QObject>

#include "mapwriter.h"

class QSettings;

namespace Tiled {
namespace Internal {

/**
 * This class holds user preferences and provides a convenient interface to
 * access them.
 */
class Preferences : public QObject
{
    Q_OBJECT

public:
    static Preferences *instance();
    static void deleteInstance();

    bool showGrid() const { return mShowGrid; }
    bool snapToGrid() const { return mSnapToGrid; }

    MapWriter::LayerDataFormat layerDataFormat() const;
    void setLayerDataFormat(MapWriter::LayerDataFormat layerDataFormat);

    bool dtdEnabled() const;
    void setDtdEnabled(bool enabled);

    QString language() const;
    void setLanguage(const QString &language);

    bool reloadTilesetsOnChange() const;
    void setReloadTilesetsOnChanged(bool value);

    bool useOpenGL() const { return mUseOpenGL; }
    void setUseOpenGL(bool useOpenGL);

    /**
     * Provides access to the QSettings instance to allow storing/retrieving
     * arbitrary values. The naming style for groups and keys is CamelCase.
     */
    QSettings *settings() const { return mSettings; }

public slots:
    void setShowGrid(bool showGrid);
    void setSnapToGrid(bool snapToGrid);

signals:
    void showGridChanged(bool showGrid);
    void snapToGridChanged(bool snapToGrid);

    void useOpenGLChanged(bool useOpenGL);

private:
    Preferences();
    ~Preferences();

    QSettings *mSettings;

    bool mShowGrid;
    bool mSnapToGrid;

    MapWriter::LayerDataFormat mLayerDataFormat;
    bool mDtdEnabled;
    QString mLanguage;
    bool mReloadTilesetsOnChange;
    bool mUseOpenGL;

    static Preferences *mInstance;
};

} // namespace Internal
} // namespace Tiled

#endif // PREFERENCES_H
