using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

//Include ImageMagick

using ImageMagick;

namespace FolderPalette
{
    class Program
    {

        //Creates and saves a new icon with our generated color scheme
        static void CreateIcon(string path, MagickColor color, Percentage alpha)
        {
            Console.WriteLine("Icon Generated!");
            Console.WriteLine("Path:" + path);

            string dir = System.AppContext.BaseDirectory;

            //Import our base icon as a collection of layers
            using (MagickImageCollection collection = new MagickImageCollection(dir + "\\baseicon.ico"))
            {
                //Loop through each layer and apply our color overlay
                for (int i = 0; i < collection.Count(); i++)
                {
                    //collection[i].Modulate(new Percentage(100), new Percentage(0)); //This desaturates
                    collection[i].Colorize(color, alpha);
                }

                //Write our new icon file to disk
                collection.Write(path + "\\icon.ico");

                //Set this file attribute to hidden
                File.SetAttributes(path + "\\icon.ico", File.GetAttributes(path + "\\icon.ico") | FileAttributes.Hidden);
            }
        }

        //Sets the icon for a specific folder using desktop.ini
        // Special Thanks to Mike Zhang[MSFT]
        static void SetFolderIcon(string path)
        {
            Console.WriteLine("Setting Folder Icon");

            //folder path
            LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();
            FolderSettings.dwMask = 0x10;
            FolderSettings.pszIconFile = @path + "\\icon.ico";
            FolderSettings.iIconIndex = 0;

            UInt32 FCS_READ = 0x00000001;
            UInt32 FCS_FORCEWRITE = 0x00000002;
            UInt32 FCS_WRITE = FCS_READ | FCS_FORCEWRITE;

            string pszPath = path;
            UInt32 HRESULT = SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_WRITE);
            //Console.WriteLine(HRESULT.ToString("x"));
            //Console.ReadLine();

        }

        static void DeleteFolderIcon()
        {
            Console.WriteLine("Deleting Old Icon");
            File.Delete(@"G:\Documents\Visual Studio 2017\Projects\FolderPalette\FolderPalette\bin\Debug\Test\Icon.ico");
            
        }


        //Uses a cmd window to run our cache command
        static void ResetWindowsIconCache()
        {
            Console.WriteLine("Clearing Icon Cache");
            const string path = @"CMD.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(path);

            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = "ipconfig -all";

            Process.Start(startInfo);
        }


        //Extracts all pngs from an icon file
        static void ExportPng()
        {
            using (MagickImageCollection collection = new MagickImageCollection("base.ico"))
            {
                //Loop through all our image layers
                for(int i = 0; i < collection.Count(); i++)
                {
                    collection[i].Write("newicon" + i + ".png");
                }
            }
        }


        //Creates a composite icon from our png images
        //This allows us to generate new icon files
        static void CreateComposite()
        {
            using (MagickImageCollection images = new MagickImageCollection())
            {
                images.Add("newicon0.png");
                images.Add("newicon1.png");
                images.Add("newicon2.png");
                images.Add("newicon3.png");
                images.Add("newicon4.png");
                images.Add("newicon5.png");
                images.Add("newicon6.png");
                images.Add("newicon7.png");

                images.Write("baseicon.ico");
            }
        }

        static void Main(string[] args)
        {
            //Default alpha
            Percentage alpha = new Percentage(60);
           
            //DeleteFolderIcon();
            //Creates our .ico file with color and alpha
            //ResetWindowsIconCache();

            if (args.Length > 0)
            {

                if (args[0] == "/color" && args.Length > 4)
                {
                    Console.WriteLine("color");
                    string dir = args[1]; //pass our path first

                    MagickColor color = MagickColor.FromRgb(Convert.ToByte(args[2]), Convert.ToByte(args[3]), Convert.ToByte(args[4]));
                    CreateIcon(dir, color, alpha);
                    SetFolderIcon(dir);
                }
                else if (args[0] == "/restore")
                {
                    Console.WriteLine("restore");
                }
            }
            else
            {
                Console.WriteLine("Please enter a parameter");
            }
            //Console.ReadKey();
        }


        //Set our folder attributes - Special Thanks to Mike Zhang[MSFT]
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        static extern UInt32 SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, UInt32 dwReadWrite);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct LPSHFOLDERCUSTOMSETTINGS
        {
            public UInt32 dwSize;
            public UInt32 dwMask;
            public IntPtr pvid;
            public string pszWebViewTemplate;
            public UInt32 cchWebViewTemplate;
            public string pszWebViewTemplateVersion;
            public string pszInfoTip;
            public UInt32 cchInfoTip;
            public IntPtr pclsid;
            public UInt32 dwFlags;
            public string pszIconFile;
            public UInt32 cchIconFile;
            public int iIconIndex;
            public string pszLogo;
            public UInt32 cchLogo;
        }

    }
}
