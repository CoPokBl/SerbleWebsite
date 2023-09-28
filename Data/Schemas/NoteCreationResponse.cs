using System.Text.Json.Serialization;

namespace SerbleWebsite.Data.Schemas; 

public class NoteCreationResponse {
    [JsonPropertyName("note_id")]
    public string? NoteId { get; set; }
}