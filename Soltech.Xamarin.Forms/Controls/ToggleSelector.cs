using System;
using Xamarin.Forms;

namespace SolTech.Forms
{
    public enum ToggleSelectorItem
    {
        Left,
        Right
    }
    public class ToggleSelector : View
    {
        public static readonly BindableProperty LeftTextProperty = BindableProperty.Create<ToggleSelector, String>(t => t.LeftText, null);
        public static readonly BindableProperty RightTextProperty = BindableProperty.Create<ToggleSelector, String>(t => t.RightText, null);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<ToggleSelector, ToggleSelectorItem>(t => t.SelectedItem, ToggleSelectorItem.Left, BindingMode.TwoWay);

        public String LeftText
        {
            get
            {
                return base.GetValue(LeftTextProperty) as String;
            }
            set
            {
                base.SetValue(LeftTextProperty, value);
            }
        }

        public String RightText
        {
            get
            {
                return base.GetValue(RightTextProperty) as String;
            }
            set
            {
                base.SetValue(RightTextProperty, value);
            }
        }


        public ToggleSelectorItem SelectedItem
        {
            get
            {
                return (ToggleSelectorItem)base.GetValue(SelectedItemProperty);
            }
            set
            {
                base.SetValue(SelectedItemProperty, value);
            }
        }
    }
}
