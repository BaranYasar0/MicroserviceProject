﻿using FreeCourse.Web.Models.ViewModels.Catalog;

namespace FreeCourse.Web.Models.Inputs.Catalog
{
    public class CourseCreateInput
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        public string CategoryId { get; set; }
    }
}