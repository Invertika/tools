#!/bin/bash
PATHS=("/home/seeseekey/Development/invertika.github.com/art /home/seeseekey/Development/invertika.github.com/client /home/seeseekey/Development/invertika.github.com/client-mobile /home/seeseekey/Development/invertika.github.com/data /home/seeseekey/Development/invertika.github.com/invertika /home/seeseekey/Development/invertika.github.com/server /home/seeseekey/Development/invertika.github.com/tools /home/seeseekey/Development/invertika.github.com/web")
number=0
for path in $PATHS
do
    cd $path
    n=`git log --pretty="oneline"|wc -l`
    number=`expr $number + $n`
done
echo $number
