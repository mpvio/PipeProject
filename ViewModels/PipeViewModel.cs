using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using PipeProject.Models;
using PipeProject.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Reflection.Emit;

namespace PipeProject.ViewModels
{
    public class PipeViewModel : BindableBase //, INotifyDataErrorInfo
    {
        private readonly IPipeService _pipeService;
        private readonly IRegionManager _regionManager;
        private Pipe _currentPipe;
        

        public PipeViewModel(IPipeService pipeService, IRegionManager regionManager)
        {
            _pipeService = pipeService;
            _regionManager = regionManager;

            PipeTypes = new ObservableCollection<PipeType>(_pipeService.GetPipeTypes());
            AllPipes = new ObservableCollection<Pipe>(_pipeService.GetAllPipes());

            ResetCommand = new DelegateCommand(ResetPipe);
            ApplyCommand = new DelegateCommand(ApplyPipe);
            SummaryCommand = new DelegateCommand(ShowSummary);
            EditCommand = new DelegateCommand(EditPipe, CanEditPipe);
            DeleteCommand = new DelegateCommand(DeletePipe, CanDeletePipe);

            CurrentPipe = new Pipe { Label = "" };

            // subscribe pipe to property changes
            if (CurrentPipe != null)
            {
                CurrentPipe.PropertyChanged += CurrentPipe_PropertyChanged;
            }

            // save/ load commands
            SaveCommand = new DelegateCommand(SavePipes);
            LoadCommand = new DelegateCommand(LoadPipes);
            ExitCommand = new DelegateCommand(ExitApplication);
        }

        public Pipe CurrentPipe
        {
            get => _currentPipe;
            set
            {
                if (_currentPipe != null)
                {
                    _currentPipe.PropertyChanged -= CurrentPipe_PropertyChanged;
                }

                SetProperty(ref _currentPipe, value);

                if (_currentPipe != null)
                {
                    _currentPipe.PropertyChanged += CurrentPipe_PropertyChanged;
                }

                // Update Young's Modulus visibility when pipe changes
                RaisePropertyChanged(nameof(ShowYoungsModulus));
                RaiseCommandsCanExecuteChanged();
            }
        }

        public ObservableCollection<PipeType> PipeTypes { get; }
        public ObservableCollection<Pipe> AllPipes { get; }

        private Pipe _selectedPipe;
        public Pipe SelectedPipe
        {
            get => _selectedPipe;
            set
            {
                SetProperty(ref _selectedPipe, value);
                RaiseCommandsCanExecuteChanged();
            }
        }

        public bool ShowYoungsModulus
        {
            get
            {
                System.Diagnostics.Debug.WriteLine($"Youngs: {CurrentPipe?.PipeType}");
                return CurrentPipe?.PipeType == PipeTypeEnum.DINStandard;
            }
        }

        public DelegateCommand ResetCommand { get; }
        public DelegateCommand ApplyCommand { get; }
        public DelegateCommand SummaryCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        // save/ load
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand LoadCommand { get; }
        public DelegateCommand ExitCommand { get; }

        private void ResetPipe()
        {
            CurrentPipe = new Pipe();
            AllPipes.Clear();
            foreach (var pipe in _pipeService.GetAllPipes())
                AllPipes.Add(pipe);
        }

        private void ApplyPipe()
        {
            if (!CheckValidity()) return;
            try
            {
                if (_pipeService.LabelExists(CurrentPipe.Label))
                {
                    _pipeService.UpdatePipe(CurrentPipe);
                }
                else
                {
                    _pipeService.AddPipe(CurrentPipe);
                }

                RefreshPipeList();
                ResetPipe(); // clear form after apply
            }
            catch (Exception ex)
            {
                throw new Exception($"Error applying pipe: {ex.Message}", ex);
            }
        }

        private bool CheckValidity()
        {
            if (CurrentPipe == null) 
            {
                return false;
            }

            var errors = new List<string>();
            if (string.IsNullOrEmpty(CurrentPipe.Label) || string.Equals(CurrentPipe.Label, ""))
            {
                errors.Add("Pipe label cannot be empty.");
            }
                if (CurrentPipe.Diameter <= 0)
            {
                errors.Add("Diameter must be > 0.");
            }
            if (CurrentPipe.Length <= 0) {
                errors.Add("Length must be > 0.");
            }
            if (CurrentPipe.YoungsModulus <= 0) {
                errors.Add("Young's Modulus must be > 0.");
            }
            if (CurrentPipe.WaveSpeed <= 0 || CurrentPipe.WaveSpeed > 1500) {
                errors.Add("Wave Speed must be > 0 and <= 1500.");
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors.ToArray()));
                return false;
            }

            return true;
        }

        private void ShowSummary()
        {
            _regionManager.RequestNavigate(Infrastructure.RegionNames.MainRegion, "SummaryView");
        }

        private void EditPipe()
        {
            if (SelectedPipe != null)
            {
                CurrentPipe = new Pipe
                {
                    Label = SelectedPipe.Label,
                    PipeType = SelectedPipe.PipeType,
                    Diameter = SelectedPipe.Diameter,
                    Length = SelectedPipe.Length,
                    WaveSpeed = SelectedPipe.WaveSpeed,
                    YoungsModulus = SelectedPipe.YoungsModulus
                };
            }
        }

        private bool CanEditPipe() => SelectedPipe != null;

        private void DeletePipe()
        {
            if (SelectedPipe != null)
            {
                _pipeService.DeletePipe(SelectedPipe.Label);
                RefreshPipeList();
                ResetPipe();
            }
        }

        private bool CanDeletePipe() => SelectedPipe != null;

        private void RefreshPipeList()
        {
            AllPipes.Clear();
            foreach (var pipe in _pipeService.GetAllPipes())
                AllPipes.Add(pipe);
        }

        private void SavePipes()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    DefaultExt = "json",
                    FileName = "pipes.json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    _pipeService.SavePipesToFile(saveFileDialog.FileName);
                    MessageBox.Show($"Pipes saved successfully to {saveFileDialog.FileName}",
                                  "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving pipes: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPipes()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    DefaultExt = "json"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // confirm with user before loading (else lose unsaved changes)
                    var result = MessageBox.Show("Loading new data will replace current pipes. Continue?",
                                               "Confirm Load",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _pipeService.LoadPipesFromFile(openFileDialog.FileName);
                        RefreshPipeList();
                        ResetPipe(); // clear current form

                        MessageBox.Show($"Pipes loaded successfully from {openFileDialog.FileName}",
                                      "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pipes: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitApplication()
        {
            // auto-save prompt
            if (AllPipes.Any())
            {
                var result = MessageBox.Show("Would you like to save before exiting?",
                                           "Save Before Exit",
                                           MessageBoxButton.YesNoCancel,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    SavePipes();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return; // don't exit
                }
            }

            Application.Current.Shutdown();
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            ApplyCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(ShowYoungsModulus));
        }

        // update property change handler to watch PipeType changes
        private void CurrentPipe_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Pipe.PipeType))
            {
                RaisePropertyChanged(nameof(ShowYoungsModulus));
            }
            RaiseCommandsCanExecuteChanged();
        }
    }
}
