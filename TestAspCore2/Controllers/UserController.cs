namespace TestAspCore2.Controllers
{
    using System.Collections.Generic;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using TestAspCore2.Repositories;
    using TestAspCore2.ViewModels;

    using Wavenet.Api.ViewModels;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;

        public UserController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return this.mapper.Map<IEnumerable<UserViewModel>>(new UserRepository().GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public UserViewModel Get(IdViewModel id)
        {
            return this.mapper.Map<UserViewModel>(new UserRepository().Get(id));
        }
    }
}