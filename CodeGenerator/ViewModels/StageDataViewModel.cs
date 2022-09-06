using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Reactive;
using System.Reactive.Subjects;
using System.Collections.ObjectModel;
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
            new CellPaletteData(new SolidColorBrush(Color.FromArgb(255,15,185,20)),CellType.Goal),
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

}
