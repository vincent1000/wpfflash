using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NamePipeClient? _pipeService;
        private DispatcherTimer _hideTimer;
        private const int _tmDuration = 5;
        public MainWindow()
        {
            InitializeComponent();
            _pipeService = new NamePipeClient();
            _pipeService.DataReceived += OnPipeDataReceived;
            _hideTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(_tmDuration),
                IsEnabled = false
            };
            _hideTimer.Tick += HideTimer_Tick;
            _ = _pipeService.StartListening();
            this.Hide();
        }
        private void HideTimer_Tick(object? sender, EventArgs e)
        {
            HideWindow();
        }
        private void HideWindow()
        {
            _hideTimer.Stop();
            this.Hide();
        }
        private void ShowWindow()
        {

            _hideTimer.Start();
            this.Show();
        }
        private void OnPipeDataReceived(char[] data, int len)
        {
            var cmdType = data[0];

            if (cmdType == 0x02)
            {
                bool mode = false;
                if (len > 1)
                {
                    mode = data[1] == 0x03;
                }
                Helper.OnHighContrastChanged(mode);
                ShowWindow();
            }
        }
    }
}