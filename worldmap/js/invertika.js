function GetOuterWorldMapFilename(internalX, internalY, zoomLevel) {
    var vX = "";
    if (internalX == 0) vX = "o";
    else if (internalX < 0) vX = "n";
    else if (internalX > 0) vX = "p";

    var vY = "";
    if (internalY == 0) vY = "o";
    else if (internalY < 0) vY = "n";
    else if (internalY > 0) vY = "p";

    var ret = sprintf("ow-%s%'04d-%s%'04d-o0000-%d.png", vX, Math.abs(internalX), vY, Math.abs(internalY), zoomLevel);
    return ret;
}

function ChangeZoomLevelOfMapName(filename, zoomLevel) {
    var mySplitResult = filename.split("-");
    mySplitResult[4] = zoomLevel + ".png";

    return mySplitResult[0] + mySplitResult[1] + mySplitResult[2] + mySplitResult[3] + mySplitResult[4];
}

function RoundToNextTileSize(size) {
    if (size <= 15) return 10;
    if (size <= 25) return 20;
    if (size <= 35) return 30;
    if (size <= 45) return 40;
    if (size <= 75) return 50;
    if (size <= 150) return 100;
    if (size <= 300) return 200;
    if (size <= 600) return 400;
    return 800;
}