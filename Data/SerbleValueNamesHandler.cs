namespace SerbleWebsite.Data; 

public static class SerbleValueNamesHandler {
    
    public static string GetNameOfPremiumLevel(int premiumLevel) {
        return premiumLevel switch {
            0 => "Free",
            10 => "Premium",
            _ => "Unknown"
        };
    }

}