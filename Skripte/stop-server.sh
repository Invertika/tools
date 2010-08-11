#!/bin/bash

pkill -SIGTERM manaserv
sleep 5
pkill -SIGKILL manaserv