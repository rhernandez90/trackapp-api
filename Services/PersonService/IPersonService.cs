using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Services.PersonService.Dto;

namespace WebApi.Services.PersonService
{
    public interface IPersonService
    {
        Task<RequestResponseDto> Create(Dto.PersonDto taksData);
        Task<RequestResponseDto> GetAll();
        Task<RequestResponseDto> GetById(int id);
        Task<RequestResponseDto> Delete(int id);
        Task<RequestResponseDto> Update(int id, Dto.PersonDto taskData);
    }
}
