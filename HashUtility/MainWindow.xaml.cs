using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Threading;

namespace HashUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected string[] files;

        public MainWindow()
        {
            InitializeComponent();
            this.cmbHashType.SelectedItem = HashType.MD5;
            this.files = ["C:/Users/SaltWood/Desktop/HashUtility.exe"];
        }

        public MainWindow(string[] args) : this()
        {
            this.files = args;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.files)
            {
                FileHashInfo i = new FileHashInfo();
                i.txtFileName.Text = item;
                this.lstItems.Items.Add(i);
            }
            Task.Run(() => this.Dispatcher.Invoke(CalculateHashAndWait));
        }

        DispatcherOperation[] StartHashCalculateTask()
        {
            DispatcherOperation[] tasks = new DispatcherOperation[this.lstItems.Items.Count];
            this.Dispatcher.Invoke(() =>
            {
                foreach (FileHashInfo item in this.lstItems.Items)
                {
                    item.cmbHashType.SelectedItem = this.cmbHashType.SelectedItem;
                }
            });
            for (int i = 0; i < this.lstItems.Items.Count; i++)
            {
                FileHashInfo info = (FileHashInfo)this.lstItems.Items[i];
                this.Dispatcher.Invoke(() => info.txtHashText.Text = "哈希计算中……");
                tasks[i] = this.Dispatcher.BeginInvoke(() =>
                {
                    Stream stream = File.OpenRead(info.txtFileName.Text);
                    byte[] data = Array.Empty<byte>();
                    switch (info.cmbHashType.SelectedItem)
                    {
                        case HashType.MD5:
                            data = MD5.HashData(stream);
                            break;
                        case HashType.SHA1:
                            data = SHA1.HashData(stream);
                            break;
                        case HashType.SHA256:
                            data = SHA256.HashData(stream);
                            break;
                        case HashType.SHA384:
                            data = SHA384.HashData(stream);
                            break;
                        case HashType.SHA512:
                            data = SHA512.HashData(stream);
                            break;
                        default:
                            break;
                    }
                    info.txtHashText.Text = Convert.ToHexString(data);
                });
            }
            return tasks;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.CalculateHashAndWait();
        }

        public void CalculateHashAndWait()
        {
            this.cmbHashType.IsEnabled = false;
            foreach (var item in StartHashCalculateTask())
            {
                item.Wait();
            }
            this.cmbHashType.IsEnabled = true;
        }
    }
}