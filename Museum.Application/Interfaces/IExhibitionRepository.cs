using Museum.Application.DTOs;
using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.Interfaces
{
    // Интерфейс репозитория для выставок
    public interface IExhibitionRepository
    {
        Task<IEnumerable<Exhibition>> GetAllAsync();
        Task<IEnumerable<Exhibition>> GetFilteredAsync(ExhibitionFilterDto filter);
    }
}
