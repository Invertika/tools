#!/bin/bash

#Config
FILENAME="/home/manaserv/.manaserv-game.log"
OLDSIZE=0

#Testschleifen
while true;
do
    sleep 300; #5 Minuten

    NEWSIZE=$(ls -l $FILENAME | tr -s " " | cut -d " " -f 5)

    if [ "$OLDSIZE" == "$NEWSIZE" ]; then
	echo "Restart server..."
        restart-server.sh
    fi

    $OLDSIZE=$NEWSIZE;
done;