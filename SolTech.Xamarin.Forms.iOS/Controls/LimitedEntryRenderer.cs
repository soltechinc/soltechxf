
using MonoTouch.UIKit;
using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.CoreText;

[assembly: ExportRenderer(typeof(LimitedEntry), typeof(SolTech.Forms.iOS.LimitedEntryRenderer))]

namespace SolTech.Forms.iOS
{
    public class LimitedEntryRenderer : EntryRenderer
    {
		public LimitedEntryRenderer ()
		{
			
		}
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {   // perform initial setup
                // lets get a reference to the native control
                var nativeEditor = (UITextField)Control;
                LimitedEntry numericEntry = e.NewElement as LimitedEntry;
                if (numericEntry.MaxLength > 0)
                {
                    nativeEditor.ShouldChangeCharacters = (textField, range, replacementString) =>
                    {
                        var newLength = textField.Text.Length + replacementString.Length - range.Length;
                        return newLength <= numericEntry.MaxLength;
                    };
                }
            }
        }
    }
}