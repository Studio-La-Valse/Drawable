namespace StudioLaValse.Drawable.WPF.ViewModels
{
    public abstract class PropertyViewModel
    {
        public string Description { get; }

        public PropertyViewModel(string description)
        {
            Description = description;
        }
    }

    public class IntegerPropertyViewModel : PropertyViewModel
    {
        public int Value { get; set; }


        public IntegerPropertyViewModel(int value, string description) : base(description)
        {
            Value = value;
        }
    }

    public class DoublePropertyViewModel : PropertyViewModel
    {
        public double Value { get; set; }


        public DoublePropertyViewModel(double value, string description) : base(description)
        {
            Value = value;
        }
    }

    public class DecimalPropertyViewModel : PropertyViewModel
    {
        public decimal Value { get; set; }


        public DecimalPropertyViewModel(decimal value, string description) : base(description)
        {
            Value = value;
        }
    }

    public class StringPropertyViewModel : PropertyViewModel
    {
        public string Value { get; set; }


        public StringPropertyViewModel(string value, string description) : base(description)
        {
            Value = value;
        }
    }

    public class FileSourcePropertyViewModel : StringPropertyViewModel
    {
        public FileSourcePropertyViewModel(string source, string description) : base(source, description)
        {

        }
    }
}
