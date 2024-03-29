﻿using FluentValidation;
using FreeCourse.Web.Models.Inputs.Catalog;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("açıklama boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2,6).WithMessage("hatalı format");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage("Süre boş olamaz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçilmeli");
        }
    }
}
