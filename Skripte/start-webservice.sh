#!/bin/bash

while true
do
  python3 webservice.py 2> last_crash_webservice_log.txt
  echo "Letzter Webservicecrash am `date`" > last_crash_webservice.txt
  sleep 5
done
