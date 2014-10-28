using System;
using System.Collections;
using Xamarin.Forms;

namespace SolTech.Forms
{
    public class BoundPicker : Picker
    {
        /// <summary>Event that is raised when a new item is selected.</summary>
        /// <remarks>To be added.</remarks>
        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<BoundPicker, IEnumerable>(t => t.ItemsSource, null, propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<BoundPicker, Object>(t => t.SelectedItem, null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public BoundPicker()
        {
            base.SelectedIndexChanged += (o, e) =>
            {
                if (base.SelectedIndex < 0 || ItemsSource == null || !ItemsSource.GetEnumerator().MoveNext())
                {
                    SelectedItem = null;
                    return;
                }

                int index = 0;
                foreach (var item in ItemsSource)
                {
                    if (index == base.SelectedIndex)
                    {
                        SelectedItem = item;
                        break;
                    }
                    index++;
                }
            };
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return base.GetValue(ItemsSourceProperty) as IEnumerable;
            }
            set
            {
                base.SetValue(ItemsSourceProperty, value);
            }
        }

        public Object SelectedItem
        {
            get
            {
                return base.GetValue(SelectedItemProperty);
            }
            set
            {
                if (SelectedItem != value)
                {
                    base.SetValue(SelectedItemProperty, value);
                    InternalUpdateSelectedIndex();
                }
            }
        }

        private void InternalUpdateSelectedIndex()
        {
            int selectedIndex = -1;
            if (ItemsSource != null)
            {
                int index = 0;
                foreach (var item in ItemsSource)
                {
                    if (item != null && item.Equals(SelectedItem))
                    {
                        selectedIndex = index;
                        break;
                    }
                    index++;
                }
            }
            base.SelectedIndex = selectedIndex;
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BoundPicker boundPicker = (BoundPicker)bindable;
            //boundPicker.BatchBegin ();
            try
            {
                boundPicker.Items.Clear();
                var enumerable = newValue as IEnumerable;
                if (enumerable != null && enumerable.GetEnumerator().MoveNext())
                {
                    foreach (var item in enumerable)
                    {
                        boundPicker.Items.Add(item.ToString());
                    }
                }
                else
                {
                    boundPicker.Items.Add(" ");
                }

                boundPicker.InternalUpdateSelectedIndex();
            }
            finally {
                //boundPicker.BatchCommit ();
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BoundPicker boundPicker = (BoundPicker)bindable;
            if (boundPicker.ItemSelected != null)
            {
                boundPicker.ItemSelected(boundPicker, new SelectedItemChangedEventArgs(newValue));
            }
            boundPicker.InternalUpdateSelectedIndex();
        }
    }
}
