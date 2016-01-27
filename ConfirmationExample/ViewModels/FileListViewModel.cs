namespace ConfirmationExample.ViewModels
{
    using System.Reactive;
    using System.Reactive.Linq;
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

            var confirmDelete = new Interaction<Unit, string>();

            DeleteFile = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var fileToDelete = SelectedFile;
                    var baseMessage = $"Do you really want to delete {fileToDelete}?";
                    var help = "\nConfirm by entering the full file name below.";
                    var message = baseMessage + help;
                    var abort = this
                        .WhenAnyValue(x => x.SelectedFile)
                        .Skip(1)
                        .Select(_ => Unit.Default);

                    ConfirmDeleteViewModel = new ConfirmEventViewModel(abort, confirmDelete)
                    {
                        Message = message
                    };

                    while (true)
                    {
                        var confirmation = await confirmDelete.Handle(Unit.Default);

                        if (confirmation == fileToDelete)
                        {
                            SelectedFile = null;
                            Files.Remove(fileToDelete);
                            ConfirmDeleteViewModel = null;
                            break;
                        }

                        if (confirmation == null)
                        {
                            ConfirmDeleteViewModel = null;
                            break;
                        }

                        ConfirmDeleteViewModel.Message = baseMessage + "\nYou didn't type the right thing." + help;
                    }
                },
                this.WhenAnyValue(vm => vm.SelectedFile).Select(s => s != null));
        }

        ConfirmEventViewModel _confirmDeleteViewModel;
        public ConfirmEventViewModel ConfirmDeleteViewModel
        {
            get { return _confirmDeleteViewModel; }
            set { this.RaiseAndSetIfChanged(ref _confirmDeleteViewModel, value); }
        }

        public ReactiveCommand<Unit, Unit> DeleteFile { get; }

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
