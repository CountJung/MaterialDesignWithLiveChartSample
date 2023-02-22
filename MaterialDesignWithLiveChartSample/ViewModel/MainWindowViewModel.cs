﻿using MaterialDesignThemes.Wpf;
using MaterialDesignWithLiveChartSample.Class;
using MaterialDesignWithLiveChartSample.Model;
using MaterialDesignWithLiveChartSample.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel? Instance { get; private set; }
        public MainWindowModel? MainModel { get; private set; }
        public MenuViewModel? MenuViewModelInst { get; private set; }
        public ObservableCollection<ControlItems>? ViewControlItems { get; }
        private ControlItems? selectedControlItem;
        public ControlItems? SelectedControlItem { get => selectedControlItem; set => Set(ref selectedControlItem, value); }
        private int selectedControlIndex;
        public int SelectedControlIndex
        {
            get => selectedControlIndex;
            set
            {
                Set(ref selectedControlIndex, value);
                SelectedControlItem = ViewControlItems?.ElementAt(SelectedControlIndex);
            }
        }
        public enum DisplayNumber
        {
            LineChartNumber, PieChartNumber, DataBaseNumber
        }
        public SequenceTask Sequence { get; private set; } = new();
        public MainWindowViewModel()
        {
            Instance = this;
            MainModel = new MainWindowModel();
            MenuItemTestCmd = new DelegateCommand(MenuItemTest);
            MenuItemTestCmd2= new DelegateCommand(MenuItemTest2);
            MenuViewModelInst = new MenuViewModel();
            ViewControlItems = new ObservableCollection<ControlItems>();
            ToggleChangeThemeCmd = new DelegateCommand(ToggleChangeTheme);

            InitializeCustomControls();
        }

        private void InitializeCustomControls()
        {
            ViewControlItems?.Add(new ControlItems("LineChartDisplay", typeof(LineChartDisplay)));
            ViewControlItems?.Add(new ControlItems("PieChartDisplay", typeof(PieChartDisplay)));
            ViewControlItems?.Add(new ControlItems("DataBaseDisplay", typeof(DataBaseDisplay)));
            SelectedControlItem = ViewControlItems?[(int)DisplayNumber.PieChartNumber];
        }
        public void OnClosingMainWindow(object? sender, CancelEventArgs e)
        {
            Sequence.ClearSequence();
            DataBaseDisplayViewModel.Instance?.DBClose();
        }
        public ICommand MenuItemTestCmd { get; private set; }
        private void MenuItemTest(object sender)
        {
            //using PCQueue queueTask = new(1);
            //queueTask.EnqueueTask(async () =>
            //{
            //    await Task.Delay(1000);
            //    Trace.WriteLine($"task queue test Count={queueTask.RemainTaskCount}");
            //    SelectedControlItem = ViewControlItems?[0];
            //});
            //queueTask.EnqueueTask(async () =>
            //{
            //    await Task.Delay(2000);
            //    Trace.WriteLine($"More Test Count={queueTask.RemainTaskCount}");
            //    SelectedControlItem = ViewControlItems?[1];
            //});
            //queueTask.EnqueueTask(() =>
            //{
            //    Trace.WriteLine($"More Test Count={queueTask.RemainTaskCount}");
            //    SelectedControlItem = ViewControlItems?[2];
            //});
            //queueTask.EnqueueTask(async () =>
            //{
            //    // WPF automatically ensures that bindings are updated on the main thread.
            //    //Application.Current.Dispatcher.Invoke(async () =>
            //    //{
            //        for (int i = 0; i < 3; i++)
            //        {
            //            await Task.Delay(1000);
            //            Trace.WriteLine("Loop Test");
            //            SelectedControlItem = ViewControlItems?[i];
            //        }
            //    //});
            //});

            //using QueueTasker queueTasker = new();
            //queueTasker.EnqueueTask(async () =>
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        await Task.Delay(1000);
            //        Trace.WriteLine("Loop Test");
            //        SelectedControlItem = ViewControlItems?[i];
            //    }
            //});
            //queueTasker.EnqueueTask(() =>
            //{
            //    Trace.WriteLine($"More Test");
            //});

            Sequence.AddSequence("ATest", async () =>
            {
                await Task.Delay(1000);
                Trace.WriteLine("A Test Loop Sequence");
                SelectedControlItem = ViewControlItems?[0];
            });
            Sequence.AddSequence("BTest", async () =>
            {
                await Task.Delay(2000);
                Trace.WriteLine("B Test Loop Sequence");
                SelectedControlItem = ViewControlItems?[1];
            });
            Sequence.AddSequence("CTest", async () =>
            {
                await Task.Delay(3000);
                Trace.WriteLine("C Test Loop Sequence");
                SelectedControlItem = ViewControlItems?[2];
            });
        }
        public ICommand MenuItemTestCmd2 { get; private set; }
        private void MenuItemTest2(object s)
        {
            //Sequence.ClearSequence();
            Sequence.RemoveSequence("ATest");
            Sequence.RemoveSequence("BTest");
        }
        public ICommand ToggleChangeThemeCmd { get; private set; }
        private void ToggleChangeTheme(object sender)
        {
            if (MainModel is not null)
                SetDarkTheme(MainModel.DarkTheme);
        }
        private void SetDarkTheme(bool dark)
        {
            if (MainModel is null)
                return;
            ITheme theme = MainModel.MainThemePallette!.GetTheme();
            IBaseTheme baseTheme = dark ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            MainModel.MainThemePallette.SetTheme(theme);
            //LineChartDisplayViewModel.Instance!.SetDarkTheme(dark);
        }
    }
}
