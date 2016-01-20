namespace ConfirmationExample.ViewModels
{
    using System.Reactive;
    using ReactiveUI;

    public class ConfirmEventViewModel<T> : ReactiveObject
    {
        T _resultValue;
        public T ResultValue
        {
            get { return _resultValue; }
            set { this.RaiseAndSetIfChanged(ref _resultValue, value); }
        }

        string _warningMessage;
        public string WarningMessage
        {
            get { return _warningMessage; }
            set { this.RaiseAndSetIfChanged(ref _warningMessage, value); }
        }

        public ReactiveCommand<Unit> Cancel { get; set; }
        public ReactiveCommand<Unit> Confirm { get; set; }
    }
}