if [ $# -eq 0 ]
then
    $SNAP/usr/bin/mono $SNAP/ChronoWiz.gtk.exe > /dev/null 2>&1
else
    $SNAP/usr/bin/mono $SNAP/ChronoWiz.gtk.exe
fi
