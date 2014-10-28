using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ToggleSelector), typeof(SolTech.Forms.Android.ToggleSelectorRenderer))]

namespace SolTech.Forms.Android
{
    public class ToggleSelectorRenderer : ViewRenderer<ToggleSelector, global::Android.Widget.RadioGroup>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ToggleSelector> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                var nativeRadioGroup = (global::Android.Widget.RadioGroup)Control;
                for (int childIndex = 0; childIndex < nativeRadioGroup.ChildCount; childIndex++)
                {
                    var child = nativeRadioGroup.GetChildAt(childIndex) as global::Android.Widget.RadioButton;
                    if (child != null) child.Click -= OnClickListener;
                }
            }
            
            if (e.NewElement != null)
            {
                var nativeRadioGroup = new global::Android.Widget.RadioGroup(Context) {
                    Id = 92344,
                    Orientation = global::Android.Widget.Orientation.Horizontal,
                    LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
                };
                var left = new global::Android.Widget.RadioButton(nativeRadioGroup.Context) 
                { 
                    Id = 92345,
                    Text = e.NewElement.LeftText,
                    Checked = e.NewElement.SelectedItem == ToggleSelectorItem.Left
                };
                var right = new global::Android.Widget.RadioButton(nativeRadioGroup.Context)
                {
                    Id = 92346,
                    Text = e.NewElement.RightText,
                    Checked = e.NewElement.SelectedItem == ToggleSelectorItem.Right
                };
                
                left.Click += OnClickListener;
                right.Click += OnClickListener;

                nativeRadioGroup.AddView(left);
                nativeRadioGroup.AddView(right);
                SetNativeControl(nativeRadioGroup);
            }
        }

        private void SetNativeSelection(ToggleSelectorItem selection)
        {
            var nativeRadioGroup = (global::Android.Widget.RadioGroup)Control;
            if (nativeRadioGroup == null) return;

            System.Diagnostics.Debug.Assert(selection == ToggleSelectorItem.Left || selection == ToggleSelectorItem.Right);
            var radioButton = nativeRadioGroup.GetChildAt((int)selection) as global::Android.Widget.RadioButton;
            System.Diagnostics.Debug.Assert(radioButton != null);
            radioButton.Checked = true;
        }

        private void OnClickListener(object sender, System.EventArgs args)
        {
            var nativeRadioGroup = (global::Android.Widget.RadioGroup)Control;
            if (nativeRadioGroup == null) return;

            for (int childIndex = 0; childIndex < nativeRadioGroup.ChildCount; childIndex++)
            {
                var radioButton = nativeRadioGroup.GetChildAt(childIndex) as global::Android.Widget.RadioButton;
                System.Diagnostics.Debug.Assert(radioButton != null);
                if (radioButton.Checked)
                {
                    System.Diagnostics.Debug.Assert(childIndex >= 0 && childIndex <= 1);
                    this.Element.SelectedItem = (ToggleSelectorItem)childIndex;
                    break;
                }
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