using OzemstvoWPF.Config;
using System.Windows;
using System.Windows.Controls;

namespace OzemstvoWPF.Controls
{
    /// <summary>
    /// Interaction logic for ColorSelect.xaml
    /// </summary>
    public partial class ColorSelect : UserControl
    {
        public static readonly DependencyProperty ColorDependency =
            DependencyProperty.Register("Color", typeof(string), typeof(ColorSelect),
                new PropertyMetadata());

        public string Color
        {
            get { return (string)GetValue(ColorDependency); }
            set { SetValue(ColorDependency, value); }
        }

        static public string[] ColorVariants { get; set; } = ColorConfig.ColorVariants;

        public ColorSelect()
        {
            InitializeComponent();
        }
    }
}
