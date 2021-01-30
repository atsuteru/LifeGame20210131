using Splat;
using System;
using System.Windows;
using WpfApp.Models;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            await Locator.Current.GetService<ILifeGameController>().InitializeAsync(columns: 10, rows: 10);
        }
    }
}
