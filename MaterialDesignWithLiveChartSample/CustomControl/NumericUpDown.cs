using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDesignWithLiveChartSample.CustomControl
{
    /// <summary>
    /// XAML 파일에서 이 사용자 지정 컨트롤을 사용하려면 1a 또는 1b단계를 수행한 다음 2단계를 수행하십시오.
    ///
    /// 1a단계) 현재 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:MaterialDesignWithLiveChartSample.CustomControl"
    ///
    ///
    /// 1b단계) 다른 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:MaterialDesignWithLiveChartSample.CustomControl;assembly=MaterialDesignWithLiveChartSample.CustomControl"
    ///
    /// 또한 XAML 파일이 있는 프로젝트의 프로젝트 참조를 이 프로젝트에 추가하고
    /// 다시 빌드하여 컴파일 오류를 방지해야 합니다.
    ///
    ///     솔루션 탐색기에서 대상 프로젝트를 마우스 오른쪽 단추로 클릭하고
    ///     [참조 추가]->[프로젝트]를 차례로 클릭한 다음 이 프로젝트를 찾아서 선택합니다.
    ///
    ///
    /// 2단계)
    /// 계속 진행하여 XAML 파일에서 컨트롤을 사용합니다.
    ///
    ///     <MyNamespace:NumericUpDown/>
    ///
    /// </summary>
    [TemplatePart(Name = "UpButtonElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "DownButtonElement", Type = typeof(RepeatButton))]
    [TemplateVisualState(Name = "Positive", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Negative", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusedStates")]
    public class NumericUpDown : Control/*, INotifyPropertyChanged*/
    {
        public NumericUpDown()
        {
            DefaultStyleKey = typeof(NumericUpDown);
            this.IsTabStop = true;
            Value = 5;
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
        //private void OnPropertyChanged([CallerMemberName] string propName = "") =>
        //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ValueChangedCallback)) );

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
                //OnPropertyChanged(nameof(Value));
            }
        }

        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown ctl = (NumericUpDown)obj;
            int newValue = (int)args.NewValue;

            // Call UpdateStates because the Value might have caused the
            // control to change ValueStates.
            ctl.UpdateStates(true);

            // Call OnValueChanged to raise the ValueChanged event.
            ctl.OnValueChanged(
                new ValueChangedEventArgs(NumericUpDown.ValueChangedEvent,
                    newValue));
        }

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Direct,
                          typeof(ValueChangedEventHandler), typeof(NumericUpDown));

        public event ValueChangedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            // Raise the ValueChanged event so applications can be alerted
            // when Value changes.
            RaiseEvent(e);
        }

        private void UpdateStates(bool useTransitions)
        {
            if (Value >= 0)
            {
                VisualStateManager.GoToState(this, "Positive", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Negative", useTransitions);
            }

            if (IsFocused)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }
        }

        public override void OnApplyTemplate()
        {
            UpButtonElement = GetTemplateChild("UpButton") as RepeatButton;
            DownButtonElement = GetTemplateChild("DownButton") as RepeatButton;
            //TextElement = GetTemplateChild("TextBlock") as TextBlock;

            UpdateStates(false);
        }

        private RepeatButton? downButtonElement;

        private RepeatButton? DownButtonElement
        {
            get
            {
                return downButtonElement;
            }

            set
            {
                if (downButtonElement != null)
                {
                    downButtonElement.Click -=
                        new RoutedEventHandler(downButtonElement_Click);
                }
                downButtonElement = value;

                if (downButtonElement != null)
                {
                    downButtonElement.Click +=
                        new RoutedEventHandler(downButtonElement_Click);
                }
            }
        }

        void downButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }

        private RepeatButton? upButtonElement;

        private RepeatButton? UpButtonElement
        {
            get
            {
                return upButtonElement;
            }

            set
            {
                if (upButtonElement != null)
                {
                    upButtonElement.Click -=
                        new RoutedEventHandler(upButtonElement_Click);
                }
                upButtonElement = value;

                if (upButtonElement != null)
                {
                    upButtonElement.Click +=
                        new RoutedEventHandler(upButtonElement_Click);
                }
            }
        }

        void upButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Focus();
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateStates(true);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateStates(true);
        }
    }

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class ValueChangedEventArgs : RoutedEventArgs
    {
        private int _value;

        public ValueChangedEventArgs(RoutedEvent id, int num)
        {
            _value = num;
            RoutedEvent = id;
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
