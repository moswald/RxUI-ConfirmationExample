namespace ConfirmationExample.Views
{
    using System.Windows;
    using ReactiveUI;
    using ViewModels;

    public sealed partial class FileListView : IViewFor<FileListViewModel>
    {
        public FileListView()
        {
            this.InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Files, v => v.FileList.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedFile, v => v.FileList.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.DeleteFile, v => v.DeleteFileButton);

            this.OneWayBind(ViewModel, vm => vm.ConfirmDeleteViewModel, v => v.DeleteConfirm.ViewModel);
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel",
            typeof(FileListViewModel),
            typeof(FileListView),
            new PropertyMetadata(default(FileListViewModel)));

        public FileListViewModel ViewModel
        {
            get { return (FileListViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FileListViewModel)value; }
        }
    }
}
