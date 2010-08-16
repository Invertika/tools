# -*- encoding: utf-8 -*-

import os,sys,getopt
import xml.etree.ElementTree as etree

sprite_path = "./client-data/graphics/sprites/"
item_path = "./client-data/graphics/items/"
sound_path = "./client-data/sfx/"

def GetSprites(path):
    global sprite_path
    tree = etree.parse(path)
    sprites = tree.findall('//sprite')
    result = ()
    for sprite in sprites:
        result += (RemoveColor(sprite.text),)
    return(result)

def GetImagesFromSprites(path):
    sprites = GetSprites(path)
    existing = GetExistingFiles()
    existingpaths = ()
    for subdir, dirs, files in os.walk(sprite_path):
        if '.svn' in dirs:
            dirs.remove('.svn');
        for filename in files:
            existingpaths += (subdir+filename,)
    result = ()
    for sprite in sprites:
        if sprite in existing:
            for path in existingpaths:
                if os.path.basename(path)==sprite:
                    spritepath = path
            tree = etree.parse(spritepath)
            imagesets = tree.findall('//imageset')
            for imageset in imagesets:
                if 'src' in imageset.attrib:
                    result += (RemoveColor(imageset.attrib['src']),)
    return(result)
            

def GetSounds(path):
    tree = etree.parse(path)
    sounds = tree.findall('//sound')
    result = ()
    for sound in sounds:
        result += (RemoveColor(sound.text),)
    return(result)

def GetImages(path):
    tree = etree.parse(path)
    items = tree.findall('//item')
    result = ()
    for item in items:
        if 'image' in item.attrib:
            result += (RemoveColor(item.attrib['image']),)
    return(result)

def GetUsedFiles(path):
    result = ()
    result += GetSprites(path)
    result += GetImagesFromSprites(path)
    result += GetSounds(path)
    result += GetImages(path)
    return(result)

def RemoveColor(string):
    data = string.split('|')
    return(os.path.basename(data[0]))

def GetInvalidFiles(path):
    files = GetUsedFiles(path)
    existingfiles = GetExistingFiles()
    for filepath in files:
        if not(filepath in existingfiles):
            print(filepath+" invalid")

def GetExistingFiles():
    result = ()
    for subdir, dirs, files in os.walk(sprite_path):
        if '.svn' in dirs:
            dirs.remove('.svn');
        for filename in files:
            result += (filename,)
    for subdir, dirs, files in os.walk(item_path):
        if '.svn' in dirs:
            dirs.remove('.svn');
        for filename in files:
            result += (filename,)
    for subdir, dirs, files in os.walk(sound_path):
        if '.svn' in dirs:
            dirs.remove('.svn');
        for filename in files:
            result += (filename,)
    return(result)

try:
    global svnroot
    svnroot = sys.argv[1]
except IndexError:
    svnroot = ''
if svnroot=='':
    print("Please specify the svn root path as first argument!")
    sys.exit(1)
os.chdir(svnroot)
GetInvalidFiles('./client-data/items.xml')
#print(GetImagesFromSprites('./client-data/items.xml'))
