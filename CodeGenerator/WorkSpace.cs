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
using Livet.EventListeners;
using Livet.Commands;

using ClassGenerator.TargetDatas;
using ClassGenerator.TextTamplate;
using CodeGenerator.Models;


namespace CodeGenerator.ViewModels
{
    public class StageDataViewModel : Livet.ViewModel
    {
        public IObservable<Unit> OnStageScaledChnage => _OnStageScaledChnage;
        public List<CellPaletteData> PaletteData => _PaletteData;

        public ObservableCollection<List<CellViewModel>> CellData => _CellData;
        public int Width
        {
            get { return _Model.Width; }
            set { _Model.setStageWidth(value); }
        }
        public int Height
        {
            get { return _Model.Height; }
            set { _Model.setStageHeight(value); }
        }

        public string StageName
        {
            get { return _Model.StageName; }
            set { _Model.StageName = value; }
        }

        public StageCellData StageCellData
        {
            get { return _Model.StageCellData; }
        }

        public CellPaletteData SelectedPalette
        {
            get { return _SelectedPalette; }
            set { _SelectedPalette = value; }
        }

        private ObservableCollection<List<CellViewModel>> _CellData = new ObservableCollection<List<CellViewModel>>();
        private Subject<Unit> _OnStageScaledChnage = new Subject<Unit>();
        private List<CellPaletteData> _PaletteData = new List<CellPaletteData>() {
            new CellPaletteData(new SolidColorBrush(Colors.Red),CellType.Start),
            new CellPaletteData(new SolidColorBrush(Colors.Red),CellType.Start),
        };
        private CellPaletteData _SelectedPalette;
        private StageDataModel _Model;
        public StageDataViewModel(StageDataModel model)
        {
            _Model = model ?? throw new ArgumentNullException(nameof(model));

            var listner = new PropertyChangedEventListener(_Model) {
                (sender,e)=>{
                    if(e.PropertyName == nameof(_Model.Height)){
                        RaisePropertyChanged(nameof(Height));
                        _OnStageScaledChnage.OnNext(Unit.Default);
                    } else if(e.PropertyName == nameof(_Model.Width)){
                        RaisePropertyChanged(nameof(Width));
                        _OnStageScaledChnage.OnNext(Unit.Default);
                    }

                    if(e.PropertyName == nameof(_Model.StageName)){
                        RaisePropertyChanged(nameof(StageName));
                    }

                    if(e.PropertyName == nameof(_Model.StageCellData)){
                        RaisePropertyChanged(nameof(StageCellData));
                    }
                }
            };
            CompositeDisposable.Add(listner);

            for (int i = 0; i < _Model.Height; i++)
            {
                var holiList = new List<CellViewModel>();
                for (int j = 0; j < _Model.Width; j++)
                {
                    var cellViewModel = new CellViewModel((CellType)_Model.StageCellData._CellDatas[i,j],this);
                    foreach (var palette in _PaletteData) {
                        if(cellViewModel.CellType == palette.CellType)
                        {
                            cellViewModel.Brush = palette.Brush;
                            break;
                        }
                    }
                    holiList.Add(cellViewModel);
                }
                _CellData.Add(holiList);
            }

            //StageTypeCollection = ViewModelHelper.CreateReadOnlyDispatcherCollection<PaletteModel, PaletteViewModel>(
            //    _Model.StageTypePalette,
            //    (stageType) => {
            //        int type = (int)stageType.StageTypePalette;
            //        return new PaletteViewModel();
            //    },
            //    DispatcherHelper.UIDispatcher
            //    );
        }

        #region Command

        private ViewModelCommand _ExportDataCommand;
        public ViewModelCommand ExportDataCommand
        {
            get {
                if (_ExportDataCommand == null) {
                    _ExportDataCommand = new ViewModelCommand(ExportCommand, CanExportStageData);
                }
                return _ExportDataCommand;
            }
        }
        private void ExportCommand() {
            SampleClassTemplate exportTemplate = new SampleClassTemplate();

            _Model.resetStageData();
            for (int i = 0; i < _Model.Height; i++)
            {
                for (int j = 0; j < _Model.Width; j++)
                {
                    _Model.changeStageData(j,i,(int)_CellData[i][j].CellType);
                }
            }
            exportTemplate.setData(_Model.StageCellData._CellDatas);
            exportTemplate.ExportDataClass("../StageData",_Model.StageName);
        }
        private bool CanExportStageData()
        {
            return true;
        }

        #endregion
    }

    public class CellViewModel : Livet.ViewModel {
        public ViewModelCommand UpdateCellCommand
        {
            get {
                if (_UpdateCellCommand == null) {
                    _UpdateCellCommand = new ViewModelCommand(updateState);
                }
                return _UpdateCellCommand;
            }
        }
        public SolidColorBrush Brush {
            get { return _Brush; }
            set { 
                _Brush = value;
                RaisePropertyChanged();
            }
        }
        public CellType CellType
        {
            get { return _CellType; }
        }

        private StageDataViewModel _StageDataViewModel;
        private SolidColorBrush _Brush;
        private CellType _CellType;
        private ViewModelCommand _UpdateCellCommand;

        public CellViewModel(CellType type, StageDataViewModel stageDataViewModel) {
            _CellType = type;
            _StageDataViewModel = stageDataViewModel ?? throw new ArgumentNullException(nameof(stageDataViewModel));
        }

        public void updateState() {
            if (_StageDataViewModel.SelectedPalette == null) {
                return;
            }

            _Brush = _StageDataViewModel.SelectedPalette.Brush;
            _CellType = _StageDataViewModel.SelectedPalette.CellType;
            RaisePropertyChanged(nameof(Brush));
        }

    }


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
    namespace CodeGenerator.Models
    {


        public class StageDataModel : NotificationObject
        {
            private static readonly int MAX_SCALE = 30;
            private static readonly int MIN_SCALE = 1;

            public int Width => _Width;
            public int Height => _Height;
            public ClassGenerator.TargetDatas.StageCellData StageCellData => _StageCellData;
            public string StageName
            {
                get { return _StageName; }
                set {
                    if (!string.IsNullOrEmpty(value)) {
                        _StageName = value;
                        RaisePropertyChanged();
                    }
                }
            }

            private int _Width;
            private int _Height;
            private ClassGenerator.TargetDatas.StageCellData _StageCellData;
            private string _StageName;

            public StageDataModel() {
            //初期データの設定頑張って
            _StageCellData = new ClassGenerator.TargetDatas.StageCellData() { 
                _CellDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
                _FloorDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
                _DirDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
            };

                _Width = _StageCellData._CellDatas.GetLength(1);
                _Height = _StageCellData._CellDatas.GetLength(0);
            }

            public void setStageWidth(int width) {
                if (width < MIN_SCALE) {
                    return;
                }
                if (width > MAX_SCALE) {
                    return;
                }

                _Width = width;
                RaisePropertyChanged(nameof(Width));
            }

            public void setStageHeight(int height)
            {
                if (height < MIN_SCALE)
                {
                    return;
                }
                if (height > MAX_SCALE)
                {
                    return;
                }

                _Height = height;
                RaisePropertyChanged(nameof(Height));
            }

            public void resetStageData() {
                _StageCellData._CellDatas = new int[Height, Width];
                _StageCellData._FloorDatas = new int[Height, Width];
                _StageCellData._DirDatas = new int[Height, Width];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        _StageCellData._FloorDatas[i, j] = 1;
                        _StageCellData._DirDatas[i, j] = 2;
                    }
                }
            }

            public void changeStageData(int x, int y, int data)
            {
                if (x < 0 || _StageCellData._CellDatas.GetLength(1) <= x) {
                    return;
                }
                if (y < 0 || _StageCellData._CellDatas.GetLength(0) <= y)
                {
                    return;
                }
                _StageCellData._CellDatas[y, x] = data;
                RaisePropertyChanged(nameof(StageCellData));
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
