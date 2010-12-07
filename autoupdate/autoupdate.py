# -*- coding: utf-8 -*-
# Import
import sqlite3
import ftplib 
import time
import os
import shutil
import glob
import time
import zlib
import os.path
from zipfile import ZipFile
from distutils.dir_util import *

#Funktionen
def uploadDirToFTP(ftp, directory):
    for filename in glob.glob(directory + '/*'):
        absPath = os.path.abspath(filename)
        basename = os.path.basename(filename)
        if os.path.isdir(absPath):
            try:
                print("Befehl: mkd " + basename)
                ftp.mkd(basename)
            except Exception:
                print("Status: " + basename + " already exist!")
            try:
                print("Befehl: CWD " + basename)
                ftp.cwd(basename)
                print("Status: uploaddir: " + basename)
                uploaddir(ftp,absPath)
                print("Befehl: CWD")
                ftp.cwd("..")
            except Exception:
                print("Error:  uploading content of " + absPath);
        else:
            try:
                filelist = ftp.nlst(".")
                if not filelist.count(basename) > 0:
                    f = open(absPath, "rb")
                    print("Befehl: STOR " + basename);
                    ftp.storbinary("STOR " + basename, f)
                    f.close()
                else:
                    print("Status: File "+basename+" already exist!")
            except Exception:
                print("Error:  STOR "+basename+" fehlgeschlagen!")

# False for .svn* folders and *.ogg files
def allowed_file(file):
    if file.lower().endswith(".ogg"):
        return False
    for item in file.split(os.sep):
        if item.startswith(".svn"):
            return False
    return True

def create_zip(basedir, folder_to_zip, zipfile_target = "default.zip"):
    # Temporarily change directory
    current_directory = os.getcwd()
    os.chdir(basedir)

    zip_files = []
    for root,dirs,files in os.walk(folder_to_zip):
        for file in files:
            if allowed_file(os.path.join(root,file)):
                zip_files+=[os.path.join(root,file)]

    update_zip = ZipFile(zipfile_target, "w")
    for file in zip_files:
        print("Archiving " + os.path.join(file))
        update_zip.write(os.path.join(file))
    print("ZIP Archive " + zipfile_target + " created (" + "Filesize: %dM)" % (os.path.getsize(zipfile_target)/(1024**2)))
    update_zip.close() 

    # Change back to previous directory
    os.chdir(current_directory);

# Optionen
update_server=False

ftp_data_server = "invertika.org"
ftp_data_user = "nutzer"
ftp_data_password = "geheim"

ftp_update_server = "invertika.org"
ftp_update_user = "nutzer"
ftp_update_password = "geheim"

path_temp_folder = "/home/manaserv/"

path_repostiory_trunk = "/home/manaserv/invertika/trunk/"
path_repostiory_server = path_repostiory_trunk + "server/"
path_repostiory_serverdata = path_repostiory_trunk + "server-data/"
path_repostiory_clientdata = path_repostiory_trunk + "client-data/"
path_repostiory_clientdata_maps = path_repostiory_trunk + "client-data/maps/"

path_server_root = "/home/manaserv/"
path_server_data = path_server_root + "data/"
path_server_data_scripts = path_server_data + "scripts/"
path_server_data_maps = path_server_data + "maps/"
path_server_start_script = path_server_root + "start-server.sh"
path_server_stop_script = path_server_root + "stop-server.sh"

#Repository Updaten
os.chdir(path_repostiory_trunk)
os.system("svn update")

#Server kompilieren
if update_server==True:
  os.chdir(path_repostiory_server)

  os.system("cmake .")
  #os.system("autoreconf -i")
  os.system("./configure")

  os.system("make")
  os.system("make install")

#Server stoppen und Serverdaten löschen
os.chdir(path_server_root)
os.system(path_server_stop_script)

if os.path.exists(path_server_data)==True:
    shutil.rmtree(path_server_data) 

os.makedirs(path_server_data)

#Serverdateien aus Repository ins Serververzeichnis kopieren
copy_tree(path_repostiory_serverdata, path_server_data,preserve_mode=1, preserve_times=1,  preserve_symlinks=0, update=1, verbose=0,  dry_run=0)
copy_tree(path_repostiory_clientdata_maps, path_server_data_maps,preserve_mode=1, preserve_times=1,  preserve_symlinks=0, update=1, verbose=0,  dry_run=0)

#clientdata xml dateien kopieren
print("clientdata xml dateien kopieren")

print("globe Dateien (XML)")
xml_files = glob.glob(path_repostiory_clientdata + "*.xml")
print(xml_files)

print("Kopiere Clientdateien (XML)")
for file in xml_files:
    print(file)
    shutil.copy(file, path_server_data)

#Client Update erstellen
#wird ohne .svn und Musik (nur im Update Verzeichnis) kopiert

print("Client Update erstellen")

timestring = int(time.time())
fnUpdateFile="update-" + str(timestring) + ".zip"

zip_folder="."
zip_file=path_temp_folder+fnUpdateFile
create_zip(path_repostiory_clientdata, zip_folder, zip_file)

#resources2.txt erstellen
print("resources2.txt erstellen")

data = open(path_temp_folder+fnUpdateFile,"rb")
chkint=zlib.adler32(data.read())
print("Checksumme")
print(chkint)
checksum_updatefile = "%x" % chkint #Checksumme Adler32

fobj = open(path_temp_folder+"resources2.txt", "w+")
fobj.write(fnUpdateFile + " " + checksum_updatefile + "\n") 
fobj.close()

#data.*.invertika.org
#Clientdaten auf testupdate hochladen
ftp = ftplib.FTP(ftp_data_server)

ftp.login(ftp_data_user, ftp_data_password)

#Alte Dateien löschen
#nicht implementiert

#Dateien hochladen
uploadDirToFTP(ftp, path_repostiory_clientdata)

ftp.quit()

#update.*.invertika.org
#Clientdaten auf testupdate hochladen
ftp = ftplib.FTP(ftp_update_server)

ftp.login(ftp_update_user, ftp_update_password)

#Alte Dateien löschen
ftpfiles=ftp.nlst();

for currentFile in ftpfiles:
    if currentFile.find("update")!=-1:
        print(currentFile)
        ftp.delete(currentFile)

#Neue Datei hochladen
f = open(path_temp_folder+fnUpdateFile, "rb") 
ftp.storbinary("STOR " + fnUpdateFile, f) 
f.close()

os.remove(path_temp_folder+fnUpdateFile)

f = open(path_temp_folder+"resources2.txt", "rb") 
ftp.storbinary("STOR resources2.txt", f) 
f.close()

ftp.quit()

#Server starten
#bash script
os.chdir(path_server_root)
os.system(path_server_start_script)

#Mail senden
