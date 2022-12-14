using MaterialDesignWithLiveChartSample.Model;
using MaterialDesignWithLiveChartSample.View;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class PieChartDisplayViewModel : ViewModelBase
    {
        public static PieChartDisplayViewModel? Instance { get; private set; }
        private PieChartDisplayModel? model;
        public PieChartDisplayModel? Model { get => model; private set => Set(ref model, value, nameof(Model)); }
        public PieChartDisplayViewModel()
        {
            Instance = this;
            Model = new PieChartDisplayModel();
            PieChartCountSetCmd = new DelegateCommand(PieChartCountSet);
            TestCommandBtnCmd = new DelegateCommand(TestCommandBtn);
        }
        /// <summary>
        /// Must Clear All Chartdatas before "Renew" charts
        /// Recreating Charts causes memory leak
        /// </summary>
        public ICommand PieChartCountSetCmd { get; private set; }
        public void PieChartCountSet(object s)
        {
            foreach (PieChartNode pies in Model?.PieChartData ?? Enumerable.Empty<PieChartNode>())
            {
                for(int i=0;i< pies.ControlValue?.Count; i++)
                {
                    pies.ControlValue[i].Clear();
                }
                pies.ControlValue?.Clear();
                //RemoveAll(pies.ControlValue!);
                //RemoveAll(pies.PieChartSeries?.Chart?.View?.Series!);
                //pies.PieChartSeries?.Chart?.View?.Series?.Clear();
                //RemoveAll(pies.PieChartSeries!);
                pies.PieChartSeries?.Clear();
                //pies.PieChartSeries = null;
            }
            //RemoveAll(Model?.PieChartData!);
            Model?.PieChartData?.Clear();
            //Model!.PieChartData = null;
            //GC.Collect();
            //Model.PieChartData = new ObservableCollection<PieChartNode>();
            uint chartCount = Model?.PiechartCount ?? 0;
            for (uint i = 0; i < chartCount; i++)
            {
                //string nodeName = string.Format("Pie Chart {0}", i);
                Model?.PieChartData?.Add(new PieChartNode(i));
            }
        }
        public static void RemoveAll(IList list)
        {
            while(list.Count > 0) 
            {
                list.RemoveAt(list.Count-1);
            }
        }
        public ICommand TestCommandBtnCmd { get; private set; }
        public void TestCommandBtn(object s)
        {
            // memory leak test
            BindingOperations.ClearBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty);
            Binding binding = new()
            {
                Path = new PropertyPath("ItemsSource"),
                Source = Model?.PieChartData
            };
            BindingOperations.SetBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty, binding);
        }
    }
}
