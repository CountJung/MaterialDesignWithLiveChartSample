using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
//using System.Windows.Interactivity;
using Microsoft.Xaml.Behaviors;

namespace MaterialDesignWithLiveChartSample.Behavior
{
    public class TemplateBindingBehavior : Behavior<DependencyObject>
    {
        public PropertyPath? TargetTemplatePropertyPath { get; set; }

        public PropertyPath? SourceProperty { get; set; }

        protected override void OnAttached()
        {
            var targetBinding = new Binding
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            };
            if (TargetTemplatePropertyPath != null && TargetTemplatePropertyPath.PathParameters.Count > 1)
            {
                object[] parameters = TargetTemplatePropertyPath.PathParameters
                    .Take(TargetTemplatePropertyPath.PathParameters.Count - 1)
                    .ToArray();
                string path = string.Join(".",
                    Enumerable.Range(0, parameters.Length).Select(x => $"({x})"));
                targetBinding.Path = new PropertyPath(path, parameters);
            }

            base.OnAttached();
            var target = targetBinding.Evaluate<DependencyObject>(this);

            if (target != null)
            {
                BindingOperations.SetBinding(target, (DependencyProperty)TargetTemplatePropertyPath!.PathParameters.Last(), new Binding
                {
                    Path = SourceProperty,
                    Source = AssociatedObject,
                    Mode = BindingMode.OneWay
                });
            }
        }
    }

    public static class BindingMixins
    {
        private static readonly DependencyProperty EvalProperty = DependencyProperty.RegisterAttached(
            "Eval", typeof(object), typeof(BindingMixins), new PropertyMetadata(default(object)));

        public static T? Evaluate<T>(this BindingBase binding, DependencyObject target)
        {
            if (binding == null) throw new ArgumentNullException(nameof(binding));
            if (target == null) throw new ArgumentNullException(nameof(target));

            BindingOperations.SetBinding(target, EvalProperty, binding);
            if (target.GetValue(EvalProperty) is T value)
            {
                return value;
            }
            return default;
        }
    }
}
