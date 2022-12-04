using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Manager.API.ViewModels;
using Manager.API.Utilities;
using AutoMapper;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Manager.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/users")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(new ResultViewModel
                {
                    Message = "Users successfully listed",
                    Success = true,
                    Data = users
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/users/{id}")]
        public async Task<ActionResult> Get(long id)
        {
            try
            {
                var user = await _userService.Get(id);

                if(user == null)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "User not found",
                        Success = true,
                        Data = user
                    });
                }
                
                return Ok(new ResultViewModel
                {
                    Message = "User found with success",
                    Success = true,
                    Data = user
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/users/get-by-email")]
        public async Task<ActionResult> GetByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetByEmail(email);

                if(user == null)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "User not found with email informed",
                        Success = true,
                        Data = user
                    });
                }
                
                return Ok(new ResultViewModel
                {
                    Message = "User found with success",
                    Success = true,
                    Data = user
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/users/search-by-name")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            try
            {
                var users = await _userService.SearchByName(name);

                if(users == null)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "Users not found with name informed",
                        Success = true,
                        Data = users
                    });
                }
                
                return Ok(new ResultViewModel
                {
                    Message = "Users found with success",
                    Success = true,
                    Data = users
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/users/search-by-email")]
        public async Task<IActionResult> SearchByEmail([FromQuery] string email)
        {
            try
            {
                var users = await _userService.SearchByEmail(email);

                if(users == null)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "Users not found with email informed",
                        Success = true,
                        Data = users
                    });
                }
                
                return Ok(new ResultViewModel
                {
                    Message = "Users found with success",
                    Success = true,
                    Data = users
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/v1/users/create")]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel model)
        {
            try
            {   
                var useDTO = _mapper.Map<UserDTO>(model);

                var userCreated = await _userService.Create(useDTO);

                return Ok(new ResultViewModel {
                    Message = "User created successfully",
                    Success = true, 
                    Data = userCreated
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/v1/users/update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel model)
        {
            try
            {
                var useDTO = _mapper.Map<UserDTO>(model);

                var userUpdated = await _userService.Update(useDTO);

                return Ok(new ResultViewModel {
                    Message = "User updated successfully",
                    Success = true,
                    Data = userUpdated
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("api/v1/users/delete/{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            try
            {
                await _userService.Remove(id);

                return Ok(new ResultViewModel {
                    Message = "User deleted successfully",
                    Success = true,
                    Data = null
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}