using System;
using System.Windows.Media;
using Livet.Commands;

using ClassGenerator.TargetDatas;


namespace CodeGenerator.ViewModels
{
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
            _Brush = new SolidColorBrush(Colors.Transparent);
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

}
