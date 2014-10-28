using SolTech.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(LimitedEntry), typeof(SolTech.Forms.WinPhone.LimitedEntryRenderer))]

namespace SolTech.Forms.WinPhone
{
    public class LimitedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null) {   // perform initial setup
                // lets get a reference to the native control
                var grid = Control as global::System.Windows.Controls.Grid;

                LimitedEntry numericEntry = e.NewElement as LimitedEntry;
                if (numericEntry.MaxLength > 0)
                {
                    foreach (var element in grid.Children)
                    {
                        var textBox = element as System.Windows.Controls.TextBox;
                        var passwordBox = element as System.Windows.Controls.PasswordBox;
    
                        if (textBox != default(System.Windows.Controls.TextBox))
                        {
                            textBox.MaxLength = numericEntry.MaxLength;
                        }
                        if (passwordBox != default(System.Windows.Controls.PasswordBox))
                        {
                            passwordBox.MaxLength = numericEntry.MaxLength;
                        }
                    }
                }
            }
        }
    }
}