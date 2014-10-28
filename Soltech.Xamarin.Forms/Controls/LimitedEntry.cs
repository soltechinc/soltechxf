using Xamarin.Forms;

namespace SolTech.Forms
{
    public class LimitedEntry : Xamarin.Forms.Entry
    {
        /// <summary>Identifies the MaxLength bindable property.</summary>
        /// <remarks />
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create("MaxLength", typeof(int), typeof(LimitedEntry), -1, BindingMode.OneWay, null, null, null, null);

        public int MaxLength
        {
            get
            {
                return (int)base.GetValue(LimitedEntry.MaxLengthProperty);
            }
            set
            {
                base.SetValue(LimitedEntry.MaxLengthProperty, value);
            }
        }
    }
}
