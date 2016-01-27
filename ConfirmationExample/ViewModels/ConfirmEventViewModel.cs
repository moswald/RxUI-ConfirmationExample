namespace ConfirmationExample.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using ReactiveUI;

    public class ConfirmEventViewModel : ReactiveObject
    {
        public ConfirmEventViewModel(IObservable<Unit> abort, Interaction<Unit, string> info)
        {
            this.Cancel = ReactiveCommand.Create(() => { });
            this.Confirm = ReactiveCommand.Create(() => { });

            info.RegisterHandler(
                context => Confirm
                    .Select(_ => ResultValue)
                    .Merge(Cancel.Select(_ => (string)null))
                    .Merge(abort.Select(_ => (string)null))
                    .Do(
                        result =>
                        {
                            context.SetOutput(result);
                            ResultValue = string.Empty;
                        })

                    // @kentcb: take a look at this
                    // not sure about this design - if you comment out this line, then the app crashes with "no handlers"
                    .Select(_ => Unit.Default));
        }

        string _message;
        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }

        string _resultValue = string.Empty;
        public string ResultValue
        {
            get { return _resultValue; }
            set { this.RaiseAndSetIfChanged(ref _resultValue, value); }
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Confirm { get; }
    }
}