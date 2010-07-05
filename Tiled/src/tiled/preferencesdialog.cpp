/*
 * preferencesdialog.cpp
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

#include "preferencesdialog.h"
#include "ui_preferencesdialog.h"

#include "languagemanager.h"
#include "preferences.h"

using namespace Tiled::Internal;

PreferencesDialog::PreferencesDialog(QWidget *parent) :
    QDialog(parent),
    mUi(new Ui::PreferencesDialog),
    mLanguages(LanguageManager::instance()->availableLanguages())
{
    mUi->setupUi(this);

    foreach (const QString &name, mLanguages) {
        QLocale locale(name);
        QString string = QString(QLatin1String("%1 (%2)"))
            .arg(QLocale::languageToString(locale.language()))
            .arg(QLocale::countryToString(locale.country()));
        mUi->languageCombo->addItem(string, name);
    }

    mUi->languageCombo->model()->sort(0);
    mUi->languageCombo->insertItem(0, tr("System default"));

    fromPreferences();

    connect(mUi->languageCombo, SIGNAL(currentIndexChanged(int)),
            SLOT(languageSelected(int)));
}

PreferencesDialog::~PreferencesDialog()
{
    toPreferences();
    delete mUi;
}

void PreferencesDialog::changeEvent(QEvent *e)
{
    QDialog::changeEvent(e);
    switch (e->type()) {
    case QEvent::LanguageChange: {
            const int formatIndex = mUi->layerDataCombo->currentIndex();
            mUi->retranslateUi(this);
            mUi->layerDataCombo->setCurrentIndex(formatIndex);
            mUi->languageCombo->setItemText(0, tr("System default"));
        }
        break;
    default:
        break;
    }
}

void PreferencesDialog::languageSelected(int index)
{
    const QString language = mUi->languageCombo->itemData(index).toString();
    Preferences *prefs = Preferences::instance();
    prefs->setLanguage(language);
}

void PreferencesDialog::fromPreferences()
{
    const Preferences *prefs = Preferences::instance();
    mUi->reloadTilesetImages->setChecked(prefs->reloadTilesetsOnChange());
    mUi->enableDtd->setChecked(prefs->dtdEnabled());

    int formatIndex = 0;
    switch (prefs->layerDataFormat()) {
    case TmxMapWriter::XML:
        formatIndex = 0;
        break;
    case TmxMapWriter::Base64:
        formatIndex = 1;
        break;
    case TmxMapWriter::Base64Gzip:
    default:
        formatIndex = 2;
        break;
    case TmxMapWriter::Base64Zlib:
        formatIndex = 3;
        break;
    case TmxMapWriter::CSV:
        formatIndex = 4;
        break;
    }
    mUi->layerDataCombo->setCurrentIndex(formatIndex);

    // Not found (-1) ends up at index 0, system default
    int languageIndex = mUi->languageCombo->findData(prefs->language());
    if (languageIndex == -1)
        languageIndex = 0;
    mUi->languageCombo->setCurrentIndex(languageIndex);
}

void PreferencesDialog::toPreferences()
{
    Preferences *prefs = Preferences::instance();

    prefs->setReloadTilesetsOnChanged(mUi->reloadTilesetImages->isChecked());
    prefs->setDtdEnabled(mUi->enableDtd->isChecked());
    prefs->setLayerDataFormat(layerDataFormat());
}

TmxMapWriter::LayerDataFormat PreferencesDialog::layerDataFormat() const
{
    switch (mUi->layerDataCombo->currentIndex()) {
    case 0:
        return TmxMapWriter::XML;
    case 1:
        return TmxMapWriter::Base64;
    case 2:
    default:
        return TmxMapWriter::Base64Gzip;
    case 3:
        return TmxMapWriter::Base64Zlib;
    case 4:
        return TmxMapWriter::CSV;
    }
}
