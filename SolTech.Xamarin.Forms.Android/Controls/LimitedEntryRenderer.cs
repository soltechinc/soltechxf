using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LimitedEntry), typeof(SolTech.Forms.Android.LimitedEntryRenderer))]

namespace SolTech.Forms.Android
{
    public class LimitedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null) {   // perform initial setup
                // lets get a reference to the native control
                var nativeEditText = (global::Android.Widget.EditText) Control;
                LimitedEntry numericEntry = e.NewElement as LimitedEntry;
                if (numericEntry.MaxLength > 0)
                {
                    nativeEditText.SetFilters(new global::Android.Text.IInputFilter[] { new global::Android.Text.InputFilterLengthFilter(numericEntry.MaxLength) });
                }
            }
        }
    }
}