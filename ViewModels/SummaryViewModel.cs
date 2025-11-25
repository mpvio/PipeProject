using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeProject.Infrastructure;
using PipeProject.Models;
using PipeProject.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace PipeProject.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IPipeService _pipeService;

        public SummaryViewModel(IRegionManager regionManager, IPipeService pipeService)
        {
            _regionManager = regionManager;
            _pipeService = pipeService;

            BackCommand = new DelegateCommand(GoBack);
        }

        public ObservableCollection<Pipe> Pipes { get; } = new ObservableCollection<Pipe>();

        public DelegateCommand BackCommand { get; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RefreshPipes();
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, "PipeView");
        }

        private void RefreshPipes()
        {
            Pipes.Clear();
            foreach (var pipe in _pipeService.GetAllPipes())
                Pipes.Add(pipe);
        }
    }
}
