namespace PurseAccountinng.Mobile.Presentation.Components.Fields;

public partial class PasswordField : FloatingFieldBase
{
    public PasswordField()
    {
        InitializeComponent();
        InitializeFloatingField(FieldEntry, TitleLabel);
    }

    private void OnToggleVisibilityClicked(object sender, EventArgs e)
    {
        FieldEntry.IsPassword = !FieldEntry.IsPassword;

        ToggleVisibilityButton.Source = FieldEntry.IsPassword
            ? "visibility_outlined.png"
            : "visibility_off_outlined.png";
    }
}
