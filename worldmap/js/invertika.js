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

function GetNextHigherZoomlevel(current) {
    corValue=RoundToNextTileSize(current);
    if (corValue == 10) return 20;
    if (corValue == 20) return 30;
    if (corValue == 30) return 40;
    if (corValue == 40) return 50;
    if (corValue == 50) return 100;
    if (corValue == 100) return 200;
    if (corValue == 200) return 400;
    if (corValue == 400) return 800;
    return corValue;
}

function GetNextLowerZoomlevel(current) {
    corValue=RoundToNextTileSize(current);
    if (corValue == 800) return 400;
    if (corValue == 400) return 200;
    if (corValue == 200) return 100;
    if (corValue == 100) return 50;
    if (corValue == 50) return 40;
    if (corValue == 40) return 30;
    if (corValue == 30) return 20;
    if (corValue == 20) return 10;
    return corValue;
}