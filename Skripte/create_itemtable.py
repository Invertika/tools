#!/usr/bin/python
""" This script creates a lua table from file items.xml """

import os
from xml.etree.ElementTree import ElementTree

# XML source file
source = "/home/manaserv/data/items.xml"
# LUA destination file
destination = "/home/manaserv/data/scripts/ivklibs/itemtable.lua"
# XML sections of interest
itemname = "item"

header="""
----------------------------------------------------------------------------------
-- Ein Lua Table mit allen Items.                                            --
-- Diese Datei wurde automatisch generiert.                                     --
----------------------------------------------------------------------------------
--  Copyright 2008-2010 The Invertika Development Team                          --
--                                                                              --
--  This file is part of Invertika.                                             --
--                                                                              --
--  Invertika is free software; you can redistribute it and/or modify it        --
--  under the terms of the GNU General  Public License as published by the Free --
--  Software Foundation; either version 2 of the License, or any later version. --
----------------------------------------------------------------------------------

module("itemtable", package.seeall)

"""

# Check if source file exists
if not os.path.isfile(source):
  raise IOError("File %s does not exist!" % source)


tree = ElementTree()
xmlelements = tree.parse(source)
items=xmlelements.findall(itemname)

# Print a pretty list
"""
def printitems(items):
  for element in items:
    for key, value in element.items():
      print key.ljust(15) + ' "'+value+'"'
    print 50*"-"

printitems(items)
"""

# Generate lua script
lua=["GLOBAL_ITEM={}"]

for element in items:
  tmp_id=0
  lua_subtable =[]
  for key, value in element.items():
    if key.lower().strip() == "id":
      tmp_id=value
      lua.append('  GLOBAL_ITEM['+tmp_id+'] = {}')
    else:
      lua_subtable += [[key,value]]
  for key,value in lua_subtable:
    lua.append('    GLOBAL_ITEM['+tmp_id+']["'+key+'"] = "'+value+'"')

# Print generated lua script
#print "\n".join(lua)


def write_file(destination, header, textlines):
  if os.path.isfile(destination):
    answer = raw_input("File exists. Overwrite? (y/n) ")
    if answer.lower() != "y":
      return None
  try:
    f = open(destination,"w")
    write_string = ( header + "\n".join(textlines) + "\n--- END" ).encode('utf-8')
    f.write( write_string )
    f.close()
    print "%s lines written." % len(write_string.split("\n"))
  except IOError:
    print "Cannot open file %s for writing" % destination


try: 
  answer = raw_input("Convert "+source+" to "+destination+" (y/n) ")
  if answer.lower() == "y":
    write_file(destination, header, lua)
except KeyboardInterrupt:
  print "\nCancelled."
  
