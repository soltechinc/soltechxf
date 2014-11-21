using Xamarin.Forms;

namespace SolTech.Forms
{
    public class StatefulButton : Button
    {
        public static readonly BindableProperty EnabledColorProperty = BindableProperty.Create("EnabledColor", typeof(Color), typeof(StatefulButton), Color.Gray, BindingMode.OneWay, null, StatefulButton.ColorsChanged, null, null);
        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create("DisabledColor", typeof(Color), typeof(StatefulButton), Color.Gray, BindingMode.OneWay, null, StatefulButton.ColorsChanged, null, null);

        public StatefulButton()
        {
            this.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == IsEnabledProperty.PropertyName)
                {
                    if (this.IsEnabled)
                        this.BackgroundColor = this.EnabledColor;
                    else
                        this.BackgroundColor = this.DisabledColor;
                }
            };
        }

        public Color EnabledColor
        {
            get
            {
                return (Color)base.GetValue(StatefulButton.EnabledColorProperty);
            }
            set
            {
                base.SetValue(StatefulButton.EnabledColorProperty, value);
            }
        }

        public Color DisabledColor
        {
            get
            {
                return (Color)base.GetValue(StatefulButton.DisabledColorProperty);
            }
            set
            {
                base.SetValue(StatefulButton.DisabledColorProperty, value);
            }
        }

        private static void ColorsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StatefulButton statefuleButton = (StatefulButton)bindable;
            if (statefuleButton != null)
            {
                if (statefuleButton.IsEnabled)
                {
                    statefuleButton.BackgroundColor = statefuleButton.EnabledColor;
                }
                else
                {
                    statefuleButton.BackgroundColor = statefuleButton.DisabledColor;
                }
            }
        }
    }
}
