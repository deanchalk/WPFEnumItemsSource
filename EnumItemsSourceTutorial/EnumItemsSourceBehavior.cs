using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace EnumItemsSourceTutorial
{
    public class EnumItemsSourceBehavior : Behavior<Selector>
    {
        public static readonly DependencyProperty EnumTypeProperty =
            DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumItemsSourceBehavior),
                new PropertyMetadata(null, OnEnumTypePropertyChanged));

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(EnumItemsSourceBehavior),
                new PropertyMetadata(null, OnSelectedValueChanged));

        public Type EnumType
        {
            get => (Type) GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        public object SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (EnumItemsSourceBehavior) d;
            behavior.SetSelectedItem(e.NewValue);
        }

        private static void OnEnumTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EnumItemsSourceBehavior) d).SetItemsSource(e.NewValue as Type);
        }

        private void SetItemsSource(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum || AssociatedObject == null)
                return;
            AssociatedObject.ItemsSource = GetEnumContainerEnumerable(EnumType);
            SetSelectedItem(SelectedValue);
        }

        private void SetSelectedItem(object value)
        {
            if (AssociatedObject?.ItemsSource == null)
                return;
            var values = AssociatedObject.ItemsSource as IEnumerable<EnumContainer>;
            var selected = values?.SingleOrDefault(v => v.Value.Equals(value));
            if (selected != null)
                AssociatedObject.SelectedItem = selected;
        }

        protected override void OnAttached()
        {
            SetItemsSource(EnumType);
            AssociatedObject.SelectionChanged += OnSelectedItemChanged;
        }

        private void OnSelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count == 0)
                return;
            AssociatedObject.SelectionChanged -= OnSelectedItemChanged;
            SetCurrentValue(SelectedValueProperty, ((EnumContainer) e.AddedItems[0]).Value);
            AssociatedObject.SelectionChanged += OnSelectedItemChanged;
        }

        private static IEnumerable GetEnumContainerEnumerable(Type type)
        {
            if (!type.IsEnum)
                return null;
            var values = Enum.GetValues(type).Cast<object>().ToList();
            var enumContainerList = values
                .Select(value => new EnumContainer(value, GetEnumDescription(value as Enum)))
                .ToList();
            return enumContainerList;
        }

        private static string GetEnumDescription(Enum value)
        {
            if (value == null)
                return string.Empty;

            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}