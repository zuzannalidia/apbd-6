using System.ComponentModel.DataAnnotations;

namespace RestApiApbdPjatkCw4.Models.DTOs;


/*Jeśli dane będą niezgodne z modelem należy zwrócić kod błędu HTTP 400.
 ROBIENIE TEGO PRZEZ DTO AUTOMATYCZNIE NAM TO GWARANTUJE!*/
public class AddAnimal
{
    /*To samo co w klasie Animal -> tylko dodajemy które z pól jest wymagane! */
    [Required]
    public int ID { get; set; }
    
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