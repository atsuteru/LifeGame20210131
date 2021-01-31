using Splat;
using System;
using System.Threading.Tasks;
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

            Locator.Current.GetService<ILifeGameController>().Start(
                1.0,
                new Cell(5, 5) { IsAlive = true },
                new Cell(5, 6) { IsAlive = true },
                new Cell(5, 7) { IsAlive = true },
                new Cell(6, 5) { IsAlive = true },
                new Cell(6, 6) { IsAlive = true },
                new Cell(6, 7) { IsAlive = true },
                new Cell(7, 5) { IsAlive = true },
                new Cell(7, 6) { IsAlive = true },
                new Cell(7, 7) { IsAlive = true }
                );
        }
    }
}
