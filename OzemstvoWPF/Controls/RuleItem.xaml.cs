using System.Windows;
using System.Windows.Controls;
using OzemstvoWPF.Models;
using static OzemstvoWPF.MainWindow;

namespace OzemstvoWPF.Controls
{
    /// <summary>
    /// Interaction logic for RuleItem.xaml
    /// </summary>
    public partial class RuleItem : UserControl
    {
        public static readonly DependencyProperty RuleDependecy =
            DependencyProperty.Register("Rule", typeof(RuleProperty), typeof(RuleItem),
                new PropertyMetadata());

        public RuleProperty Rule
        {
            get { return (RuleProperty)GetValue(RuleDependecy); }
            set { SetValue(RuleDependecy, value); }
        }

        public static readonly RoutedEvent OnRemoveEvent =
            EventManager.RegisterRoutedEvent("OnRemove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RuleItem));

        public event RoutedEventHandler Remove
        { 
            add { AddHandler(OnRemoveEvent, value);  }
            remove { RemoveHandler(OnRemoveEvent, value); }
        }

        public static readonly RoutedEvent OnEditEvent =
            EventManager.RegisterRoutedEvent("OnEdit", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RuleItem));

        public event RoutedEventHandler Edit
        {
            add { AddHandler(OnEditEvent, value); }
            remove { RemoveHandler(OnEditEvent, value); }
        }

        public RuleItem()
        {
            InitializeComponent();
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnRemoveEvent));
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnEditEvent));
        }
    }
}
