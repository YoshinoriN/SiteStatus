#!bin/bash

find ../ -name '*.cs' | xargs file | grep CRLF | awk -F: '{print $1}' | xargs dos2unix
