﻿using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Shiny;
using Template.Services.Dialogs;
using Template.Services.Localization;
using Template.Services.Settings;

namespace Template.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject,
                                      IAutoInitialize,
                                      IInitialize,
                                      IInitializeAsync,
                                      INavigatedAware,
                                      IPageLifecycleAware,
                                      IDestructible,
                                      IConfirmNavigationAsync
    {
        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            DialogsService = ShinyHost.Resolve<IDialogService>();
            SettingsService = ShinyHost.Resolve<ISettingsService>();
            LocalizationService = ShinyHost.Resolve<ILocalizationService>();

            NavigateBackCommand = ExecutionAwareCommand.FromTask(NavigateBackAsync)
                .OnIsExecutingChanged(OnIsExecutingChanged);
        }

        #region Services

        protected INavigationService NavigationService { get; }

        protected IDialogService DialogsService { get; }

        protected ISettingsService SettingsService { get; }

        protected ILocalizationService LocalizationService { get; }

        #endregion

        #region Properties

        public string this[string name] => LocalizationService.GetText(name);

        private int _busyCounter;
        protected int BusyCounter
        {
            get => _busyCounter;
            set
            {
                _busyCounter = Math.Max(value, 0);
                IsBusy = _busyCounter > 0;
            }
        }

        [Reactive] 
        public bool IsBusy { get; private set; }

        [Reactive] 
        public string Title { get; protected set; }

        #endregion

        #region Commands

        public ICommand NavigateBackCommand { get; }

        #endregion

        #region Methods

        private void OnIsExecutingChanged(bool isExecuting)
        {
            if (isExecuting)
                BusyCounter++;
            else
                BusyCounter--;
        }

        #endregion

        #region Lifecycle

        private CompositeDisposable _deactivateWith;
        protected CompositeDisposable DeactivateWith => _deactivateWith ??= new CompositeDisposable();

        protected CompositeDisposable DestroyWith { get; } = new CompositeDisposable();

        protected virtual void Deactivate()
        {
            _deactivateWith?.Dispose();
            _deactivateWith = null;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) => this.Deactivate();

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual Task InitializeAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual void Destroy() => this.DestroyWith?.Dispose();

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) => Task.FromResult(true);

        public virtual Task NavigateBackAsync() => NavigationService.GoBackAsync(); 

        #endregion
    }
}
