using AutoMapper;
using Lenceas.Core.Model;

namespace Lenceas.Core.Extensions.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<Test, TestWebModel>();
            CreateMap<Test, TestEditWebModel>();
            CreateMap<TestWebModel, Test>();
            CreateMap<TestEditWebModel, Test>();
            CreateMap<User, UserWebModel>();
            CreateMap<User, UserEditWebModel>();
            CreateMap<UserWebModel, User>();
            CreateMap<UserEditWebModel, User>();
            CreateMap<Menu, MenuWebModel>();
            CreateMap<Menu, MenuEditWebModel>();
            CreateMap<UserWebModel, Menu>();
            CreateMap<MenuEditWebModel, Menu>();
        }
    }
}
