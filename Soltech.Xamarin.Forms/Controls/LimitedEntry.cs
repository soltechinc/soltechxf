using Xamarin.Forms;

namespace SolTech.Forms
{
    public class LimitedEntry : Xamarin.Forms.Entry
    {
        /// <summary>Identifies the MaxLength bindable property.</summary>
        /// <remarks />
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create<LimitedEntry, int>(t => t.MaxLength, -1);

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
