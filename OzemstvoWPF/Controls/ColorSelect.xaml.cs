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

        static public string[] ColorVariants { get; set; } = new string[]
        {
            "#151F30", // Dark Blue
            "#103778", // Blue
            "#0593A2", // Light Blue
            "#FF7A48", // Orange
            "#E3371E" // Red
        };

        public ColorSelect()
        {
            InitializeComponent();
        }
    }
}
