#!/bin/sh
dotnet ef migrations $@ --context BoardGamesContext --startup-project ../../BoardGamesWebsite/
