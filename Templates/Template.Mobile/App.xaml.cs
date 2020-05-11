﻿using System;
using System.Diagnostics;
using System.Linq;
using Prism.Events;
using Prism.Ioc;
using Template.Mobile.Helpers;
using Template.Mobile.ViewModels;
using Template.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Template.Mobile
{
    [AutoRegisterForNavigation]
    public partial class App
    {
        public App()
        {
        }

        protected override void OnInitialized()
        {
            try
            {
                InitializeComponent();

                ResourcesHelper.ApplyTheme(AppTheme.Acrylic);

                NavigationService.NavigateAsync($"{nameof(MDPage)}/{nameof(NavigationPage)}/{nameof(Debug_ImagesListPage)}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //REGISTER ALL USED GENERIC PAGES (not AutoRegistered) IN CONTAINER, FOR PRISM NAVIGATION
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        protected override void OnNavigationError(INavigationError navigationError)
        {
#if DEBUG
            // Ensure we always break here while debugging!
            Debugger.Break();
#else
            Helpers.Logger.Write(navigationError.Exception)(
                new[] {(nameof(navigationError.NavigationUri), navigationError.NavigationUri)}
                    .Concat(navigationError.Parameters
                        .Select(x => (x.Key, x.Value.ToString())))
                    .ToArray());
#endif

            base.OnNavigationError(navigationError);
        }
    }
}
