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

#ifndef MAPWRITERINTERFACE_H
#define MAPWRITERINTERFACE_H

class QString;

namespace Tiled {

class Map;

/**
 * An interface to be implemented by map writers. A map writer implements
 * support for saving to a certain map format.
 *
 * At the moment, Tiled only provides a writer for its own .tmx map format
 * through the TmxMapWriter.
 */
class MapWriterInterface
{
public:
    virtual ~MapWriterInterface() {}

    /**
     * Writes the given map to the given file name.
     *
     * @return <code>true</code> on success, <code>false</code> when an error
     *         occurred. The error can be retrieved by errorString().
     */
    virtual bool write(const Map *map, const QString &fileName) = 0;

    /**
     * Returns the name of this map writer.
     */
    virtual QString name() const = 0;

    /**
     * Returns the error to be shown to the user if an error occured while
     * trying to write a map.
     */
    virtual QString errorString() const = 0;
};

} // namespace Tiled

#endif // MAPWRITERINTERFACE_H
