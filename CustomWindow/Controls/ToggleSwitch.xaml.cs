using System;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindow.Controls
{
    /// <summary>
    /// Логика взаимодействия для ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
    
        #region Dependency Properties

        #region Header

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                "Header",
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnHeaderChanged));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #region OffContent

        public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(
            "OffContent",
            typeof(string),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                "Off",
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender,
                OffContentPropertyChanged));

        public string OffContent
        {
            get => (string)GetValue(OffContentProperty);
            set => SetValue(OffContentProperty, value);
        }

        private static void OffContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {

        }

        #endregion

        #region OnContent

        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(
            "OnContent",
            typeof(string),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                "On",
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnContentPropertyChanged));


        public string OnContent
        {
            get => (string)GetValue(OnContentProperty);
            set => SetValue(OnContentProperty, value);
        }

        private static void OnContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {

        }

        #endregion

        #region IsOn

        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
            "IsOn",
            typeof(bool),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender,
                IsOnPropertyChanged));

        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        private static void IsOnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            
        }

        #endregion

        #endregion

        #region Routed Events

        public static readonly RoutedEvent ToggledEvent = EventManager.RegisterRoutedEvent("Toggled",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleSwitch));

        public event RoutedEventHandler Toggled
        {
            add => AddHandler(ToggledEvent, value);
            remove => RemoveHandler(ToggledEvent, value);
        }

        #endregion


        public ToggleSwitch()
        {
            DataContext = this;
            InitializeComponent();
        }


        #region Methods



        #endregion

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            IsOn = ToggleButton.IsChecked == true;

            RaiseEvent(new RoutedEventArgs(ToggledEvent));
        }
    }
}
