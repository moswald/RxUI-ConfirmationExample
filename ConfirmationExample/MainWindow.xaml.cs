namespace ConfirmationExample
{
    using ReactiveUI;
    using Splat;
    using ViewModels;
    using Views;

    public partial class MainWindow : IScreen
    {
        public MainWindow()
        {
            InitializeComponent();

            Locator.CurrentMutable.Register(() => new FileListView(), typeof(IViewFor<FileListViewModel>));
            Locator.CurrentMutable.Register(() => new ConfirmEventView(), typeof(IViewFor<ConfirmEventViewModel<string>>));

            Router = ViewHost.Router = new RoutingState();
            Router.Navigate.Execute(new FileListViewModel(this));
        }

        public RoutingState Router { get; }
    }
}
