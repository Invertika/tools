#!/usr/bin/env python
# -*- encoding: utf-8 -*-

import sqlite3, hashlib
from optparse import OptionParser

# Kommandozeilenargumente parsem
parser = OptionParser("add_user.py username password") 
(optionen, args) = parser.parse_args() 
if len(args) != 2: 
    parser.error("Es werden exakt zwei Argumente erwartet")
username = args[0]
password = args[1]

# Datenbank erzeugen, falls nicht existent, und verbinden
con = sqlite3.connect('webservice.db') # Warning: This file is created in the current directory
con.execute("CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, username char(100) NOT NULL, password char(100) NOT NULL)")

# Neuen User hinzufügen
con.execute("INSERT INTO users (username, password) VALUES (?, ?)", (username, hashlib.md5(password.encode()).hexdigest()))
con.commit()

# Nachricht ausgeben
print('Der neue User '+username+' wurde hinzugefügt.')
