public class ApplicationsModel
{
    public Guid Author { get; set; } 

    public Guid Id { get; set; }  
    
    [Required]
    [MaxLength(100, ErrorMessage = "Length must be less then 100 characters")]
    [DefaultValue("Default Name")]
    public string Name { get; set; }   

    [MaxLength(300, ErrorMessage = "Length must be less then 300 characters")]
    [DefaultValue("Default Description")]
    public string Description { get; set; }

    [Required]
    [JsonConverter(typeof(ActivityConverter))]
    public Activity? Activity { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Length must be less then 1000 characters")]
    [DefaultValue("Default Outline")]
    public string Outline { get; set; }

    [JsonIgnore]
    public DateTime FirstTimeCreated { get; set; }

    [JsonIgnore]
    public DateTime Submited { get; set; }
    
}
