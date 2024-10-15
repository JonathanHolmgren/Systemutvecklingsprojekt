using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum AuthorizationLevel
    {
        [Description("Admin")]
        Admin,

        [Description("Economyassistant")]
        EconomyAssistant,

        [Description("Salesmanager")]
        SalesManager,

        [Description("Salesperson")]
        SalesPerson,

        [Description("CEO")]
        CEO,

        [Description("Salesassistant")]
        SalesAssistant,
    }
}
