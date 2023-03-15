﻿using OzemstvoConsole;
using OzemstvoWPF.Models;
using System;
using System.Windows;
using System.Windows.Controls;

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

        public string[] Types { get; set; } = Array.Empty<string>();

        public MatchItem()
        {
            Types = Enum.GetNames(typeof(MatchType));
            InitializeComponent();
        }
    }
}
