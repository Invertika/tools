#!/bin/bash

#Config
FILENAME="~/.manaserv-game.log"
OLDSIZE=-1

#Testschleifen
while true;
do
    sleep 300; #5 Minuten
    sleep 5;

    NEWSIZE=$(ls -l $FILENAME | tr -s " " | cut -d " " -f 5)

    if [ "$OLDSIZE" == "$NEWSIZE" ] 
      then
        restart-server.sh
    fi

    $OLDSIZE=$NEWSIZE
done; 
