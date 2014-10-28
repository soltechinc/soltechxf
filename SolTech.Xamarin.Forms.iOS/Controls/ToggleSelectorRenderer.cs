using MonoTouch.UIKit;
using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ToggleSelector), typeof(SolTech.Forms.iOS.ToggleSelectorRenderer))]

namespace SolTech.Forms.iOS
{
    public class ToggleSelectorRenderer : ViewRenderer<ToggleSelector, UISegmentedControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ToggleSelector> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                var nativeSegmentedControl = (UISegmentedControl)Control;
                nativeSegmentedControl.ValueChanged -= ValueChangedListener;
            }

            if (e.NewElement != null)
            {   // perform initial setup
                // lets get a reference to the native control
                var nativeSegmentedControl = new UISegmentedControl();
                nativeSegmentedControl.InsertSegment(e.NewElement.LeftText, 0, false);
                nativeSegmentedControl.InsertSegment(e.NewElement.RightText, 1, false);
                nativeSegmentedControl.SelectedSegment = (int)e.NewElement.SelectedItem;
                
                nativeSegmentedControl.ValueChanged += ValueChangedListener;

                SetNativeControl(nativeSegmentedControl);
            }
        }

        private void SetNativeSelection(ToggleSelectorItem selection)
        {
            var nativeSegmentedControl = Control as UISegmentedControl;
            if (nativeSegmentedControl == null) return;

            System.Diagnostics.Debug.Assert(selection == ToggleSelectorItem.Left || selection == ToggleSelectorItem.Right);
            nativeSegmentedControl.SelectedSegment = (int)selection;
        }

        private void ValueChangedListener(object sender, System.EventArgs args)
        {
            var nativeSegmentedControl = sender as UISegmentedControl;
            if (nativeSegmentedControl == null) return;

            this.Element.SelectedItem = (ToggleSelectorItem)nativeSegmentedControl.SelectedSegment;
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