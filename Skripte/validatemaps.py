# -*- encoding: utf-8 -*-

import os,sys,getopt
import xml.etree.ElementTree as etree

def GetTilesetsOfMap(path):
    tree = etree.parse(path)
    root = tree.getroot()
    result = ()
    for child in root:
        if child.tag=="tileset":
            for child2 in child:
                if child2.tag=="image":
                    result += (child2.attrib["source"],)
    return(result)

def ValidateTilesets(tilesets):
    result = True
    for tileset in tilesets:
        result = result & os.access(tileset,os.F_OK)
    return(result)

def GetInvalidTilesets(filename):
    tilesets = GetTilesetsOfMap(filename)
    result = """"""
    for tileset in tilesets:
        if os.access(tileset,os.F_OK)==False:
            result += """  """+tileset+"""
"""
    return(result)

def GetMapsInDirectory(path):
    result = ()
    for subdir, dirs, files in os.walk(path):
        if subdir=="./":
            for filename in files:
                result += (filename,)
    return(result)

try:
    global pathwithmaps
    pathwithmaps = sys.argv[1]
except IndexError:
    pathwithmaps = ''
if pathwithmaps=='':
    print("Please specify the path with the maps as first argument!")
    sys.exit(1)
os.chdir(pathwithmaps)
files = GetMapsInDirectory("./")
for filename in files:
    if ValidateTilesets(GetTilesetsOfMap(filename))==False:
        print(filename+": INVALID")
        print(GetInvalidTilesets(filename))