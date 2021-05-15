using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
                return new RequestResponseDto { Key = id, Message = "Removed" };
            }
            return null;
        }

        public async Task<RequestResponseDto> GetAll()
        {
            var person = _context.Persons;
            var personList = _mapper.Map<List<PersonDto>>(person);
            return new RequestResponseDto { Data = personList };
        }

        public async Task<RequestResponseDto> GetById(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                return null;

            return new RequestResponseDto { Data = person };
        }

        public async Task<RequestResponseDto> Update(int id, PersonDto personData)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                person = _mapper.Map<Persons>(personData);
                _context.Persons.Update(person);
                await _context.SaveChangesAsync();

                return new RequestResponseDto { Key = person.Id, Data = person };
            }
            return null;
        }
    }
}
