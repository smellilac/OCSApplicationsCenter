public class ApplicationsModel
{
    public Guid Author { get; set; } 

    public Guid Id { get; set; }  
    
    [Required]
    [MaxLength(100, ErrorMessage = "Length must be less then 100 characters")]
    public string Name { get; set; }   

    [MaxLength(300, ErrorMessage = "Length must be less then 300 characters")]
    public string Description { get; set; }

    [Required]
    [JsonConverter(typeof(ActivityConverter))]
    public Activity Activity { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Length must be less then 1000 characters")]
    public string Outline { get; set; }

    public DateTime FirstTimeCreated { get; set; }

    public DateTime Submited { get; set; }
    
}
