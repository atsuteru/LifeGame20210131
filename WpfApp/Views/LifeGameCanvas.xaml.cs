using ReactiveUI;
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

            for (int x = 0; x < 10; x++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(10.0, GridUnitType.Pixel),
                });
            }
            for (int y = 0; y < 10; y++)
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
