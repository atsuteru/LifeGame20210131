using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views
{
    /// <summary>
    /// LifeGameCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class LifeGameCanvas : ReactiveUserControl<LifeGameCanvasViewModel>
    {
        public LifeGameCanvas()
        {
            InitializeComponent();

            DataContext = new LifeGameCanvasViewModel();

            this.WhenActivated(d =>
            {
                ViewModel = DataContext as LifeGameCanvasViewModel;

                ViewModel.Cells?
                    .ToObservableChangeSet()
                    .ToCollection()
                    .Subscribe(cells =>
                    {
                        InitializeCanvas(
                            cells.Max(cell => cell.PositionX),
                            cells.Max(cell => cell.PositionY));

                    });
            });
        }

        private void InitializeCanvas(int columns, int rows)
        {
            Grid.ColumnDefinitions.Clear();
            for (int x = 0; x < columns; x++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(10.0, GridUnitType.Pixel),
                });
            }

            Grid.RowDefinitions.Clear();
            for (int y = 0; y < rows; y++)
            {
                Grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(10.0, GridUnitType.Pixel),
                });
            }
            for (int y = 0; y < Grid.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < Grid.ColumnDefinitions.Count; x++)
                {
                    var cellVM = new CellPanelViewModel()
                    {
                        PositionX = x,
                        PositionY = y,
                    };
                    var cell = new CellPanel();
                    cell.DataContext = cellVM;
                    Grid.Children.Add(cell);
                }
            }
        }
    }
}
