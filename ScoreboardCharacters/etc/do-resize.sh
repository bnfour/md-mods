#!/usr/bin/env bash

# this "script" (a command saved for reuse) only makes sense if your setup is similar to mine:
# note that gimp is flatpaked

flatpak run org.gimp.GIMP -i --batch-interpreter plug-in-script-fu-eval -b - < gimp-resize.scm
