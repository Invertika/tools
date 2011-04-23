#!/bin/bash

pkill -SIGTERM server-monitor
sleep 5
pkill -SIGKILL server-monitor