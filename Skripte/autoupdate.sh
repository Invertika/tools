#!/bin/bash
while true;
do
    sleep 60;
    REVISION_SERVER=`svn info http://invertika.googlecode.com/svn/trunk | grep Revision|awk '{print $2}'`
    REVISION_LOCAL=`svn info ~/invertika/trunk | grep Revision|awk '{print $2}'` # Pfad anpassen
    if [ "$REVISION_SERVER" != "$REVISION_LOCAL" ] 
      then
        mono autoupdate.exe autoupdate.xml;
        sleep 600;
    fi
done; 
