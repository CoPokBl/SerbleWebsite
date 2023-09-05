namespace SerbleWebsite.Data.Schemas; 

public class PatchEditRequest {

    public string Field { get; set; }
    public string NewValue { get; set; }
    
    public PatchEditRequest(string field, string newValue) {
        Field = field;
        NewValue = newValue;
    }

}