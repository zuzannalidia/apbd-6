using System.ComponentModel.DataAnnotations;

namespace RestApiApbdPjatkCw4.Models.DTOs;

public class UpdateAnimal
{
    /*To samo co w klasie UpdateAnimal -> tylko bez ID! */
    [Required]
    /* MaxLength 200 bo data type dla stringów to nvarchar(200)*/
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    /*? -> wartość jest null'owalna - nie dostaje [Required] */
    public string? Description { get; set; } 
    
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Area { get; set; } = string.Empty;
}