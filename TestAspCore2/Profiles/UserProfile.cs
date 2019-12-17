namespace TestAspCore2.Profiles
{
    using AutoMapper;
    using TestAspCore2.ViewModels;
    using TestAspCore2.Models;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}