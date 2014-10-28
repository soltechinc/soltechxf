using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ToggleSelector), typeof(SolTech.Forms.WinPhone.ToggleSelectorRenderer))]

namespace SolTech.Forms.WinPhone
{
    public class ToggleSelectorRenderer : ViewRenderer<ToggleSelector, System.Windows.Controls.Grid>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ToggleSelector> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                var nativeRadioGroup = (System.Windows.Controls.Grid)Control;
                foreach (var child in nativeRadioGroup.Children)
                {
                    var radioButton = child as System.Windows.Controls.RadioButton;
                    if (radioButton != null) radioButton.Checked -= OnCheckedListener;
                }
            }
            
            if (e.NewElement != null)
            {
                var nativeRadioGroup = new System.Windows.Controls.Grid();
                var left = new System.Windows.Controls.RadioButton
                { 
                    Content = e.NewElement.LeftText,
                    IsChecked = e.NewElement.SelectedItem == ToggleSelectorItem.Left,
                    Tag = ToggleSelectorItem.Left
                };
                var right = new System.Windows.Controls.RadioButton
                {
                    Content = e.NewElement.RightText,
                    IsChecked = e.NewElement.SelectedItem == ToggleSelectorItem.Right,
                    Tag = ToggleSelectorItem.Right
                };

                left.Checked += OnCheckedListener;
                right.Checked += OnCheckedListener;

                nativeRadioGroup.Children.Add(left);
                nativeRadioGroup.Children.Add(right);
                SetNativeControl(nativeRadioGroup);
            }
        }

        private void SetNativeSelection(ToggleSelectorItem selection)
        {
            var nativeRadioGroup = Control as System.Windows.Controls.Grid;
            if (nativeRadioGroup == null) return;

            System.Diagnostics.Debug.Assert(selection == ToggleSelectorItem.Left || selection == ToggleSelectorItem.Right);
            System.Diagnostics.Debug.Assert(nativeRadioGroup.Children.Count > 1);
            var radioButton = nativeRadioGroup.Children[(int)selection] as System.Windows.Controls.RadioButton;
            System.Diagnostics.Debug.Assert(radioButton != null);
            radioButton.IsChecked = true;
        }

        private void OnCheckedListener(object sender, System.Windows.RoutedEventArgs args)
        {
            var nativeRadioGroup = Control as System.Windows.Controls.Grid;
            if (nativeRadioGroup == null) return;

            var radioButton = sender as System.Windows.Controls.RadioButton;
            if (radioButton != default(System.Windows.Controls.RadioButton) && 
                radioButton.Tag != null && 
                radioButton.Tag is ToggleSelectorItem)
            {
                this.Element.SelectedItem = (ToggleSelectorItem)radioButton.Tag;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var source = sender as ToggleSelector;
            if (source != null && e.PropertyName == ToggleSelector.SelectedItemProperty.PropertyName)
            {
                SetNativeSelection(source.SelectedItem);
            }
        }
    }
}