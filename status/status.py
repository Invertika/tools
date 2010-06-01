# Import
import sqlite3
import ftplib 
import time
import os

# Optionen
ftp_server = "invertika.org"
ftp_user = "nutzername"
ftp_password = "geheim"

path_mana_db = "/home/manaserv/invertika.db"
path_server_file = "server.txt"
path_rrd_file = "invertika.rrd"

# Daten in die Datei schreiben
server_file = open(path_server_file, 'w')

connection = sqlite3.connect(path_mana_db)
cursor = connection.cursor()

server_file.write(str(time.time()) + '\n')

countPlayer = 0
cursor.execute('SELECT * FROM mana_v_online_chars')
for row in cursor: countPlayer+=1
server_file.write(str(countPlayer) + '\n')

cursor.execute('SELECT * FROM mana_v_online_chars ORDER BY name')
for row in cursor: server_file.write(row[3] + '\n')

cursor.close()
connection.close()

server_file.close()

# Auswertung RRD
if os.path.isfile(path_rrd_file) == False:
    os.system('rrdtool create invertika.rrd	           \
	--step 60 \
	DS:online:GAUGE:75:0:15000 \
	RRA:MAX:0.5:1:60          \
	RRA:MAX:0.5:6:700          \
	RRA:MAX:0.5:24:775         \
	RRA:MAX:0.5:288:797')
	
#Update
os.system('rrdtool update ' + path_rrd_file + ' N:' + str(countPlayer))

os.system('rrdtool graph ivk_0.png --start -3600         \
        -t "Invertika Serverstatus - Stunde"      \
        -g -l 0 DEF:inoctets=' + path_rrd_file + ':online:MAX  \
        LINE2:inoctets#4a3118:"Online Players" \
        -c SHADEA#00000000 \
        -c SHADEB#00000000 \
        -c BACK#00000000 \
        -c CANVAS#e3d3a77f \
		-X 0')
		
os.system('rrdtool graph ivk_1.png --start -86400         \
        -t "Invertika Serverstatus - Tag"      \
        -g -l 0 DEF:inoctets=' + path_rrd_file + ':online:MAX  \
        LINE1:inoctets#4a3118:"Online Players" \
        -c SHADEA#00000000 \
        -c SHADEB#00000000 \
        -c BACK#00000000 \
        -c CANVAS#e3d3a77f \
		-X 0')

os.system('rrdtool graph ivk_2.png --start -604800         \
        -t "Invertika Serverstatus - Woche"      \
        -g -l 0 DEF:inoctets=' + path_rrd_file + ':online:MAX  \
        LINE1:inoctets#4a3118:"Online Players" \
        -c SHADEA#00000000 \
        -c SHADEB#00000000 \
        -c BACK#00000000 \
        -c CANVAS#e3d3a77f \
		-X 0')
		
os.system('rrdtool graph ivk_3.png --start -2629743         \
        -t "Invertika Serverstatus - Monat"      \
        -g -l 0 DEF:inoctets=' + path_rrd_file + ':online:MAX  \
        LINE1:inoctets#4a3118:"Online Players" \
        -c SHADEA#00000000 \
        -c SHADEB#00000000 \
        -c BACK#00000000 \
        -c CANVAS#e3d3a77f \
		-X 0')
		
os.system('rrdtool graph ivk_4.png --start -31556926         \
        -t "Invertika Serverstatus - Jahr"      \
        -g -l 0 DEF:inoctets=' + path_rrd_file + ':online:MAX  \
        LINE1:inoctets#4a3118:"Online Players" \
        -c SHADEA#00000000 \
        -c SHADEB#00000000 \
        -c BACK#00000000 \
        -c CANVAS#e3d3a77f  \
		-X 0')
		
os.system('composite ivk_0.png status_bg.png ivk_hour.jpg')
os.system('composite ivk_1.png status_bg.png ivk_day.jpg')
os.system('composite ivk_2.png status_bg.png ivk_week.jpg')
os.system('composite ivk_3.png status_bg.png ivk_month.jpg')
os.system('composite ivk_4.png status_bg.png ivk_year.jpg')

# Datei per FTP hochladen
ftp = ftplib.FTP(ftp_server)
ftp.login(ftp_user, ftp_password)

f = open(path_server_file, "r") 
ftp.storbinary("STOR server.txt", f) 
f.close()

f = open("ivk_hour.jpg", "r") 
ftp.storbinary("STOR ivk_hour.jpg", f) 
f.close()

f = open("ivk_day.jpg", "r") 
ftp.storbinary("STOR ivk_day.jpg", f) 
f.close()

f = open("ivk_week.jpg", "r") 
ftp.storbinary("STOR ivk_week.jpg", f) 
f.close()

f = open("ivk_month.jpg", "r") 
ftp.storbinary("STOR ivk_month.jpg", f) 
f.close()

f = open("ivk_year.jpg", "r") 
ftp.storbinary("STOR ivk_year.jpg", f) 
f.close()

ftp.quit()