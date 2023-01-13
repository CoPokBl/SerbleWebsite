namespace SerbleWebsite.Data; 

public static class SerbleValueNamesHandler {
    
    public static string GetNameOfPremiumLevel(int premiumLevel) {
        Localiser localiser = new();
        return premiumLevel switch {
            0 => localiser["account-type-free"],
            10 => localiser["account-type-premium"],
            _ => localiser["unknown"]
        };
    }

}