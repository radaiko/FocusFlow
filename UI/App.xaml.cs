using Core;

namespace UI;

public partial class App : Application {
  public App() {
    InitializeComponent();
    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
  }


  private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
    var exception = (Exception)e.ExceptionObject;
    Logger.Error("Unhandled exception", exception);
  }

  private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) {
    var exception = e.Exception;
    Logger.Error("Unhandled exception", exception);
    e.SetObserved();
  }

  protected override Window CreateWindow(IActivationState? activationState) {
    return new Window(new MainPage());
  }
}