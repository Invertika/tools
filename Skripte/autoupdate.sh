#!/bin/bash

##Start with "./autoupdate.sh &"
REPOSITORY="/home/ablu/invertika/invertika/data/"

cd $REPOSITORY

#Schleife
while true
do
    sleep 60
    (cd $REPOSITORY && git fetch)
    num_of_changes=`(cd $REPOSITORY && git diff HEAD origin/master|wc -l)`
    [ $num_of_changes != 0 ] && {
        (cd $REPOSITORY && git pull origin master)
        mono $AUTOUPDATE_EXE_PATH autoupdate.xml
        sleep 600
    }
done 

