using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeProject.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;

namespace PipeProject.Modules
{
    public class PipeModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public PipeModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // go to pipe data entry view
            _regionManager.RequestNavigate(RegionNames.MainRegion, "PipeView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
