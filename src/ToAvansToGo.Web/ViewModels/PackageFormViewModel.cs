using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Web.ViewModels;

public class PackageFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Give the package a name.")]
    [StringLength(100, ErrorMessage = "The name can be at most 100 characters.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Meal type")]
    public MealType TypeOfMeal { get; set; }

    [Display(Name = "Pick-up from")]
    public DateTime PickUpTime { get; set; }

    [Display(Name = "Available until")]
    public DateTime AvailableTill { get; set; }

    [Range(0, 100, ErrorMessage = "The price must be between 0 and 100 euros.")]
    [Display(Name = "Price (euro)")]
    public double Price { get; set; }

    public List<int> SelectedProductIds { get; set; } = new();

    // Alleen om het formulier te vullen, wordt niet gevalideerd
    [ValidateNever] public List<Product> AllProducts { get; set; } = new();
}
