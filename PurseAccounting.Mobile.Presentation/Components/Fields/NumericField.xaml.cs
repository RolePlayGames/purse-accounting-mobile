namespace PurseAccountinng.Mobile.Presentation.Components.Fields;

public partial class NumericField : FloatingFieldBase
{
    public NumericField()
    {
        InitializeComponent();
        InitializeFloatingField(FieldEntry, TitleLabel);
    }
}
