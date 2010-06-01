#!/bin/bash

# Festlegung des Datums - Format: 20050710
DATE=`date +"%Y%m%d_%H%M%S"`

# Backup-Verzeichnis anlegen 
mkdir /root/backup

# Verzeichnisse die ins Backup integriert werden sollen 
rsync -az --delete --delete-after /home/manaserv /root/backup

# In Backup Verzeichnis wechseln
cd /root/backup

# Alle Dateien mit tar.bz2 komprimieren
FTP_FILE=server.invertika.org-$DATE.tar.bz2
tar cjfp $FTP_FILE manaserv

# Alle komprimierten Dateien per FTP auf den Backup-Server laden
ftp -in <<EOF
open invertika.org
user username password
bin
put $FTP_FILE
close
bye
EOF

# Anschließend alle auf den Server angelegten Dateien wieder löschen
rm -r -f /root/backup
