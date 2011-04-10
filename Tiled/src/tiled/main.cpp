/*
 * main.cpp
 * Copyright 2008-2010, Thorbjørn Lindeijer <thorbjorn@lindeijer.nl>
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

#include "mainwindow.h"
#include "languagemanager.h"
#include "tiledapplication.h"

#include <QDebug>
#include <QtPlugin>

#ifdef STATIC_BUILD
Q_IMPORT_PLUGIN(qgif)
Q_IMPORT_PLUGIN(qjpeg)
Q_IMPORT_PLUGIN(qtiff)
#endif

using namespace Tiled::Internal;

namespace {

struct CommandLineOptions {
    CommandLineOptions()
        : showHelp(false)
        , showVersion(false)
    {}

    bool showHelp;
    bool showVersion;
    QStringList filesToOpen;
};

void showHelp()
{
    // TODO: Make translatable
    qWarning() <<
            "Usage: tiled [option] [files...]\n\n"
            "Options:\n"
            "  -h --help    : Display this help\n"
            "  -v --version : Display the version";
}

void showVersion()
{
    qWarning() << "Tiled (Qt) Map Editor"
            << qPrintable(QApplication::applicationVersion());
}

void parseCommandLineArguments(CommandLineOptions &options)
{
    const QStringList arguments = QCoreApplication::arguments();

    for (int i = 1; i < arguments.size(); ++i) {
        const QString &arg = arguments.at(i);
        if (arg.isEmpty())
            continue;

        if (arg == QLatin1String("--help") || arg == QLatin1String("-h")) {
            options.showHelp = true;
        } else if (arg == QLatin1String("--version")
                || arg == QLatin1String("-v")) {
            options.showVersion = true;
        } else if (arg.at(0) == QLatin1Char('-')) {
            qWarning() << "Unknown option" << arg;
            options.showHelp = true;
        } else {
            options.filesToOpen.append(arg);
        }
    }
}

} // anonymous namespace

int main(int argc, char *argv[])
{
    /*
     * On X11, Tiled uses the 'raster' graphics system by default, because the
     * X11 native graphics system has performance problems with drawing the
     * tile grid.
     */
#ifdef Q_WS_X11
    QApplication::setGraphicsSystem(QLatin1String("raster"));
#endif

    TiledApplication a(argc, argv);

    a.setOrganizationDomain(QLatin1String("mapeditor.org"));
    a.setApplicationName(QLatin1String("Tiled"));
    a.setApplicationVersion(QLatin1String("0.6.1"));
#ifdef Q_WS_MAC
    a.setAttribute(Qt::AA_DontShowIconsInMenus);
#endif

    LanguageManager *languageManager = LanguageManager::instance();
    languageManager->installTranslators();

    CommandLineOptions options;
    parseCommandLineArguments(options);

    if (options.showVersion)
        showVersion();
    if (options.showHelp)
        showHelp();
    if (options.showVersion || options.showHelp)
        return 0;

    MainWindow w;
    w.show();

    QObject::connect(&a, SIGNAL(fileOpenRequest(QString)),
                     &w, SLOT(openFile(QString)));

    if (!options.filesToOpen.empty()) {
        foreach (const QString &fileName, options.filesToOpen)
            w.openFile(fileName);
    } else {
        w.openLastFiles();
    }

    return a.exec();
}
