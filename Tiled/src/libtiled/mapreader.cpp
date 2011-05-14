/*
 * mapreader.cpp
 * Copyright 2008-2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
 * Copyright 2010, Jeff Bland <jksb@member.fsf.org>
 * Copyright 2010, Dennis Honeyman <arcticuno@gmail.com>
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

#include "mapreader.h"

#include "compression.h"
#include "objectgroup.h"
#include "map.h"
#include "mapobject.h"
#include "tile.h"
#include "tilelayer.h"
#include "tileset.h"

#include <QCoreApplication>
#include <QDebug>
#include <QDir>
#include <QFileInfo>
#include <QMap>
#include <QXmlStreamReader>

using namespace Tiled;
using namespace Tiled::Internal;

namespace Tiled {
namespace Internal {

class MapReaderPrivate
{
    Q_DECLARE_TR_FUNCTIONS(MapReader)

public:
    MapReaderPrivate(MapReader *mapReader):
        p(mapReader),
        mMap(0),
        mReadingExternalTileset(false)
    {}

    Map *readMap(QIODevice *device, const QString &path);
    Tileset *readTileset(QIODevice *device, const QString &path);

    bool openFile(QFile *file);

    QString errorString() const;

private:
    void readUnknownElement();

    Map *readMap();

    Tileset *readTileset();
    void readTilesetTile(Tileset *tileset);
    void readTilesetImage(Tileset *tileset);

    TileLayer *readLayer();
    void readLayerData(TileLayer *tileLayer);
    void decodeBinaryLayerData(TileLayer *tileLayer,
                               const QStringRef &text,
                               const QStringRef &compression);
    void decodeCSVLayerData(TileLayer *tileLayer, const QString &text);

    /**
     * Returns the tile for the given global tile ID. When an error occurs,
     * \a ok is set to false and an error is raised.
     *
     * @param gid the global tile ID, must be at least 0
     * @param ok  returns whether the conversion went ok
     * @return the tile associated with the given global tile ID, or 0 if
     *         not found
     */
    Tile *tileForGid(int gid, bool &ok);

    ObjectGroup *readObjectGroup();
    MapObject *readObject();

    Properties readProperties();
    void readProperty(Properties *properties);

    MapReader *p;

    QString mError;
    QString mPath;
    Map *mMap;
    QMap<int, Tileset*> mGidsToTileset;
    bool mReadingExternalTileset;

    QXmlStreamReader xml;
};

} // namespace Internal
} // namespace Tiled

Map *MapReaderPrivate::readMap(QIODevice *device, const QString &path)
{
    mError.clear();
    mPath = path;
    Map *map = 0;

    xml.setDevice(device);

    if (xml.readNextStartElement() && xml.name() == "map") {
        map = readMap();
    } else {
        xml.raiseError(tr("Not a map file."));
    }

    mGidsToTileset.clear();
    return map;
}

Tileset *MapReaderPrivate::readTileset(QIODevice *device, const QString &path)
{
    mError.clear();
    mPath = path;
    Tileset *tileset = 0;
    mReadingExternalTileset = true;

    xml.setDevice(device);

    if (xml.readNextStartElement() && xml.name() == "tileset")
        tileset = readTileset();
    else
        xml.raiseError(tr("Not a tileset file."));

    mReadingExternalTileset = false;
    return tileset;
}

QString MapReaderPrivate::errorString() const
{
    if (!mError.isEmpty()) {
        return mError;
    } else {
        return tr("%3\n\nLine %1, column %2")
                .arg(xml.lineNumber())
                .arg(xml.columnNumber())
                .arg(xml.errorString());
    }
}

bool MapReaderPrivate::openFile(QFile *file)
{
    if (!file->exists()) {
        mError = tr("File not found: %1").arg(file->fileName());
        return false;
    } else if (!file->open(QFile::ReadOnly | QFile::Text)) {
        mError = tr("Unable to read file: %1").arg(file->fileName());
        return false;
    }

    return true;
}

void MapReaderPrivate::readUnknownElement()
{
    qDebug() << "Unknown element (fixme):" << xml.name();
    xml.skipCurrentElement();
}

static Map::Orientation orientationFromString(const QStringRef &string)
{
    Map::Orientation orientation = Map::Unknown;
    if (string == QLatin1String("orthogonal")) {
        orientation = Map::Orthogonal;
    } else if (string == QLatin1String("isometric")) {
        orientation = Map::Isometric;
    }
    return orientation;
}

Map *MapReaderPrivate::readMap()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "map");

    const QXmlStreamAttributes atts = xml.attributes();
    const int mapWidth =
            atts.value(QLatin1String("width")).toString().toInt();
    const int mapHeight =
            atts.value(QLatin1String("height")).toString().toInt();
    const int tileWidth =
            atts.value(QLatin1String("tilewidth")).toString().toInt();
    const int tileHeight =
            atts.value(QLatin1String("tileheight")).toString().toInt();

    const QStringRef orientationRef =
            atts.value(QLatin1String("orientation"));
    const Map::Orientation orientation =
            orientationFromString(orientationRef);

    if (orientation == Map::Unknown) {
        xml.raiseError(tr("Unsupported map orientation: \"%1\"")
                       .arg(orientationRef.toString()));
    }

    mMap = new Map(orientation, mapWidth, mapHeight, tileWidth, tileHeight);

    while (xml.readNextStartElement()) {
        if (xml.name() == "properties")
            mMap->mergeProperties(readProperties());
        else if (xml.name() == "tileset")
            mMap->addTileset(readTileset());
        else if (xml.name() == "layer")
            mMap->addLayer(readLayer());
        else if (xml.name() == "objectgroup")
            mMap->addLayer(readObjectGroup());
        else
            readUnknownElement();
    }

    // Clean up in case of error
    if (xml.hasError()) {
        // The tilesets are not owned by the map
        qDeleteAll(mMap->tilesets());

        delete mMap;
        mMap = 0;
    }

    return mMap;
}

Tileset *MapReaderPrivate::readTileset()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "tileset");

    const QXmlStreamAttributes atts = xml.attributes();
    const QString source = atts.value(QLatin1String("source")).toString();
    const int firstGid =
            atts.value(QLatin1String("firstgid")).toString().toInt();

    Tileset *tileset = 0;

    if (source.isEmpty()) { // Not an external tileset
        const QString name =
                atts.value(QLatin1String("name")).toString();
        const int tileWidth =
                atts.value(QLatin1String("tilewidth")).toString().toInt();
        const int tileHeight =
                atts.value(QLatin1String("tileheight")).toString().toInt();
        const int tileSpacing =
                atts.value(QLatin1String("spacing")).toString().toInt();
        const int margin =
                atts.value(QLatin1String("margin")).toString().toInt();

        if (tileWidth <= 0 || tileHeight <= 0
            || (firstGid <= 0 && !mReadingExternalTileset)) {
            xml.raiseError(tr("Invalid tileset parameters for tileset"
                              " '%1'").arg(name));
        } else {
            tileset = new Tileset(name, tileWidth, tileHeight,
                                  tileSpacing, margin);

            while (xml.readNextStartElement()) {
                if (xml.name() == "tile")
                    readTilesetTile(tileset);
                else if (xml.name() == "image")
                    readTilesetImage(tileset);
                else
                    readUnknownElement();
            }
        }
    } else { // External tileset
        const QString absoluteSource = p->resolveReference(source, mPath);
        QString error;
        tileset = p->readExternalTileset(absoluteSource, &error);

        if (!tileset) {
            xml.raiseError(tr("Error while loading tileset '%1': %2")
                           .arg(absoluteSource, error));
        }

        xml.skipCurrentElement();
    }

    if (tileset && !mReadingExternalTileset)
        mGidsToTileset.insert(firstGid, tileset);

    return tileset;
}

void MapReaderPrivate::readTilesetTile(Tileset *tileset)
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "tile");

    const QXmlStreamAttributes atts = xml.attributes();
    const int id = atts.value(QLatin1String("id")).toString().toInt();

    if (id < 0 || id >= tileset->tileCount()) {
        xml.raiseError(tr("Invalid tile ID: %1").arg(id));
        return;
    }

    // TODO: Add support for individual tiles (then it needs to be added here)

    while (xml.readNextStartElement()) {
        if (xml.name() == "properties") {
            Tile *tile = tileset->tileAt(id);
            tile->mergeProperties(readProperties());
        } else {
            readUnknownElement();
        }
    }
}

void MapReaderPrivate::readTilesetImage(Tileset *tileset)
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "image");

    const QXmlStreamAttributes atts = xml.attributes();
    QString source = atts.value(QLatin1String("source")).toString();
    QString trans = atts.value(QLatin1String("trans")).toString();

    if (!trans.isEmpty()) {
        if (!trans.startsWith(QLatin1Char('#')))
            trans.prepend(QLatin1Char('#'));
        tileset->setTransparentColor(QColor(trans));
    }

    source = p->resolveReference(source, mPath);

    const QImage tilesetImage = p->readExternalImage(source);
    if (!tileset->loadFromImage(tilesetImage, source))
        xml.raiseError(tr("Error loading tileset image:\n'%1'").arg(source));

    xml.skipCurrentElement();
}

static void readLayerAttributes(Layer *layer,
                                const QXmlStreamAttributes &atts)
{
    const QStringRef opacityRef = atts.value(QLatin1String("opacity"));
    const QStringRef visibleRef = atts.value(QLatin1String("visible"));

    bool ok;
    const float opacity = opacityRef.toString().toFloat(&ok);
    if (ok)
        layer->setOpacity(opacity);

    const int visible = visibleRef.toString().toInt(&ok);
    if (ok)
        layer->setVisible(visible);
}

TileLayer *MapReaderPrivate::readLayer()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "layer");

    const QXmlStreamAttributes atts = xml.attributes();
    const QString name = atts.value(QLatin1String("name")).toString();
    const int x = atts.value(QLatin1String("x")).toString().toInt();
    const int y = atts.value(QLatin1String("y")).toString().toInt();
    const int width = atts.value(QLatin1String("width")).toString().toInt();
    const int height = atts.value(QLatin1String("height")).toString().toInt();

    TileLayer *tileLayer = new TileLayer(name, x, y, width, height);
    readLayerAttributes(tileLayer, atts);

    while (xml.readNextStartElement()) {
        if (xml.name() == "properties")
            tileLayer->mergeProperties(readProperties());
        else if (xml.name() == "data")
            readLayerData(tileLayer);
        else
            readUnknownElement();
    }

    return tileLayer;
}

void MapReaderPrivate::readLayerData(TileLayer *tileLayer)
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "data");

    const QXmlStreamAttributes atts = xml.attributes();
    QStringRef encoding = atts.value(QLatin1String("encoding"));
    QStringRef compression = atts.value(QLatin1String("compression"));

    int x = 0;
    int y = 0;

    while (xml.readNext() != QXmlStreamReader::Invalid) {
        if (xml.isEndElement())
            break;
        else if (xml.isStartElement()) {
            if (xml.name() == QLatin1String("tile")) {
                if (y >= tileLayer->height()) {
                    xml.raiseError(tr("Too many <tile> elements"));
                    continue;
                }

                const QXmlStreamAttributes atts = xml.attributes();
                int gid = atts.value(QLatin1String("gid")).toString().toInt();
                bool ok;
                Tile *tile = tileForGid(gid, ok);
                if (ok)
                    tileLayer->setTile(x, y, tile);
                else
                    xml.raiseError(tr("Invalid tile: %1").arg(gid));

                x++;
                if (x >= tileLayer->width()) {
                    x = 0;
                    y++;
                }

                xml.skipCurrentElement();
            } else {
                readUnknownElement();
            }
        } else if (xml.isCharacters() && !xml.isWhitespace()) {
            if (encoding == QLatin1String("base64")) {
                decodeBinaryLayerData(tileLayer,
                                      xml.text(),
                                      compression);
            } else if (encoding == QLatin1String("csv")) {
                decodeCSVLayerData(tileLayer, xml.text().toString());
            } else {
                xml.raiseError(tr("Unknown encoding: %1")
                               .arg(encoding.toString()));
                continue;
            }
        }
    }
}

void MapReaderPrivate::decodeBinaryLayerData(TileLayer *tileLayer,
                                             const QStringRef &text,
                                             const QStringRef &compression)
{
#if QT_VERSION < 0x040800
    const QString textData = QString::fromRawData(text.unicode(), text.size());
    const QByteArray latin1Text = textData.toLatin1();
#else
    const QByteArray latin1Text = text.toLatin1();
#endif
    QByteArray tileData = QByteArray::fromBase64(latin1Text);
    const int size = (tileLayer->width() * tileLayer->height()) * 4;

    if (compression == QLatin1String("zlib")
        || compression == QLatin1String("gzip")) {
        tileData = decompress(tileData, size);
    } else if (!compression.isEmpty()) {
        xml.raiseError(tr("Compression method '%1' not supported")
                       .arg(compression.toString()));
        return;
    }

    if (size != tileData.length()) {
        xml.raiseError(tr("Corrupt layer data for layer '%1'")
                       .arg(tileLayer->name()));
        return;
    }

    const unsigned char *data =
            reinterpret_cast<const unsigned char*>(tileData.constData());
    int x = 0;
    int y = 0;

    for (int i = 0; i < size - 3; i += 4) {
        const int gid = data[i] |
                        data[i + 1] << 8 |
                        data[i + 2] << 16 |
                        data[i + 3] << 24;

        bool ok;
        Tile *tile = tileForGid(gid, ok);
        if (ok)
            tileLayer->setTile(x, y, tile);
        else {
            xml.raiseError(tr("Invalid tile: %1").arg(gid));
            return;
        }

        x++;
        if (x == tileLayer->width()) {
            x = 0;
            y++;
        }
    }
}

void MapReaderPrivate::decodeCSVLayerData(TileLayer *tileLayer, const QString &text)
{
    QString trimText = text.trimmed();
    QStringList tiles = trimText.split(QLatin1Char(','));

    if (tiles.length() != tileLayer->width() * tileLayer->height()) {
        xml.raiseError(tr("Corrupt layer data for layer '%1'")
                       .arg(tileLayer->name()));
        return;
    }

    for (int y = 0; y < tileLayer->height(); y++) {
        for (int x = 0; x < tileLayer->width(); x++) {
            bool conversionOk;
            const int gid = tiles.at(y * tileLayer->width() + x)
                            .toInt(&conversionOk);
            if (!conversionOk) {
                xml.raiseError(
                        tr("Unable to parse tile at (%1,%2) on layer '%3'")
                               .arg(x + 1).arg(y + 1).arg(tileLayer->name()));
                return;
            }
            bool gidOk;
            Tile *tile = tileForGid(gid, gidOk);
            if (gidOk)
                tileLayer->setTile(x, y, tile);
            else {
                xml.raiseError(tr("Invalid tile: %1").arg(gid));
            }
        }
    }
}

Tile *MapReaderPrivate::tileForGid(int gid, bool &ok)
{
    Tile *result = 0;

    if (gid < 0) {
        xml.raiseError(tr("Invalid global tile id (less than 0): %1")
                       .arg(gid));
        ok = false;
    } else if (gid == 0) {
        ok = true;
    } else if (mGidsToTileset.isEmpty()) {
        xml.raiseError(tr("Tile used but no tilesets specified"));
        ok = false;
    } else {
        // Find the tileset containing this tile
        QMap<int, Tileset*>::const_iterator i = mGidsToTileset.upperBound(gid);
        --i; // Navigate one tileset back since upper bound finds the next
        const int tileId = gid - i.key();
        const Tileset *tileset = i.value();

        result = tileset ? tileset->tileAt(tileId) : 0;
        ok = true;
    }

    return result;
}

ObjectGroup *MapReaderPrivate::readObjectGroup()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "objectgroup");

    const QXmlStreamAttributes atts = xml.attributes();
    const QString name = atts.value(QLatin1String("name")).toString();
    const int x = atts.value(QLatin1String("x")).toString().toInt();
    const int y = atts.value(QLatin1String("y")).toString().toInt();
    const int width = atts.value(QLatin1String("width")).toString().toInt();
    const int height = atts.value(QLatin1String("height")).toString().toInt();

    ObjectGroup *objectGroup = new ObjectGroup(name, x, y, width, height);
    readLayerAttributes(objectGroup, atts);

    const QString color = atts.value(QLatin1String("color")).toString();
    if (!color.isEmpty())
        objectGroup->setColor(color);

    while (xml.readNextStartElement()) {
        if (xml.name() == "object")
            objectGroup->addObject(readObject());
        else if (xml.name() == "properties")
            objectGroup->mergeProperties(readProperties());
        else
            readUnknownElement();
    }

    return objectGroup;
}

MapObject *MapReaderPrivate::readObject()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "object");

    const QXmlStreamAttributes atts = xml.attributes();
    const QString name = atts.value(QLatin1String("name")).toString();
    const int gid = atts.value(QLatin1String("gid")).toString().toInt();
    const int x = atts.value(QLatin1String("x")).toString().toInt();
    const int y = atts.value(QLatin1String("y")).toString().toInt();
    const int width = atts.value(QLatin1String("width")).toString().toInt();
    const int height = atts.value(QLatin1String("height")).toString().toInt();
    const QString type = atts.value(QLatin1String("type")).toString();

    // Convert pixel coordinates to tile coordinates
    const int tileHeight = mMap->tileHeight();
    const int tileWidth = mMap->tileWidth();
    qreal xF, yF, widthF, heightF;

    if (mMap->orientation() == Map::Isometric) {
        // Isometric needs special handling, since the pixel values are based
        // solely on the tile height.
        xF = (qreal) x / tileHeight;
        yF = (qreal) y / tileHeight;
        widthF = (qreal) width / tileHeight;
        heightF = (qreal) height / tileHeight;
    } else {
        xF = (qreal) x / tileWidth;
        yF = (qreal) y / tileHeight;
        widthF = (qreal) width / tileWidth;
        heightF = (qreal) height / tileHeight;
    }

    MapObject *object = new MapObject(name, type, xF, yF, widthF, heightF);

    if (gid) {
        bool ok;
        Tile *tile = tileForGid(gid, ok);
        if (ok) {
            object->setTile(tile);
        } else {
            xml.raiseError(tr("Invalid tile: %1").arg(gid));
        }
    }

    while (xml.readNextStartElement()) {
        if (xml.name() == "properties")
            object->mergeProperties(readProperties());
        else
            readUnknownElement();
    }

    return object;
}

Properties MapReaderPrivate::readProperties()
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "properties");

    Properties properties;

    while (xml.readNextStartElement()) {
        if (xml.name() == "property")
            readProperty(&properties);
        else
            readUnknownElement();
    }

    return properties;
}

void MapReaderPrivate::readProperty(Properties *properties)
{
    Q_ASSERT(xml.isStartElement() && xml.name() == "property");

    const QXmlStreamAttributes atts = xml.attributes();
    QString propertyName = atts.value(QLatin1String("name")).toString();
    QString propertyValue = atts.value(QLatin1String("value")).toString();

    while (xml.readNext() != QXmlStreamReader::Invalid) {
        if (xml.isEndElement()) {
            break;
        } else if (xml.isCharacters() && !xml.isWhitespace()) {
            if (propertyValue.isEmpty())
                propertyValue = xml.text().toString();
        } else if (xml.isStartElement()) {
            readUnknownElement();
        }
    }

    properties->insert(propertyName, propertyValue);
}


MapReader::MapReader()
    : d(new MapReaderPrivate(this))
{
}

MapReader::~MapReader()
{
    delete d;
}

Map *MapReader::readMap(QIODevice *device, const QString &path)
{
    return d->readMap(device, path);
}

Map *MapReader::readMap(const QString &fileName)
{
    QFile file(fileName);
    if (!d->openFile(&file))
        return 0;

    return readMap(&file, QFileInfo(fileName).absolutePath());
}

Tileset *MapReader::readTileset(QIODevice *device, const QString &path)
{
    return d->readTileset(device, path);
}

Tileset *MapReader::readTileset(const QString &fileName)
{
    QFile file(fileName);
    if (!d->openFile(&file))
        return 0;

    Tileset *tileset = readTileset(&file, QFileInfo(fileName).absolutePath());
    if (tileset)
        tileset->setFileName(fileName);

    return tileset;
}

QString MapReader::errorString() const
{
    return d->errorString();
}

QString MapReader::resolveReference(const QString &reference,
                                    const QString &mapPath)
{
    if (QDir::isRelativePath(reference))
        return mapPath + QLatin1Char('/') + reference;
    else
        return reference;
}

QImage MapReader::readExternalImage(const QString &source)
{
    return QImage(source);
}

Tileset *MapReader::readExternalTileset(const QString &source,
                                        QString *error)
{
    MapReader reader;
    Tileset *tileset = reader.readTileset(source);
    if (!tileset)
        *error = reader.errorString();
    return tileset;
}
