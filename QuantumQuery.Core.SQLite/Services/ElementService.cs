﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuantumQuery.Core.SQLite.DTOs;
using QuantumQuery.Core.SQLite.Interfaces;
using QuantumQuery.DAL.SQLite.Context;
using QuantumQuery.DAL.SQLite.Models;

namespace QuantumQuery.Core.SQLite.Services
{
	public class ElementService : IService<ElementDto, ElementDto>
	{
		private readonly QuantumQueryDbContext _quantumQueryDbContext;
		private readonly IMapper _mapper;

		public ElementService(QuantumQueryDbContext quantumQueryDbContext, IMapper mapper)
		{
			_quantumQueryDbContext = quantumQueryDbContext;
			_mapper = mapper;
		}

		public async Task<DeleteDto> DeleteById(Guid id)
		{
			var model = await _quantumQueryDbContext.Elements
				.FirstOrDefaultAsync(x => x.Id == id.ToString());

			var dto = new DeleteDto();

			if (model == null)
			{
				dto.Id = id;
				dto.IsDeleted = false;
				return dto;
			}

			//model.IsActive = false;
			//_startUpDbContext.Update(model);

			_quantumQueryDbContext.Elements.Remove(model);
			await _quantumQueryDbContext.SaveChangesAsync();

			dto.Id = id;
			dto.IsDeleted = true;

			return dto;
		}

		public async Task<List<ElementDto>> GetAll()
		{
			var model = await _quantumQueryDbContext.Elements
				.ToListAsync();

			return _mapper.Map<List<ElementDto>>(model);
		}

		public async Task<ElementDto> GetById(Guid id)
		{
			var model = await _quantumQueryDbContext.Elements
				.FirstOrDefaultAsync(x => x.Id == id.ToString());

			return _mapper.Map<ElementDto>(model);
		}

		public async Task<ElementDto> UpdateOrInsert(ElementDto obj)
		{
			var model = _mapper.Map<Element>(obj);

			if (model.Id == string.Empty || model.Id == " ")
			{
				model.Id = Guid.NewGuid().ToString();
			}

			var existingModel = await _quantumQueryDbContext.Elements
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == model.Id);

			if (existingModel == null)
			{
				_quantumQueryDbContext.Elements.Add(model);
			}
			else
			{
				_quantumQueryDbContext.Elements.Update(model);
			}

			await _quantumQueryDbContext.SaveChangesAsync();

			return _mapper.Map<ElementDto>(model);
		}
	}
}