using Museum.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Museum.Application.Interfaces
{
    // Интерфейс репозитория для выставок
    public interface IExhibitionRepository
    {
        Task<List<Exhibition>> GetAllAsync();
        Task<Exhibition?> GetByIdAsync(int id);
    }
}
