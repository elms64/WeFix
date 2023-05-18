using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models;

public class Part
{
    //Declaring attributes for all car parts
    public int ID { get; set; }

    //Validation rules and appropriate display names are defined
    [StringLength(100, MinimumLength = 3), Required]
    [Display(Name = "Part Name")]
    public string? PartName { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Release Date")]
    public DateTime ReleaseDate { get; set; }

    [StringLength(100, MinimumLength = 3), Required]
    [Display(Name = "Car Model")]
    public string? CarModel { get; set; }

    [DataType(DataType.Currency), Range(1, 100), Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Range(0, 2000)]
    [Display(Name = "Stock")]
    public int StockQuantity { get; set; }

}