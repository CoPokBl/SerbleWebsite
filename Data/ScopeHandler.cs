namespace SerbleWebsite.Data; 

public static class ScopeHandler {
    
    // Scope Format
    // Scope will be a string of 1s and 0s where 1 is a granted scope and 0 is a denied scope.
    // In the following order (Format: INDEX. FULL NAME | IDENTIFIER)
    // 0. Full Access | full_access
    // 1. File Host Access | file_host
    
    private static string[] _scopes = {
        "full_access",
        "file_host"
    };
    
    private static string[] _scopeNames = {
        "Full Account Access",
        "File Host"
    };

    public static string ListOfScopeIdsToString(IEnumerable<string> scopeIds) {
        return _scopes.Aggregate("", (current, scope) => current + (scopeIds.Contains(scope) ? "1" : "0"));
    }
    
    // Convert list of scope ids to list of scope names
    public static IEnumerable<string> ListOfScopeIdsToScopeNames(IEnumerable<string> scopeIds) {
        return _scopeNames.Where((_, index) => scopeIds.Contains(_scopes[index]));
    }
    
}