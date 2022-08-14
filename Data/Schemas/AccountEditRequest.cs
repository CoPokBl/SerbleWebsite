using System.Text.Json.Serialization;

namespace SerbleWebsite.Data.Schemas; 

public class AccountEditRequest {

    public string Field { get; set; }
    public string NewValue { get; set; }
    
    public AccountEditRequest(string field, string newValue) {
        Field = field;
        NewValue = newValue;
    }

}