using System;
using System.Collections.Generic;
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

using ClassGenerator.TargetDatas;

using CodeGenerator.ViewModels;

namespace CodeGenerator.Views
{
    /// <summary>
    /// EditorMainView.xaml の相互作用ロジック
    /// </summary>
    public partial class EditorMainView : UserControl
    {
        private ScaleTransform _ScaleTransform = new ScaleTransform(1, 1);

        private int _StageWidth = 13;
        private int _StageHeight = 12;

        public EditorMainView()
        {
            InitializeComponent();

            DataContextChanged += ChangeDataContext;

        }

        private void ChangeDataContext(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is StageDataViewModel)
            {
                var vm = e.NewValue as StageDataViewModel;
                vm.OnStageScaledChnage.Subscribe(_ => {

                    _StageWidth = vm.Width;
                    _StageHeight = vm.Height;
                    BuildView();
                });
            }

            //Palette.SelectedItem
        }

        private void BuildView() {
            int stageWidth = _StageWidth;
            int stageHeight = _StageHeight;

            StageGrid.Children.Clear();

            float strokeThickness = 1;
            float rectWidth = (float)StageGrid.ActualWidth / stageWidth;
            float rectHeight = (float)StageGrid.ActualHeight / stageHeight;

            var size = Math.Min(rectWidth, rectHeight);
            var radius = 5.0f;

            for (int i = 0; i < stageWidth; i++) {
                Path path = new Path() {
                    Data = new LineGeometry(new Point(i * size, 0), new Point(size * i, size * stageHeight)),
                    Stroke = new SolidColorBrush(Colors.DarkGray),
                    StrokeThickness = strokeThickness
                };
                path.Data.Transform = _ScaleTransform;

                StageGrid.Children.Add(path);
            }

            for (int i = 0; i < stageHeight; i++)
            {
                Path path = new Path()
                {
                    Data = new LineGeometry(new Point(0, i * size), new Point(size * stageWidth, size * i)),
                    Stroke = new SolidColorBrush(Colors.DarkGray),
                    StrokeThickness = strokeThickness
                };
                path.Data.Transform = _ScaleTransform;

                StageGrid.Children.Add(path);
            }


            float rectSize = size - strokeThickness * 2;

            var vm = DataContext as StageDataViewModel;
            if (vm == null) {
                return;
            }

            while (stageHeight - vm.CellData.Count > 0) {
                var holiList = new List<CellViewModel>();
                vm.CellData.Add(holiList);
            }

            while (vm.CellData.Count - stageHeight > 0)
            {
                vm.CellData.RemoveAt(vm.CellData.Count - 1);
            }

            for (int i = 0; i < vm.CellData.Count; i++) {
                while (stageWidth - vm.CellData[i].Count > 0)
                {
                    var cellViewModel = new CellViewModel(CellType.None,vm);
                    vm.CellData[i].Add(cellViewModel);
                }

                while (vm.CellData[i].Count - stageWidth > 0)
                {
                    vm.CellData[i].RemoveAt(vm.CellData[i].Count - 1);
                }
            }


            for (int i = 0; i < stageWidth; i++) {
                for (int j = 0; j < stageHeight; j++)
                {
                    Path path = new Path()
                    {
                        Data = new RectangleGeometry(new Rect(i* size + strokeThickness/2, j * size + strokeThickness/2,size,size)),
                        Fill = new SolidColorBrush(Colors.DarkGray),
                    };
                    path.Data.Transform = _ScaleTransform;

                    var cellViewModel = vm.CellData[j][i];

                    path.InputBindings.Add(new InputBinding(cellViewModel.UpdateCellCommand, new MouseGesture(MouseAction.LeftClick)));
                    path.DataContext = cellViewModel;
                    path.SetBinding(Path.FillProperty, new Binding("Brush"));

                    StageGrid.Children.Add(path);

                    //var rect = new Rectangle();

                    //rect.Width = size;
                    //rect.Height = size;

                    //rect.RadiusX = radius;
                    //rect.RadiusY = radius;

                    //var color = Colors.Black;
                    //rect.Fill = new SolidColorBrush(color);

                    //Canvas.SetLeft(rect, size * i +1);
                    //Canvas.SetTop(rect, size * j +1);

                    //StageGrid.Children.Add(rect);
                }
            }

            
        }


        private void AddCell() { 
        }
        private void RemoveCell()
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void StageGrid_Loaded(object sender, RoutedEventArgs e)
        {
            BuildView();
        }

        private void StageGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BuildView();
        }
    }
}
