using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Helper;

namespace Lockdown
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool isHook = false;
        public static bool isLock = false;
        KeyHelper KH = new KeyHelper();
        public static MouseHelper MH = new MouseHelper();
        public static int width, height;
        bool ctrl, shift, willActivate;
        public static IntPtr hookId;
        MainWindow main;
        VideoWindow video;
        string pin;
        protected override void OnStartup(StartupEventArgs e)
        {
            var bounds = System.Windows.Forms.Screen.AllScreens[0].Bounds;
            width = bounds.Width;
            height = bounds.Height;
            KH.KeyDown += KH_KeyDown;
            KH.KeyUp += KH_KeyUp;
            main = new MainWindow();
            video = new VideoWindow();
        }

        private void KH_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.LControlKey || e.KeyCode == System.Windows.Forms.Keys.RControlKey) ctrl = false;
            if (e.KeyCode == System.Windows.Forms.Keys.LShiftKey || e.KeyCode == System.Windows.Forms.Keys.RShiftKey) shift = false;
            if(willActivate && ((e.KeyCode >= System.Windows.Forms.Keys.NumPad0 && e.KeyCode <= System.Windows.Forms.Keys.NumPad9) ||
                (e.KeyCode >= System.Windows.Forms.Keys.D0 && e.KeyCode <= System.Windows.Forms.Keys.D9))){
                pin += e.KeyCode.ToString().Last();
                if (pin.Length == 1) main.B1.Visibility = Visibility.Visible;
                if (pin.Length == 2) main.B2.Visibility = Visibility.Visible;
                if (pin.Length == 3) main.B3.Visibility = Visibility.Visible;
                if (pin.Length == 4) main.B4.Visibility = Visibility.Visible;
                if(pin.Length == 4 && pin == "1234")
                {
                    pin = "";
                    if (pin.Length == 1) main.B1.Visibility = Visibility.Hidden;
                    if (pin.Length == 2) main.B2.Visibility = Visibility.Hidden;
                    if (pin.Length == 3) main.B3.Visibility = Visibility.Hidden;
                    if (pin.Length == 4) main.B4.Visibility = Visibility.Hidden;
                    main.Hide();
                    isHook = isLock;
                    if (isLock)
                    {
                        hookId = MH.MouseHook(width, height);
                        Process.GetProcessesByName("explorer").ToList().ForEach(x => x.Kill());
                        video = new VideoWindow();
                        video.Show();
                    }
                    else
                    {
                        Process.GetProcessesByName("sihost").ToList().ForEach(x => x.Kill());
                        video.Close();
                    }
                    willActivate = false;
                }
                if(pin.Length == 4 && pin != "1234")
                {
                    pin = "";
                    main.Hide();
                    isLock = !isLock;
                    if (isLock) hookId = MH.MouseHook(width, height);
                    willActivate = false;
                }
            }
            else if(willActivate && e.KeyCode == System.Windows.Forms.Keys.Back)
            {
                if(pin.Length == 1)
                {
                    main.B1.Visibility = Visibility.Hidden;
                    pin = "";
                }
                if(pin.Length == 2)
                {
                    main.B2.Visibility = Visibility.Hidden;
                    pin = pin.Substring(0, 1);
                }
                if (pin.Length == 3)
                {
                    main.B3.Visibility = Visibility.Hidden;
                    pin = pin.Substring(0, 2);
                }
                if (pin.Length == 4)
                {
                    main.B4.Visibility = Visibility.Hidden;
                    pin = pin.Substring(0, 3);
                }
            }
            else
            {
                pin = "";
                main.B1.Visibility = Visibility.Hidden;
                main.B2.Visibility = Visibility.Hidden;
                main.B3.Visibility = Visibility.Hidden;
                main.B4.Visibility = Visibility.Hidden;
            }
        }

        private void KH_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.LControlKey || e.KeyCode == System.Windows.Forms.Keys.RControlKey) ctrl = true;
            if (e.KeyCode == System.Windows.Forms.Keys.LShiftKey || e.KeyCode == System.Windows.Forms.Keys.RShiftKey) shift = true;
            if(e.KeyCode == System.Windows.Forms.Keys.L && ctrl && shift)
            {
                if (!isLock)
                {
                    willActivate = true;
                    isLock = true;
                    main.Show();
                }
            }
            if(e.KeyCode == System.Windows.Forms.Keys.U && ctrl && shift)
            {
                if (isLock)
                {
                    MH.UnHook(hookId);
                    willActivate = true;
                    isLock = false;
                    main.Show();
                }
            }
            e.Handled = isHook;
        }
    }
}
