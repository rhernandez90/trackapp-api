using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services.PersonService.Dto;

namespace WebApi.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PersonService(
            DataContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<RequestResponseDto> Create(PersonDto personData)
        {
            var person = _mapper.Map<Persons>(personData);
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return new RequestResponseDto { Key = person.Id, Data = person };
        }

        public async Task<RequestResponseDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResponseDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResponseDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResponseDto> Update(int id, PersonDto taskData)
        {
            throw new NotImplementedException();
        }
    }
}
