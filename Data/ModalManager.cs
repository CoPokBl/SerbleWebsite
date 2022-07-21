using Microsoft.JSInterop;

namespace SerbleWebsite.Data; 

public class ModalManager {
    private readonly IJSRuntime _jsRuntime;
    
    public ModalManager(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowModal(string id) {
        await _jsRuntime.InvokeVoidAsync("eval", "$(\"" + "#" + id + "\").modal('show');");
    }
    
    public async Task HideModal(string id) {
        await _jsRuntime.InvokeVoidAsync("eval", "$(\"" + "#" + id + "\").modal('hide');");
    }
    
}