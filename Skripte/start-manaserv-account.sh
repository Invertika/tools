#!/bin/bash

while true
do
  manaserv-account --v 3
  echo "Letzter Servercrash am `date`" > last_crash_manaserv-account.txt
  sleep 5
done