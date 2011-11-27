#!/bin/bash

##Start with "./autoupdate.sh &"
REPOSITORY="/path/to/rep/"

#Schleife
while true
do
    sleep 60
    git fetch --work-tree=$REPOSITORY
    num_of_changes=`git diff HEAD origin/master --work-tree=$REPOSITORY|wc -l`
    if [ $num_of_changes != 0 ]
        git pull origin master --work-tree=$REPOSITORY
        mono autoupdate.exe autoupdate.xml
        sleep 600
    fi
done 
