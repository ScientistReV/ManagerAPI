using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Services.Interfaces;
using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO;
using Manager.Infra.Interfaces;
using Manager.Core.Exceptions;
using EscNet.Cryptography.Interfaces;

namespace Manager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        private readonly IRijndaelCryptography _rijndaelCryptography;
        public UserService(IUserRepository userRepository, IMapper mapper, IRijndaelCryptography rijndaelCryptography)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _rijndaelCryptography = rijndaelCryptography;
        }

        public UserService()
        {

        }
        public async Task<UserDTO> Get(long id)
        {
            var user = await _userRepository.Get(id);

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<List<UserDTO>> GetAll()
        {
            var allUsers = await _userRepository.GetAll();

            return _mapper.Map<List<UserDTO>>(allUsers);
        }
        public async Task<UserDTO> Create(UserDTO userDTO)
        {
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if (userExists != null)
            {
                throw new DomainException("User already exists");
            }

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            user.ChangePassword(_rijndaelCryptography.Encrypt(user.Password));

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }
        public async Task<UserDTO> Update(UserDTO userDTO)
        {
            var userExists = await _userRepository.Get(userDTO.Id);

            if (userExists == null)
            {
                throw new DomainException("User does not exists");
            }

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            user.ChangePassword(_rijndaelCryptography.Encrypt(user.Password));

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }
        public async Task Remove(long id)
        {
            await _userRepository.Remove(id);
        }
        public async Task<List<UserDTO>> SearchByName(string name)
        {
            var users = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDTO>>(users);
        }
        public async Task<List<UserDTO>> SearchByEmail(string email)
        {
            var users = await _userRepository.SearchByEmail(email);
        
            return _mapper.Map<List<UserDTO>>(users);
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDTO>(user);
        }
        
    }
}