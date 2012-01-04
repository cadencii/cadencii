#!/bin/sh

name=$1
if [ "$1" = "" ]; then
    echo "error: form name not specified"
    exit 1
fi

wd=$(cd $(dirname $0);pwd)

uiPath=$wd/../build/java/com/github/cadencii/${name}Ui.java
touch $uiPath
cd $wd/src/com/github/cadencii && ln -s $uiPath

uiListenerPath=$wd/../build/java/com/github/cadencii/${name}UiListener.java
touch $uiListenerPath
cd $wd/src/com/github/cadencii && ln -s $uiListenerPath

uiImplPath=$wd/../Cadencii/ui/java/${name}UiImpl.java
touch $uiImplPath
cd $wd/src/com/github/cadencii/ui && ln -s $uiImplPath
