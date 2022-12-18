using OzemstvoConsole;
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
            new EditorWindow(Rule).ShowDialog();
        }
    }
}
