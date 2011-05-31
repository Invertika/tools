#!/bin/bash

##Start with "./autoupdate.sh &"

#Schleife
while true
do
    sleep 60
    REVISION_SERVER=`svn info http://invertika.googlecode.com/svn/trunk | grep Revision|awk '{print $2}'`
    REVISION_LOCAL=`svn info ~/invertika/trunk | grep Revision|awk '{print $2}'` # Pfad anpassen
    let REVISION_SERVER_INT=$REVISION_SERVER
    let REVISION_LOCAL_INT=$REVISION_LOCAL
    if [ $REVISION_LOCAL_INT -eq 0 ]
    then
        False # Nix tun
    elif [ $REVISION_SERVER_INT -eq 0 ]
    then
        False # Nix tun
    elif [ $REVISION_SERVER != $REVISION_LOCAL ] 
    then
        mono autoupdate.exe autoupdate.xml
        sleep 600
    fi
done 
