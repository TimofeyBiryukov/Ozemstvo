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

namespace OzemstvoWPF.Controls
{
    /// <summary>
    /// Interaction logic for RuleItem.xaml
    /// </summary>
    public partial class RuleItem : UserControl
    {
        public static readonly DependencyProperty RuleNameProperty =
            DependencyProperty.Register("RuleName", typeof(string), typeof(RuleItem),
                new PropertyMetadata("New Rule"));

        public string RuleName
        {
            get { return (string)GetValue(RuleNameProperty); }
            set { SetValue(RuleNameProperty, value); }
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
    }
}
