using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SolTech.Forms
{
    public class ImageButton : Image
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<ImageButton, ICommand>(t => t.Command, null, BindingMode.OneWay, null, OnCommandChanged);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<ImageButton, object>(t => t.CommandParameter, null, BindingMode.OneWay, null, OnCommandParameterChanged);

        public ImageButton()
        {
            GestureRecognizers.Add(new TapGestureRecognizer(sender =>
            {
                // Do whatever you want to do when its tapped
                Opacity = 0.6;
                this.FadeTo(1);
                ICommand command = Command;
                if (command != null)
                {
                    command.Execute(null);
                }
            }));
        }

        public ICommand Command
        {
            get
            {
                return GetValue(CommandProperty) as ICommand;
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>Gets or sets the parameter to pass to the Command property. This is a bindable property.</summary>
        /// <value>A object to pass to the command property. The default value is <see langword="null" />.</value>
        /// <remarks />
        public object CommandParameter
        {
            get
            {
                return base.GetValue(Button.CommandParameterProperty);
            }
            set
            {
                base.SetValue(Button.CommandParameterProperty, value);
            }
        }

        private static void OnCommandChanged(BindableObject bindable, ICommand oldValue, ICommand newValue)
        {
            ImageButton button = bindable as ImageButton;
            if (button == null) throw new InvalidOperationException("Expected ImageButton");

            if (oldValue != null)
            {
                oldValue.CanExecuteChanged -= button.CommandCanExecuteChanged;
            }
            if (newValue != null)
            {
                newValue.CanExecuteChanged += button.CommandCanExecuteChanged;
                button.CommandCanExecuteChanged(button, EventArgs.Empty);
            }
            button.IsEnabled = newValue.CanExecute(button.CommandParameter);
        }

        private void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            ICommand command = this.Command;
            if (command != null)
            {
                IsEnabled = command.CanExecute(CommandParameter);
            }
        }

        private static void OnCommandParameterChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ImageButton button = bindable as ImageButton;
            if (button == null) throw new InvalidOperationException("Expected ImageButton");
            button.CommandCanExecuteChanged(bindable, EventArgs.Empty);
        }
    }
}
