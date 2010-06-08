# -*- coding: utf-8 -*-# Importimport sqlite3import ftplib import timeimport osimport shutilimport globimport zlibimport os.pathfrom zipfile import ZipFilefrom distutils.dir_util import *#Funktionendef create_zip(basedir, folder_to_zip, zipfile_target = "default.zip"):    # False for .svn* folders and *.ogg files    def allowed_file(file):        if file.lower().endswith(".ogg"):            return False        for item in file.split(os.sep):            if item.startswith(".svn"):                return False        return True    # Temporarily change directory    current_directory = os.getcwd()    os.chdir(basedir)    zip_files = []    for root,dirs,files in os.walk(folder_to_zip):        for file in files:            if allowed_file(os.path.join(root,file)):                zip_files+=[os.path.join(root,file)]	    update_zip = ZipFile(zipfile_target, "w")    for file in zip_files:        print "Archiving " + os.path.join(file)        update_zip.write(os.path.join(file))    print "ZIP Archive " + zipfile_target + " created (" + "Filesize: %dM)" % (os.path.getsize(zipfile_target)/(1024**2))    update_zip.close()     # Change back to previous directory    os.chdir(current_directory);# Optionenftp_server = "invertika.org"ftp_user = "nutzer"ftp_password = "geheim"path_temp_folder = "/root/"path_repostiory_root = "/root/invertika.googlecode.com/"path_repostiory_server = path_repostiory_root + "server/"path_repostiory_serverdata = path_repostiory_root + "server-data/"path_repostiory_clientdata = path_repostiory_root + "client-data/"path_repostiory_clientdata_maps = path_repostiory_root + "client-data/maps/"path_server_data = "/home/manaserv/data/"path_server_data_scripts = "/home/manaserv/data/scripts/"path_server_data_maps = "/home/manaserv/data/maps/"path_server_start_script = "/home/manaserv/start-server.sh"path_server_stop_script = "/home/manaserv/stop-server.sh"path_server_root = "/home/manaserv/"#Repository Updatenos.chdir(path_repostiory_root)os.system("svn update")#Server kompilierenos.chdir(path_repostiory_server)#os.system("cmake -G \"Unix Makefiles\"")os.system("autoreconf -i")os.system("./configure")os.system("make")os.system("make install")#Server stoppen und Serverdaten löschen#os.system("su manaserv")os.chdir(path_server_root)os.system(path_server_stop_script)if os.path.exists(path_server_data)==True:    shutil.rmtree(path_server_data) os.makedirs(path_server_data)#Serverdateien aus Repository ins Serververzeichnis kopieren#IGNORE_PATTERNS = ('*.pyc','CVS','^.git','tmp','.svn')#copytree(path_repostiory_serverdata, path_server_data, ignore=shutil.ignore_patterns(IGNORE_PATTERNS)) #Serverdata#copytree(path_repostiory_clientdata_maps, path_server_data, ignore=shutil.ignore_patterns(IGNORE_PATTERNS)) #Mapscopy_tree(path_repostiory_serverdata, path_server_data,preserve_mode=1, preserve_times=1,  preserve_symlinks=0, update=1, verbose=0,  dry_run=0)copy_tree(path_repostiory_clientdata_maps, path_server_data_maps,preserve_mode=1, preserve_times=1,  preserve_symlinks=0, update=1, verbose=0,  dry_run=0)#clientdata xml dateien kopierenprint "clientdata xml dateien kopieren"print "globe Dateien (XML)"xml_files = glob.glob(path_repostiory_clientdata + "*.xml")print xml_filesprint "Kopiere Clientdateien (XML)"for file in xml_files:    print file    shutil.copy(file, path_server_data)#Client Update erstellen#ohne svn kopieren(ohne musik evt, wegen der größe)print "Client Update erstellen"zip_folder="."zip_file=path_temp_folder+"update-sgrty.zip"create_zip(path_repostiory_clientdata, zip_folder, zip_file)#resources2.txt erstellenprint "resources2.txt erstellen"data = open(path_temp_folder+"update-sgrty.zip","rb")chkint=zlib.adler32(data.read())print "Checksumme"print chkintchecksum_updatefile = "%x" % chkint #Checksumme Adler32fobj = open(path_temp_folder+"resources2.txt", "w+")fobj.write("update-sgrty.zip " + checksum_updatefile + "\n") fobj.close()#Clientdaten auf testupdate hochladenftp = ftplib.FTP(ftp_server)ftp.login(ftp_user, ftp_password)f = open(path_temp_folder+"update-sgrty.zip", "r") ftp.storbinary("STOR update-sgrty.zip", f) f.close()f = open(path_temp_folder+"resources2.txt", "r") ftp.storbinary("STOR resources2.txt", f) f.close()ftp.quit()#Server starten#bash scriptos.chdir(path_server_root)os.system("sudo -H -u manaserv " + path_server_start_script)# s #Mail senden