using CodeGenerator.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {

        public Livet.Commands.ViewModelCommand OnClosedWindowCommand
        {
            get {
                if (_OnClosedWindowCommand == null) {
                    _OnClosedWindowCommand = new ViewModelCommand(Dispose);
                }
                return _OnClosedWindowCommand;
            }
        }
        public StageDataViewModel StageDataViewModel => _StageDataViewModel;

        private Livet.Commands.ViewModelCommand _OnClosedWindowCommand;
        private StageDataViewModel _StageDataViewModel;
        private StageDataModel _StageDataModel;

        public MainWindowViewModel() {
            Initialize();
        }

        public void Initialize()
        {
            bool isDesignMode = (bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue;
            if (isDesignMode) {
                return;
            }

            _StageDataModel = new StageDataModel();
            _StageDataViewModel = new StageDataViewModel(_StageDataModel);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
