using AbyssLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AbyssConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RoomSubPage RoomView { get; private set; }
        public HomeSubPage HomeView { get; private set; }
        public ClockSubPage TimeView { get; private set; }
        public HintSubPage HintView { get; private set; }

        bool isExiting;

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += OnWindowClosing;

            this.HomeView = new HomeSubPage();
            this.RoomView = new RoomSubPage();
            this.TimeView = new ClockSubPage();
            this.HintView = new HintSubPage();

            SwapToHomeView();
        }

        public void Refresh()
        {
            if (isExiting)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                RoomView.Refresh();
                HomeView.Refresh();
            });
        }

        public void SwapToHomeView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(HomeView);
        }

        public void SwapToRoomView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(RoomView);
        }

        public void SwapToTimeView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(TimeView);
        }

        public void SwapToHintView()
        {
            this.SubViewGrid.Children.Clear();
            this.SubViewGrid.Children.Add(HintView);
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App app = (App)Application.Current;
            app.Controller.ExitProgram();

            isExiting = true;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SwapToHomeView();
        }
    }
}
