namespace EnumItemsSourceTutorial
{
    public class EnumContainer
    {
        public EnumContainer(object enumValue, string enumDescription)
        {
            Value = enumValue;
            Description = enumDescription;
        }

        public object Value { get; }

        public string Description { get; }

        public override string ToString()
        {
            return Description;
        }
    }
}