namespace ConfirmationExample.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public class ConfirmEventViewModel : ReactiveObject
    {
        private IObservable<string> handled;

        public ConfirmEventViewModel(IObservable<Unit> abort)
        {
            this.Cancel = ReactiveCommand.Create(() => { });
            this.Confirm = ReactiveCommand.Create(() => { });
            this.handled = this
                .Confirm
                .Select(_ => this.ResultValue)
                .Merge(this.Cancel.Select(_ => (string)null))
                .Merge(abort.Select(_ => (string)null));
        }

        public IObservable<string> Handled => this.handled;

        public string Message
        {
            get;
            set;
        }

        public string File
        {
            get;
            set;
        }

        string _resultValue;
        public string ResultValue
        {
            get { return _resultValue; }
            set { this.RaiseAndSetIfChanged(ref _resultValue, value); }
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; set; }
        public ReactiveCommand<Unit, Unit> Confirm { get; set; }
    }
}