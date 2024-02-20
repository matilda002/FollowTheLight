#!/bin/bash
curl -s localhost:3000/player/register -d kompis
curl -s localhost:3000/player/register -d bästis
curl -s localhost:3000/player/1/chat -d hej
curl -s localhost:3000/player/2/chat -d hur
curl -s localhost:3000/player/2/chat -d mår
curl -s localhost:3000/player/2/chat -d du
curl -s localhost:3000/player/1/chat -d jag
curl -s localhost:3000/player/1/chat -d mår
curl -s localhost:3000/player/1/chat -d bra
curl -s localhost:3000/player/1/chat -d tack
curl -s localhost:3000/player/1/chat -d !
curl -s localhost:3000/player/2/chat -d skönt
curl -s localhost:3000/player/2/chat -d att
curl -s localhost:3000/player/2/chat -d höra
