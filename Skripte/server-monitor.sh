#!/bin/bash

#Start with "./server-monitor.sh &"

#Config
FILENAME="/home/manaserv/.manaserv-game.log"
OLDSIZE=$(ls -l $FILENAME | tr -s " " | cut -d " " -f 5)

#Testschleifen
while true;
do
    sleep 300; #5 Minuten

    NEWSIZE=$(stat -c %s $FILENAME) # Datei-größe ermitteln

    if [ $OLDSIZE == $NEWSIZE ]; then
	echo "Restart server..."
        ./restart-server.sh
    fi

    OLDSIZE=$NEWSIZE;
done;
