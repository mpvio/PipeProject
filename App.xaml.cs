using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PipeProject.Services;
using PipeProject.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace PipeProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // register services for DI
            containerRegistry.RegisterSingleton<IPipeService, PipeService>();

            // register views for navigation
            containerRegistry.RegisterForNavigation<PipeView>();
            containerRegistry.RegisterForNavigation<SummaryView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Modules.PipeModule>();
        }
    }
}
