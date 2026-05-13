namespace PurseAccountinng.Mobile.Presentation.Components.Fields;

public partial class TextField : FloatingFieldBase
{
    public TextField()
    {
        InitializeComponent();
        InitializeFloatingField(FieldEntry, TitleLabel);
    }
}
