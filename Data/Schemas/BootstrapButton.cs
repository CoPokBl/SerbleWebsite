namespace SerbleWebsite.Data.Schemas; 

public class BootstrapButton {
    public BootstrapColor Color { get; set; }
    public string Text { get; set; }
    public Func<Task> OnClick { get; set; }
    public bool CloseModal { get; set; }
}