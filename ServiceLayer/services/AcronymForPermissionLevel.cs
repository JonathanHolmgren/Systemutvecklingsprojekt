using Models;

namespace ServiceLayer.services;

public class AcronymForPermissionLevel
{
    public string GetAcronymForAuthorizationLevel(AuthorizationLevel level)
    {
        switch (level)
        {
            case AuthorizationLevel.Admin:
                return "AD";
            case AuthorizationLevel.EconomyAssistant:
                return "EA";
            case AuthorizationLevel.SalesAssistant:
                return "SA";
            case AuthorizationLevel.CEO:
                return "CEO";
            case AuthorizationLevel.SalesManager:
                return "SC";
            case AuthorizationLevel.SalesPerson:
                return "SP";
            default:
                return string.Empty;
        }
    }

    public string GenereateAcronym(string agentnumber, AuthorizationLevel level)
    {
        string acronym = GetAcronymForAuthorizationLevel(level);
        return $"{acronym}{agentnumber}";
    }
}
