namespace ConfirmationExample.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class FileListViewModel : ReactiveObject, IRoutableViewModel
    {
        public FileListViewModel(IScreen host)
        {
            HostScreen = host;

            Files = new ReactiveList<string>(new []
            {
                "c:/temp/foo.txt",
                "c:/temp/bar.txt",
                "c:/temp/baseball.dat",
                "c:/temp/basketball.dat",
                "c:/temp/handegg.dat"
            });

            DeleteFile = ReactiveCommand.CreateAsyncTask(
                this.WhenAnyValue(vm => vm.SelectedFile).Select(s => s != null),
                _ =>
                {
                    var fileToDelete = SelectedFile;
                    var warningMessage = $"Do you really want to delete {fileToDelete}?";
                    var help = "\nConfirm by entering the full file name below.";

                    ConfirmDeleteViewModel = new ConfirmEventViewModel<string>
                    {
                        ResultValue = string.Empty,
                        WarningMessage = warningMessage + help,
                        Cancel = ReactiveCommand.CreateAsyncTask(
                            __ =>
                            {
                                ConfirmDeleteViewModel = null;
                                return Task.CompletedTask;
                            }),
                        Confirm = ReactiveCommand.CreateAsyncTask(
                            __ =>
                            {
                                if (ConfirmDeleteViewModel?.ResultValue == fileToDelete)
                                {
                                    SelectedFile = null;

                                    Files.Remove(fileToDelete);
                                    ConfirmDeleteViewModel = null;
                                }
                                else if (ConfirmDeleteViewModel != null)
                                {
                                    ConfirmDeleteViewModel.WarningMessage = warningMessage + "\nYou didn't type the right thing." + help;
                                }

                                if (ConfirmDeleteViewModel != null)
                                {
                                    ConfirmDeleteViewModel.ResultValue = string.Empty;
                                }

                                return Task.CompletedTask;
                            })
                    };

                    return Task.CompletedTask;
                });

            // don't let the user delete the current file if they click away
            this.WhenAnyValue(vm => vm.SelectedFile)
                .Subscribe(_ => ConfirmDeleteViewModel = null);
        }

        public ReactiveCommand<Unit> DeleteFile { get; }

        ConfirmEventViewModel<string> _confirmDeleteViewModel;
        public ConfirmEventViewModel<string> ConfirmDeleteViewModel
        {
            get { return _confirmDeleteViewModel; }
            set { this.RaiseAndSetIfChanged(ref _confirmDeleteViewModel, value); }
        }

        string _selectedFile;
        public string SelectedFile
        {
            get { return _selectedFile; }
            set { this.RaiseAndSetIfChanged(ref _selectedFile, value); }
        }

        public ReactiveList<string> Files { get; }

        public string UrlPathSegment => string.Empty;
        public IScreen HostScreen { get; }
    }
}
