using Jack__Wallpaper_Changer.Model;
using Jack__Wallpaper_Changer.ViewModel;
using Jacky__Wallpaper_Changer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Jack__Wallpaper_Changer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotKey hotkeyNextWallpaper;
        private HotKey hotkeyPreviewWallpaper;
        private readonly System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public MainWindow()
        {
            LogHelper.WriteLog("Wallpaper Changer Start!");
            InitializeComponent();
            InitNotifyIcon();
        }
        ~MainWindow()
        {
            LogHelper.WriteLog("Wallpaper Changer End!");
        }
        private void RegisterHotKeys()
        {
            hotkeyPreviewWallpaper = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL_WIN, HotKey.Keys.KEY_PAGEUP);
            hotkeyPreviewWallpaper.OnHotKey += OnPreviewWallpaper;
            hotkeyNextWallpaper = new HotKey(this, HotKey.KeyFlags.MOD_CONTROL_WIN, HotKey.Keys.KEY_PAGEDOWN);
            hotkeyNextWallpaper.OnHotKey += OnNextWallpaper;
        }

        private void InitNotifyIcon()
        {
            this.notifyIcon.BalloonTipText = "Jacky Wallpaper Changer";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "Jacky Wallpaper Changer";
            string iconfile = Path.Combine(System.Windows.Forms.Application.StartupPath, "logo.ico");
            this.notifyIcon.Icon = new System.Drawing.Icon(iconfile);
            this.notifyIcon.Visible = true;
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(Show);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) this.Show(o, e);
            });
        }
        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void BtnSetWallapaer_Click(object sender, RoutedEventArgs e)
        {
            if (lbWallpaper.SelectedItem != null)
            {
                WallpaperStyle wallpaperStyle = 0;
                string txtStyle = cbWallpaperStyle.Text;
                switch (txtStyle)
                {
                    case "填充":
                        wallpaperStyle = WallpaperStyle.Fill;
                        break;
                    case "适应":
                        wallpaperStyle = WallpaperStyle.Fit;
                        break;
                    case "拉伸":
                        wallpaperStyle = WallpaperStyle.Stretch;
                        break;
                    case "平铺":
                        wallpaperStyle = WallpaperStyle.Tile;
                        break;
                    case "居中":
                        wallpaperStyle = WallpaperStyle.Center;
                        break;
                    case "跨区":
                        wallpaperStyle = WallpaperStyle.Span;
                        break;
                    default:
                        break;
                }
                string wallpaperFile = ((WallpaperItemModel)lbWallpaper.SelectedItem).path;
                WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
                if (vm != null)
                {
                    vm.SetWallpaper(wallpaperFile, wallpaperStyle);
                    vm.Save();
                }

            }
        }
        private void BtnAddFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = @"I:\Picture\Wallpaper";
            openFileDialog.Filter = "所有文件|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
                    if (vm != null)
                    {
                        vm.AddWallpaper(new WallpaperItemModel() { path = fileName, position = WallpaperStyle.Fill });
                    }
                }
            }
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            RegisterHotKeys();
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Hide();
        }

        private void BtnAddFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (lbWallpaper.SelectedItems.Count > 0)
            {
                WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
                if (vm != null)
                {
                    foreach (var item in lbWallpaper.SelectedItems)
                    {
                        WallpaperItemModel temp = (WallpaperItemModel)item;
                        vm.RemoveWallpaper(temp.path);
                    }
                }
            }
        }

        private void BtnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
            if (vm != null)
            {
                vm.ClearAllWallpaper();
            }
        }
        private void OnNextWallpaper()
        {
            WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
            if (vm != null)
            {
                vm.NextWallpaper();
            }
        }
        private void OnPreviewWallpaper()
        {
            WallpaperViewModel vm = (WallpaperViewModel)this.DataContext;
            if (vm != null)
            {
                vm.PreviewWallpaper();
            }
        }
        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void LbWallpaper_Drop(object sender, System.Windows.DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
            foreach (var ph in paths)
            {
                ((WallpaperViewModel)this.DataContext).AddWallpaper(new WallpaperItemModel { path = ph, position = WallpaperStyle.Fit });
            }
        }
    }
}
