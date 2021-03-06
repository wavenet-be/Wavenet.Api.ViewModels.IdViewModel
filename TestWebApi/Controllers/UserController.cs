﻿namespace TestWebApi.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using AutoMapper;

    using TestWebApi.Models;
    using TestWebApi.Repositories;
    using TestWebApi.ViewModels;

    using Wavenet.Api.ViewModels;

    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        private readonly IMapper mapper;

        public UserController()
        {
            this.mapper = new MapperConfiguration(m => m.CreateMap<User, UserViewModel>().ReverseMap()).CreateMapper();
        }

        [HttpGet]
        [Route]
        public IEnumerable<UserViewModel> Get()
        {
            return this.mapper.Map<IEnumerable<UserViewModel>>(new UserRepository().GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public UserViewModel Get(IdViewModel id)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpResponseException(this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState));
            }

            return this.mapper.Map<UserViewModel>(new UserRepository().Get(id));
        }
    }
}