using System.Windows;

namespace Lockdown
{
    public class MainWindowViewModel
    {
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public MainWindowViewModel(Window window)
        {
            WindowHeight = SystemParameters.PrimaryScreenHeight;
            WindowWidth = SystemParameters.PrimaryScreenWidth;
        }
    }
}
