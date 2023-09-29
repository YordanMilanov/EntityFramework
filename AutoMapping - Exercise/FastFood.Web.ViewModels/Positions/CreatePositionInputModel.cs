using System.ComponentModel.DataAnnotations;
using FastFood.Common.EntityConfiguration;

namespace FastFood.Web.ViewModels.Positions
{
    public class CreatePositionInputModel
    {
        [MinLength(ViewModelsConfiguration.PositionNameMinLength)]
        [MaxLength(ViewModelsConfiguration.PositionNameMaxLength)]
        public string PositionName { get; set; } = null!;
    }
}