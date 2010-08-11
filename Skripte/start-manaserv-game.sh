#!/bin/bash

while true
do
  manaserv-game --v 3
  echo "Letzter Servercrash am `date`" > last_crash_manaserv-game.txt
  sleep 5
done