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

namespace SharpBattles
{
    /// <summary>
    /// Interaction logic for CharacterSelect.xaml
    /// </summary>
    public partial class CharacterSelect : Page
    {
        string p1;
        string p2;

        public CharacterSelect()
        {
            InitializeComponent();
            p1 = "Swordsman";
            p2 = "Slime";
        }

        private void _player1Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (p1 == null) return;
            p1 = _player1Select.SelectedItem.ToString();
            p1 = p1.Substring(38, p1.Length - 38);
            _vsText.Text = p1 + " vs. " + p2;
            _player1.Source = new BitmapImage(new Uri(@"Images/" + p1 + ".png", UriKind.Relative));
        }

        private void _player2Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (p2 == null) return;
            p2 = _player2Select.SelectedItem.ToString();
            p2 = p2.Substring(38, p2.Length - 38);
            _vsText.Text = p1 + " vs. " + p2;
            _player2.Source = new BitmapImage(new Uri(@"Images/" + p2 + ".png", UriKind.Relative));
        }

        private void _startBattle_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Battle(p1, p2));
        }
    }
}
