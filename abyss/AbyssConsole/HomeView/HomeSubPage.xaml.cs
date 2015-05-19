using AbyssLibrary;
using AbyssScreen;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RoomView.xaml
    /// </summary>
    public partial class HomeSubPage : UserControl
    {
        RoomThumbUserControl roomThumb;
        TimeThumbUserControl timeThumb;
        HintThumbUserControl hintThumb;

        public HomeSubPage()
        {
            InitializeComponent();

            roomThumb = new RoomThumbUserControl();
            roomThumb.MouseDown += Room_Click;
            this.WrapPanel.Children.Add(roomThumb);

            timeThumb = new TimeThumbUserControl();
            timeThumb.MouseDown += Time_Click;
            this.WrapPanel.Children.Add(timeThumb);

            hintThumb = new HintThumbUserControl();
            hintThumb.MouseDown += Hint_Click;
            this.WrapPanel.Children.Add(hintThumb);
        }

        public void AddClock(CountDownTimer timer)
        {
            timeThumb.AddClock(timer);
        }

        public void Refresh()
        {
            roomThumb.Refresh();
            timeThumb.Refresh();
            hintThumb.Refresh();
        }

        private void Time_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToTimeView();
        }

        private void Room_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToRoomView();
        }

        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.RootWindow.SwapToHintView();
        }
    }
}
