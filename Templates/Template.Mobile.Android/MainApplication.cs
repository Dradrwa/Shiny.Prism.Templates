﻿using Android.App;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Plugin.CurrentActivity;
using Shiny;
using System;
using Template.Mobile.Droid.Services;
using Template.Mobile.Services;
using Xamarin.Forms;

namespace Template.Mobile.Droid
{

#if DEBUG
    [Application(Debuggable = true)]
    #else
    [Application(Debuggable = false)]
    #endif
    public class MainApplication : ShinyAndroidApplication<Startup>
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {

            
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
        }

        protected override void OnBuildApplication(IServiceCollection services)
        {
            base.OnBuildApplication(services);

            services.AddSingleton<IPlatformThemeService, PlatformThemeService>();
        }
    }
}