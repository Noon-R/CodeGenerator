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

using CodeGenerator.ViewModels;

namespace CodeGenerator.Views
{
    /// <summary>
    /// EditorMainView.xaml の相互作用ロジック
    /// </summary>
    public partial class EditorMainView : UserControl
    {
        public EditorMainView()
        {
            InitializeComponent();

            DataContextChanged += ChangeDataContext;

        }

        private void ChangeDataContext(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is StageEditorViewModel)
            {

                DrawCellGrid();
            }

            //Palette.SelectedItem
        }

        private void DrawCellGrid() {
            int stageWidth = 10;
            int stageHeight = 10;

            StageGrid.Children.Clear();

            float rectWidth = (float)StageGrid.ActualWidth / stageWidth;
            float rectHeight = (float)StageGrid.ActualHeight / stageHeight;

            var size = Math.Min(rectWidth, rectHeight);
            var radius = 5.0f;
            for (int i = 0; i < stageWidth; i++) {
                for (int j = 0; j < stageHeight; j++)
                {
                    var rect = new Rectangle();

                    rect.Width = size;
                    rect.Height = size;

                    rect.RadiusX = radius;
                    rect.RadiusY = radius;

                    var color = Colors.Black;
                    rect.Fill = new SolidColorBrush(color);

                    Canvas.SetLeft(rect, size * i +1);
                    Canvas.SetTop(rect, size * j +1);

                    StageGrid.Children.Add(rect);
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


    }
}
