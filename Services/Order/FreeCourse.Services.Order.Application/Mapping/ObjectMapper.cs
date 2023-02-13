using AutoMapper;
using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public class ObjectMapper
    {
        //normalde eger domain apide olsaydı program.cs de tanımlanarak kullanılabilirdi ama farklı bir class libraryde oldugumuz için custom olarak injection yapılması gerekiyordu o yuzden bu methodlara ihtiyaç var!
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });
            return config.CreateMapper();
        });

        //mapper cağıgrana kadar yukarıdaki kodlar çalışmayacak!
        public static IMapper Mapper => lazy.Value;
    }
}
