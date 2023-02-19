using FluentValidation;
using FreeCourse.Web.Models.Inputs.Discount;

namespace FreeCourse.Web.Validators
{
    public class DiscountApplyInputValidator:AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İnidirim kupon alanı boş olamaz");
        }
    }
}
