#!/bin/bash

./stop-server.sh
sleep 1
./start-manaserv-account.sh &
sleep 5
./start-manaserv-game.sh &