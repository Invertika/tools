# -*- encoding: utf-8 -*-

import os,sys,getopt
import xml.etree.ElementTree as etree

def GetMapsInDirectory(path):
    result = ()
    for subdir, dirs, files in os.walk(path):
        if subdir=="./":
            for filename in files:
                result += (filename,)
    return(result)

def GetMonstersOfMap(path):
    tree = etree.parse(path)
    root = tree.getroot()
    result = ()
    for child in root:
        if child.tag=="objectgroup":
            for child2 in child:
                if (child2.tag=="object") and ("type" in child2.attrib):
                    if child2.attrib["type"] == "SPAWN":
                        properties = child2.find('properties')
                        for prop in properties:
                            if ('name' in prop.attrib) and ('value' in prop.attrib):
                                if prop.attrib['name']=='MONSTER_ID':
                                    result += (prop.attrib['value'],)
    return(result)

os.chdir('../../client-data/maps/')
maps = GetMapsInDirectory('./')
monsterarray = {}
for filename in maps:
    monsters = GetMonstersOfMap(filename)
    for monster in monsters:
        if int(monster) in monsterarray.keys():
            monsterarray[int(monster)] += (filename,)
        else:
            monsterarray[int(monster)] = (filename,)
        
for monster in monsterarray:
    print(str(monster)+": "+str(monsterarray[monster]))
