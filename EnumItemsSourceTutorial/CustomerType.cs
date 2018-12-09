using System.ComponentModel;

namespace EnumItemsSourceTutorial
{
    public enum CustomerType
    {
        [Description("Contact Only")]
        Contact,

        [Description("Qualified Lead")]
        Lead,

        [Description("Prospect In Progress")]
        Prospect,

        [Description("Current Customer")]
        Customer,

        [Description("Previous Customer")]
        ExCustomer
    }
}