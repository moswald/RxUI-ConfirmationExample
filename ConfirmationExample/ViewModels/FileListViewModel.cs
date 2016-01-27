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

            ConfirmDelete = new Interaction<ConfirmEventViewModel, string>();

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

                    while (true)
                    {
                        var info = new ConfirmEventViewModel(abort)
                        {
                            File = fileToDelete,
                            Message = message
                        };
                        var confirmation = await ConfirmDelete.Handle(info);

                        if (confirmation == null)
                        {
                            break;
                        }
                        else if (confirmation == fileToDelete)
                        {
                            SelectedFile = null;
                            Files.Remove(fileToDelete);
                            break;
                        }
                        else
                        {
                            message = baseMessage + "\nYou didn't type the right thing." + help;
                        }
                    }
                },
                this.WhenAnyValue(vm => vm.SelectedFile).Select(s => s != null));
        }

        public Interaction<ConfirmEventViewModel, string> ConfirmDelete { get; }

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
