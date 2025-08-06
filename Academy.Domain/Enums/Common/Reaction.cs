using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Common;

public enum ReactionType:byte
{
    [Display(Name = "لایک")]
    Like,

    [Display(Name = "دیس لایک")]
    DisLike
}