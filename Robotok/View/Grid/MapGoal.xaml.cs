using Robotok.MVVM;
using Robotok.ViewModel;
using RobotokModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Robotok.View.Grid
{
    /// <summary>
    /// Interaction logic for MapGoal.xaml
    /// </summary>
    public partial class MapGoal : UserControl
    {
        public double Zoom { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public ObservableCollectionWrapper<Goal> ObservableGoals { get; set; }

        public MapGoal()
        {
            DataContext = this;
            ObservableGoals = new ([]);
            InitializeComponent();

        }

        public void SetDataContext(MainWindowViewModel viewModel)
        {
            this.DataContext = viewModel;
            viewModel.GoalsChanged += new EventHandler(RefreshGoals);
        }

        private void RefreshGoals(object? sender, EventArgs e)
        {
            if(sender == null)
                return;
            List<Goal> goals = (List<Goal>)sender;

            MapCanvas.Children.Clear();
            foreach(Goal goal in goals)
            {
                if(!goal.IsAssigned)
                    continue;
                System.Windows.Controls.Grid grid = new()
                {
                    Width = GridConverterFunctions.unit,
                    Height = GridConverterFunctions.unit,
                    ToolTip = goal.Id,
                    Margin = new Thickness(
                        GridConverterFunctions.unit * goal.Position.X,
                        GridConverterFunctions.unit * goal.Position.Y,
                        0,
                        0)
                };
                ToolTipService.SetInitialShowDelay(grid, 0);
                ToolTipService.SetShowDuration(grid, 9999999);
                ToolTipService.SetBetweenShowDelay(grid, 0);

                System.Windows.Shapes.Rectangle rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb(251, 171, 9)),
                    Margin = new Thickness(0.5)
                };

                System.Windows.Controls.TextBlock textBlock = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12,
                    Text = goal.Id.ToString()
                };

                grid.Children.Add(rectangle);
                grid.Children.Add(textBlock);
                MapCanvas.Children.Add(grid);

            }
        }
    }
}
