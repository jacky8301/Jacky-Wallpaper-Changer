using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jack__Wallpaper_Changer.Model
{
    public class WallpaperItemModel: ObservableObject
    {
        private string _path;
        private WallpaperStyle _pos;
        public WallpaperStyle position
        {
            get { return _pos; }
            set { _pos = value;RaisePropertyChanged(() => position); }
        }
        public string path
        {
            get { return _path; }
            set { _path = value;RaisePropertyChanged(() => path); }
        }
        public override string ToString()
        {
            return path;
        }
    }
}
