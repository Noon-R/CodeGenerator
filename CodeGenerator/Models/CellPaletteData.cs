using System;
using System.Windows.Media;

using ClassGenerator.TargetDatas;


namespace CodeGenerator.ViewModels
{
    public class CellPaletteData :Livet.ViewModel
    {
        public SolidColorBrush Brush => _Brush;
        public CellType CellType
        {
            get { return _CellType; }
            set { }
        }

        private SolidColorBrush _Brush;
        private CellType _CellType;

        public CellPaletteData(SolidColorBrush brush, CellType cellType)
        {
            _Brush = brush ?? throw new ArgumentNullException(nameof(brush));
            _CellType = cellType;
        }
    }

}
