using PurseAccounting.Mobile.Application.Login;
using ReactiveUI;
using System.Reactive;

namespace PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login
{
    public class LoginViewModel : ReactiveObject
    {
        private readonly ILoginService _loginService;

        private string? _login;
        private string? _password;
        private bool _canLogin = true;

        public string? Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value, nameof(Login));
        }

        public string? Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value, nameof(Password));
        }

        public bool CanLogin
        {
            get => _canLogin;
            set => this.RaiseAndSetIfChanged(ref _canLogin, value, nameof(CanLogin));
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public ReactiveCommand<Unit, Unit> GoogleLoginCommand { get; }

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;

            this.WhenAnyValue(x => x.Login).Subscribe(login => UpdateCanLogin(login, Password));
            this.WhenAnyValue(x => x.Password).Subscribe(password => UpdateCanLogin(Login, password));

            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
            GoogleLoginCommand = ReactiveCommand.CreateFromTask(GoogleLoginAsync);
        }

        public async Task LoginAsync()
        {
            if (Login != null && Password != null)
                await _loginService.Login(Login, Password, CancellationToken.None);
        }

        public Task GoogleLoginAsync()
        {
            return Task.CompletedTask;
        }

        private void UpdateCanLogin(string? login, string? password)
        {
            CanLogin = !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password);
        }
    }
}
