using Jacky__Wallpaper_Changer;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace Jack__Wallpaper_Changer.Model
{
    public enum WallpaperStyle : int
    {
        Tile,
        Center,
        Stretch,
        Fit,
        Fill,
        Span
    }
    public class WallpaperSetter
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        // this is the system DLL for doing wallpaper stuff
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static int SetWallpaper(String file, WallpaperStyle style)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (style == WallpaperStyle.Tile)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }
                if (style == WallpaperStyle.Center)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == WallpaperStyle.Stretch)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == WallpaperStyle.Fit)
                {
                    key.SetValue(@"WallpaperStyle", 6.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == WallpaperStyle.Fill)
                {
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == WallpaperStyle.Span)
                {
                    key.SetValue(@"WallpaperStyle", 22.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                // set the wallpaper using the external method
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, file, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("catch exception", ex);
                return 1;
            }            
            return 0;
        }
    }
}
