using NHunspell;
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

namespace WpfApplication11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic"))
            {
                var input = txtInput.Text;
                List<string> suggestions = hunspell.Suggest(input);
                var sb = new StringBuilder();
                foreach (string suggestion in suggestions)
                {
                    sb.AppendLine(suggestion);
                }
                this.txtOutput.Text = sb.ToString();
            }
        }
    }
}
