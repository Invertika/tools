#!/bin/bash
while true;
do
    sleep 120;
    REVISION_SERVER=`svn info http://invertika.googlecode.com/svn/trunk | grep Revision|awk '{print $2}'`
    REVISION_LOCAL=`svn info ~/invertika | grep Revision|awk '{print $2}'` # Pfad anpassen
    if [ "$REVISION_SERVER" == "$REVISION_LOCAL" ]
    then
        #echo "Kein Update"
    else
        #echo "Update"
        mono autoupdate.exe autoupdate.xml;
        sleep 3600;
    fi
done; 
