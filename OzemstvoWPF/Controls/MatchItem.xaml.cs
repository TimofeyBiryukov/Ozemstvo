using OzemstvoConsole;
using OzemstvoWPF.Models;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OzemstvoWPF.Controls
{
    /// <summary>
    /// Interaction logic for MatchItem.xaml
    /// </summary>
    public partial class MatchItem : UserControl
    {
        public static readonly DependencyProperty MatchDependecy =
            DependencyProperty.Register("Match", typeof(MatchProperty), typeof(MatchItem),
                new PropertyMetadata());

        public MatchProperty Match
        {
            get { return (MatchProperty)GetValue(MatchDependecy); }
            set { SetValue(MatchDependecy, value); }
        }

        public static readonly RoutedEvent OnRemoveEvent =
            EventManager.RegisterRoutedEvent("OnRemove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MatchItem));

        public event RoutedEventHandler Remove
        {
            add { AddHandler(OnRemoveEvent, value); }
            remove { RemoveHandler(OnRemoveEvent, value); }
        }

        public string[] Types { get; set; } = Array.Empty<string>();

        public MatchItem()
        {
            Types = Enum.GetNames(typeof(MatchType));
            InitializeComponent();
            //var random = new Random();
            //var number = random.Next(1, 6);
            //switch (number)
            //{
            //    case 1:
            //        border.Background = new SolidColorBrush(FromHex("#151F30"));
            //        break;
            //    case 2:
            //        border.Background = new SolidColorBrush(FromHex("#103778"));
            //        break;
            //    case 3:
            //        border.Background = new SolidColorBrush(FromHex("#0593A2"));
            //        break;
            //    case 4:
            //        border.Background = new SolidColorBrush(FromHex("#FF7A48"));
            //        break;
            //    case 5:
            //        border.Background = new SolidColorBrush(FromHex("#E3371E"));
            //        break;
            //}
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnRemoveEvent));
        }
    }
}
