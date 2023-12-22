using Mvp24Hours.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Priority;
using Xunit;
using System.Reflection;
using Mvp24Hours.Core.Contract.Mappings;
using AutoMapper;
using Mvp24Hours.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Helpers;
using System.Threading.Tasks;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class MapProfileTest
    {
        public class TestAClass
        {
            public int MyProperty { get; set; }
        }

        public class TestBClass : IMapFrom
        {
            public int MyProperty { get; set; }
            public void Mapping(Profile profile)
            {
                profile.CreateMap<TestAClass, TestBClass>();
            }
        }

        [Fact]
        public async Task TestAsync()
        {
            var local = Assembly.GetExecutingAssembly();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile(local));
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var services = new ServiceCollection();
            services.AddSingleton(mapper);
            ServiceProviderHelper.SetProvider(services.BuildServiceProvider());

            var classA = new TestAClass { MyProperty = 1 };
            var classB = classA.MapTo<TestBClass>();
            Assert.True(classB != null && classB.MyProperty == 1);
        }
    }
}
