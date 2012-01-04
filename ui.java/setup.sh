#!/bin/sh

wd=$(cd $(dirname $0);pwd)
files=`ls $wd/../Cadencii/ui/java/*UiImpl.java`

rm -rf $wd/src/com/github/cadencii/ui
rm -rf $wd/src/com/github/cadencii/*.java
rm -f $wd/src/resources
mkdir -p $wd/src/com/github/cadencii/ui

for file in $files
do
    uiImplementation=`basename $file | sed -e 's/UiImpl[.]java$//g'`
    expectedUiPath=java/com/github/cadencii/${uiImplementation}Ui.java
    if [ -e $wd/../build/$expectedUiPath ]; then
        cd $wd/src/com/github/cadencii && ln -s $wd/../build/$expectedUiPath
    fi

    expectedUiListenerPath=java/com/github/cadencii/${uiImplementation}UiListener.java
    if [ -e $wd/../build/$expectedUiListenerPath ]; then
        cd $wd/src/com/github/cadencii && ln -s $wd/../build/$expectedUiListenerPath
    fi

    cd $wd/src/com/github/cadencii/ui && ln -s $file
done

cd $wd/src/com/github/cadencii && ln -s $wd/../build/java/com/github/cadencii/UiBase.java
cd $wd/src/com/github/cadencii/ui && ln -s $wd/../Cadencii/ui/java/DialogBase.java
cd $wd/src/com/github/cadencii/ui && ln -s $wd/../Cadencii/ui/java/ListView.java
cd $wd/src && ln -s $wd/../build/java/resources
