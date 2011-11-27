#!/bin/bash
PATHS=("/home/ablu/invertika/invertika/server-data /home/ablu/invertika/invertika/client-data")
number=0
for path in $PATHS
do
    cd $path
    n=`git log --pretty="oneline"|wc -l`
    number=`expr $number + $n`
done
echo $number
