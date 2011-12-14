#!/bin/bash

##Start with "./autoupdate.sh &"
REPOSITORY="/home/manaserv/invertika/data/"
AUTOUPDATE_EXE_FILENAME="/home/manaserv/autoupdate.exe"
AUTOUPDATE_XML_FILENAME="/home/manaserv/autoupdate.xml"

cd $REPOSITORY

#Schleife
while true
do
    sleep 60
    (cd $REPOSITORY && git fetch)
    num_of_changes=`(cd $REPOSITORY && git diff HEAD origin/master|wc -l)`
    [ $num_of_changes != 0 ] && {
        (cd $REPOSITORY && git pull origin master)
        mono $AUTOUPDATE_EXE_FILENAME $AUTOUPDATE_XML_FILENAME
        sleep 600
    }
done