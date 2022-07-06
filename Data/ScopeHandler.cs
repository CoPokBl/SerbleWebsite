namespace SerbleWebsite.Data; 

public static class ScopeHandler {
    
    // Scope Format
    // Scope will be a string of 1s and 0s where 1 is a granted scope and 0 is a denied scope.
    // In the following order (Format: INDEX. FULL NAME | IDENTIFIER)
    // 0. Full Access | full_access
    // 1. File Host Access | file_host
    
    public static readonly string[] Scopes = {
        "full_access",
        "file_host"
    };
    
    public static readonly string[] ScopeNames = {
        "Full Account Access",
        "File Host"
    };

    // id, name
    public static List<(string, string)> ScopeList => Scopes.Select((t, i) => (t, ScopeNames[i])).ToList();

    public static string[] ScopeDescriptions = {
        "Allows full access to the account.",
        "Allows the user to access the file host."
    };

    public static string ListOfScopeIdsToString(IEnumerable<string> scopeIds) {
        return Scopes.Aggregate("", (current, scope) => current + (scopeIds.Contains(scope) ? "1" : "0"));
    }
    
    // Convert list of scope ids to list of scope names
    public static IEnumerable<string> ListOfScopeIdsToScopeNames(IEnumerable<string> scopeIds) {
        return ScopeNames.Where((_, index) => scopeIds.Contains(Scopes[index]));
    }
    
    public static string[] FilterInvalidScopes(IEnumerable<string> scopes) {
        return scopes.Where(scope => Scopes.Contains(scope)).ToArray();
    }
    
}