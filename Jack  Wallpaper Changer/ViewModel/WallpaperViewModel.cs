using GalaSoft.MvvmLight;
using Jack__Wallpaper_Changer.Model;
using Jacky__Wallpaper_Changer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Jack__Wallpaper_Changer.ViewModel
{
    public class WallpaperViewModel : ViewModelBase
    {
        public WallpaperViewModel()
        {
            Load();
        }
        ~WallpaperViewModel()
        {
            Save();
        }

        private const string DATA_FILE_NAME = "wallpaper.json";
        private ObservableCollection<WallpaperItemModel> wallpaperList;

        public ObservableCollection<WallpaperItemModel> WallpaperList
        {
            get { return wallpaperList; }
            set { wallpaperList = value; RaisePropertyChanged(() => WallpaperList); }
        }
        public void Load()
        {
            LogHelper.WriteLog("Loading Data....");
            string jsonFile = Path.Combine(System.Windows.Forms.Application.StartupPath, DATA_FILE_NAME);
            if (File.Exists(jsonFile))
            {
                using (StreamReader file = File.OpenText(jsonFile))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject jsonObj = (JObject)JToken.ReadFrom(reader);
                        JArray a = (JArray)jsonObj["items"];
                        WallpaperList = JsonConvert.DeserializeObject<ObservableCollection<WallpaperItemModel>>(a.ToString());
                        JObject current = (JObject)jsonObj["current"];
                        if (current == null)
                        {
                            if (WallpaperList.Count > 0)
                            {
                                CurrentSet = wallpaperList[0];
                            }
                        }
                        else
                        {
                            string path = (string)current["path"];
                            int nIndex = GetCurrentWallpaperIndexByPath(path);
                            if (nIndex >= 0 || nIndex < WallpaperList.Count)
                            {
                                CurrentSet = WallpaperList[nIndex];
                            }
                        }
                    }
                }
            }
        }
        public void SaveCurrent()
        {
            string file = Path.Combine(System.Windows.Forms.Application.StartupPath, DATA_FILE_NAME);
            string strJson = @"{}";
            if (File.Exists(file))
            {
                strJson = File.ReadAllText(file);
            }
            JObject root = JObject.Parse(strJson);
            root["current"] = JObject.FromObject(CurrentSet);
            File.WriteAllText(file, root.ToString());

        }
        public void SetWallpaper(string file, WallpaperStyle style)
        {
            WallpaperSetter.SetWallpaper(file, style);
            int nIndex = GetCurrentWallpaperIndexByPath(file);
            if (nIndex >= 0 && nIndex < WallpaperList.Count)
            {
                WallpaperList[nIndex].position = style;
                CurrentSet = WallpaperList[nIndex];
            }
        }
        public void Save()
        {
            LogHelper.WriteLog("Saving Data....");
            string file = Path.Combine(System.Windows.Forms.Application.StartupPath, DATA_FILE_NAME);
            string strJson = @"{}";
            JObject root = JObject.Parse(strJson);
            //  保存壁纸列表
            root["items"] = JArray.FromObject(WallpaperList);
            // 保存当前项
            if (CurrentSet != null)
            {
                root["current"] = JObject.FromObject(CurrentSet);
            }
            // 创建或者覆盖原有文件
            File.WriteAllText(file, root.ToString());
        }
        public void AddWallpaper(WallpaperItemModel item)
        {
            WallpaperList.Add(item);
            string file = Path.Combine(System.Windows.Forms.Application.StartupPath, DATA_FILE_NAME);
            string strJson = @"{}";
            if (File.Exists(file)) { strJson = File.ReadAllText(file); }
            JObject root = JObject.Parse(strJson);
            JArray arrItem = (JArray)root["items"];
            arrItem.Add(JObject.FromObject(item));
            File.WriteAllText(file, root.ToString());
        }
        public void ClearAllWallpaper()
        {
            WallpaperList.Clear();
            CurrentSet = null;
            Save();
        }
        public void RemoveWallpapers(string[] paths)
        {
        }
        public void RemoveWallpaper(string path)
        {
            bool bRemoved = false;
            foreach (var item in WallpaperList)
            {
                if (item.path.Equals(path, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    WallpaperList.Remove(item);
                    bRemoved = true;
                    break;
                }
            }
            //  保存到JSON
            if (bRemoved)
            {
                string file = Path.Combine(System.Windows.Forms.Application.StartupPath, DATA_FILE_NAME);
                if (File.Exists(file))
                {
                    string strJson = File.ReadAllText(file);
                    JObject root = JObject.Parse(strJson);
                    JArray arrItem = (JArray)root["items"];
                    foreach (var item in arrItem)
                    {
                        string temp = (string)item["path"];
                        if (temp.Equals(path, System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            arrItem.Remove(item);
                            break;
                        }
                    }
                    File.WriteAllText(file, root.ToString());
                }
            }
        }
        private WallpaperItemModel currentSet = null;

        public WallpaperItemModel CurrentSet
        {
            get { return currentSet; }
            set { currentSet = value; RaisePropertyChanged(() => CurrentSet); }
        }
        public void NextWallpaper()
        {
            if (WallpaperList.Count == 0)
            {
                return;
            }
            int nIndex = GetCurrentWallpaperIndexByPath(CurrentSet.path);
            if (nIndex == WallpaperList.Count - 1 || nIndex == -1)
            {
                nIndex = 0;
            }
            else
            {
                nIndex++;
            }
            CurrentSet = WallpaperList[nIndex];
            this.SetWallpaper(CurrentSet.path, CurrentSet.position);
        }
        public void PreviewWallpaper()
        {
            if (WallpaperList.Count == 0)
            {
                return;
            }
            int nIndex = GetCurrentWallpaperIndexByPath(CurrentSet.path);
            if (nIndex > 0)
            {
                nIndex--;
            }
            else
            {
                nIndex = WallpaperList.Count-1;
            }
            CurrentSet = WallpaperList[nIndex];
            this.SetWallpaper(CurrentSet.path, CurrentSet.position);
        }

        private int GetCurrentWallpaperIndexByPath(string wallpaperPath)
        {
            int index = -1;
            foreach (var item in WallpaperList)
            {
                if (item.path.Equals(wallpaperPath, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    index = WallpaperList.IndexOf(item);
                    break;
                }
            }
            return index;
        }
    }
}
