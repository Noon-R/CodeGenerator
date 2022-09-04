using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Reactive;
using System.Reactive.Subjects;
using System.Collections.ObjectModel;
using Livet;

using ClassGenerator.TargetDatas;

using CodeGenerator.Models;

using app;

namespace CodeGenerator.ViewModels
{
    public class StageEditorViewModel : Livet.ViewModel
    {

        private Subject<Unit> Subject = new Subject<Unit>();
        public ReadOnlyDispatcherCollection<PaletteViewModel> StageTypeCollection;
        PaletteModel _Model;
        public StageEditorViewModel(PaletteModel model)
        {
            //StageTypeCollection = ViewModelHelper.CreateReadOnlyDispatcherCollection<PaletteModel, PaletteViewModel>(
            //    _Model.StageTypePalette,
            //    (stageType) => {
            //        int type = (int)stageType.StageTypePalette;
            //        return new PaletteViewModel();
            //    },
            //    DispatcherHelper.UIDispatcher
            //    );
        }
    }

    public class PaletteViewModel : Livet.ViewModel
    {
        public ColorSet ColorSet => _ColorSet;
        private ColorSet _ColorSet;

        public PaletteViewModel(SolidColorBrush brush, int type)
        {
            _ColorSet = new ColorSet(brush,type);
        }
    }
}

namespace CodeGenerator.Models
{
    public class PaletteModel: NotificationObject {
        public ObservableCollection<CellType> StageTypePalette => _StageTypePalette;
        public ObservableCollection<int> FloorPalette => _FloorPalette;
        public ObservableCollection<Direction> DirPalette => _DirPalette;

        private ObservableCollection<CellType> _StageTypePalette = new ObservableCollection<CellType>();
        private ObservableCollection<int> _FloorPalette = new ObservableCollection<int>();
        private ObservableCollection<Direction> _DirPalette = new ObservableCollection<Direction>();

        public PaletteModel()
        {
            for (int i = 0; i < (int)CellType.MAX; i++) {
                _StageTypePalette.Add((CellType)i);
            }

            for (int i = 0; i < (int)Direction.UNDIF; i++)
            {
                _FloorPalette.Add(i);
            }

            for (int i = 0; i < (int)Direction.UNDIF; i++)
            {
                _DirPalette.Add((Direction)i);
            }
        }
    }


}

namespace app {

    public class ColorSet {
        public SolidColorBrush Brush => _Brush;
        public int Type => _Type;

        private readonly SolidColorBrush _Brush;
        private readonly int _Type;

        public ColorSet(SolidColorBrush brush, int type)
        {
            _Brush = brush ?? throw new ArgumentNullException(nameof(brush));
            _Type = type;
        }
    }

    public abstract class CellBase
    {
        public SolidColorBrush RectFill => _RectFill;
        public Point Pos => _Pos;
        public float Size => _Size;

        private readonly SolidColorBrush _RectFill;
        private readonly Point _Pos;
        private readonly float _Size;
    }
}