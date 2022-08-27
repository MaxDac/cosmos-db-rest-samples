using System.ComponentModel.DataAnnotations;

namespace CosmosDbRestSamples;

public class Configuration {
    public string? AccountName { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Key { get; set; } = "";
    
    [Required(AllowEmptyStrings = false)]
    public string Host { get; set; } = "";
}