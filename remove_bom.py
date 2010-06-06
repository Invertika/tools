#!/usr/bin/python

"""
Strip leading byte-order-mark from utf-8 files.
"""

import sys

"""
This script requires Python 2.X
"""

if len(sys.argv) <= 1:
  print "Usage:"
  print "./remove_bom.py scripts/*.lua"
  print "./remove_bom.py maps/*.tmx"
  sys.exit(0)


utf8bom = '\xef\xbb\xbf'

for filename in sys.argv[1:]:
  
  if not (filename.endswith(".tmx") or filename.endswith(".lua")):
    print filename + ": File extensions not allowed"
    continue
  
  f = open(filename,"r")
  buf = f.read(len(utf8bom))
  # Check if file starts with byte order mark
  if buf == utf8bom:
    print filename + ": Removing byte order mark"
    buf = f.read()
    f.close()
    f = open(filename,"w")
    f.write(buf)
  else:
    print filename + ": No byte order mark found"
  f.close()
