namespace ConfirmationExample.Views
{
    using System.Windows;
    using ReactiveUI;
    using ViewModels;

    public sealed partial class ConfirmEventView : IViewFor<ConfirmEventViewModel>
    {
        public ConfirmEventView()
        {
            this.InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Message, v => v.WarningMessage.Text);
            this.Bind(ViewModel, vm => vm.ResultValue, v => v.ConfirmationBox.Text);
            this.BindCommand(ViewModel, vm => vm.Confirm, v => v.ConfirmButton);
            this.BindCommand(ViewModel, vm => vm.Cancel, v => v.CancelButton);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(ConfirmEventViewModel),
            typeof(ConfirmEventView),
            new PropertyMetadata(default(ConfirmEventViewModel)));

        public ConfirmEventViewModel ViewModel
        {
            get { return (ConfirmEventViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConfirmEventViewModel)value; }
        }
    }
}
