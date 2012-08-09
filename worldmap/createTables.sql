CREATE TABLE IF NOT EXISTS `wmFeatureMaps` (
  `IndexID` int(11) NOT NULL AUTO_INCREMENT,
  `FeatureName` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `FeatureDescription` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `FeatureNameForFilename` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`IndexID`)
);

CREATE TABLE IF NOT EXISTS `wmInformation` (
  `IndexID` int(11) NOT NULL AUTO_INCREMENT,
  `MapID` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `FileName` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `Title` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `Music` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`IndexID`)
);