using System;
using System.Windows;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class ControlItems : ViewModelBase
    {
        private object? controlContent;
        public object? ControlContent { get => controlContent ??= CreateContent(); }
        private readonly object? dataContext;
        private readonly Type? controlType;

        public string? ControlName { get; private set; }
        public ControlItems(string controlName, Type contentType, object? controlContext = null)
        {
            ControlName = controlName;
            controlType = contentType;
            dataContext = controlContext;
        }
        private object? CreateContent()
        {
            object? content = Activator.CreateInstance(controlType!);
            if (dataContext != null && content is FrameworkElement element)
                element.DataContext = dataContext;
            return content;
        }
    }
}