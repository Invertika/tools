#!/bin/bash

#Start with "./server-monitor.sh &"

#Config
FILENAME="/home/manaserv/.manaserv-game.log"
OLDSIZE=$(stat -c %s $FILENAME) # Dateigröße ermitteln

#Testschleifen
while true;
do
    sleep 120; #2 Minuten

    NEWSIZE=$(stat -c %s $FILENAME) # Dateigröße ermitteln

    echo $OLDSIZE;
    echo $NEWSIZE;

    if [ $OLDSIZE == $NEWSIZE ]; then
	echo "Restart server..."
        ./restart-server.sh
    fi

    OLDSIZE=$NEWSIZE;
done;
