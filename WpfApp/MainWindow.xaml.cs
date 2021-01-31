using Splat;
using System;
using System.Collections.Generic;
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

            await Locator.Current.GetService<ILifeGameController>().InitializeAsync(columns: 40, rows: 40);

            var aliveCells = new List<Cell>();
            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                aliveCells.Add(new Cell(random.Next(1, 40), random.Next(1, 40)));
            }

            Locator.Current.GetService<ILifeGameController>().Start(
                100.0,
                aliveCells.ToArray()
                );
        }
    }
}
