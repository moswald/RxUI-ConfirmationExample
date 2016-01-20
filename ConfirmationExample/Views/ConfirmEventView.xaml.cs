namespace ConfirmationExample.Views
{
    using System.Windows;
    using ReactiveUI;
    using ViewModels;

    public sealed partial class ConfirmEventView : IViewFor<ConfirmEventViewModel<string>>
    {
        public ConfirmEventView()
        {
            this.InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.WarningMessage, v => v.WarningMessage.Text);
            this.Bind(ViewModel, vm => vm.ResultValue, v => v.ConfirmationBox.Text);
            this.BindCommand(ViewModel, vm => vm.Confirm, v => v.ConfirmButton);
            this.BindCommand(ViewModel, vm => vm.Cancel, v => v.CancelButton);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(ConfirmEventViewModel<string>),
            typeof(ConfirmEventView),
            new PropertyMetadata(default(ConfirmEventViewModel<string>)));

        public ConfirmEventViewModel<string> ViewModel
        {
            get { return (ConfirmEventViewModel<string>) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConfirmEventViewModel<string>)value; }
        }
    }
}
