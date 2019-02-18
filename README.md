# Folder Palette

This is a simple application that allows you to color a folder using the context menu. When you right click on a folder, you will get the option Folder Palette. It will create an icon in that color and assign it to the folder for you. If the folder color doesn't update, you may choose Refresh Icon Cache to force it to update.

## Getting Started

Clone this repo and build in VS2017. I use the ImageMagick Library for modifying and creating .ico files. If the dependency isn't automatically added, you may add it through nuget. 

Compiling will give you FolderPalette.exe.

Make sure you place the baseicon.ico file in the same directory as your FolderPalette.exe file. This icon is normally hidden.

To color a folder use:

`FolderPalette /color FOLDERPATH R G B`

Example (red folder):

`FolderPalette /color G:\Test 255 0 0`

Note: In order to install the context menu, run the reg file. You made need to modify it to your build path.

## Known Issues

1. Currently there is bug that doesn't allow us to change a folder's color once the folder has been created.

2. Currently there is no way from the context menu to restore to a normal folder color. You can, however delete the generated .ico file out of the folder to restore the normal color (the .ico file is hidden by default).

3. Sometimes we must refresh the icon cache to get icons to show up. Ideally, we should refresh after we set the icon using a bat file or command line.

## Authors

* **Walker 'Nice Rain' Twyman**
