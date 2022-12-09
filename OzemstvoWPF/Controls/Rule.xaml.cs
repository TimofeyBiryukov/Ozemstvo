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
    /// Interaction logic for Rule.xaml
    /// </summary>
    public partial class Rule : UserControl
    {
        public static readonly DependencyProperty RuleNameProperty =
            DependencyProperty.Register("RuleName", typeof(string), typeof(Rule),
                new PropertyMetadata("New Rule"));

        public string RuleName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public Rule()
        {
            InitializeComponent();
        }
    }
}
