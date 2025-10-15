using MauiIcons.Core;
using MauiIcons.Material.Outlined;

namespace PurseAccountinng.Mobile.Presentation.Pages.Unauthorized;

public partial class LoginPage : ContentPage
{
	private bool _isPasswordVisible = false;
	
	public LoginPage()
	{
		InitializeComponent();
	}

	private void OnTogglePasswordClicked(object sender, EventArgs e)
	{
		_isPasswordVisible = !_isPasswordVisible;
		PasswordEntry.IsPassword = !_isPasswordVisible;

        TogglePasswordButton.Source = _isPasswordVisible
            ? "visibility_outlined.svg"
            : "visibility_off_outlined.svg";
    }
}
