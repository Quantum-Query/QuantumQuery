﻿using Moleculab.Core.Extensions;
using Moleculab.Core.SQLite.DTOs;
using Moleculab.Core.SQLite.Interfaces;
using Moleculab.Math.Interfaces.Calculators.GasDensity;

namespace Moleculab.Math.Calculators.GasDensity
{
	public class GasDensityCompoundCalculator : IGasDensityCompoundCalculator
	{
		private readonly IElementService _elementService;

		private Compound _compound { get; set; }
		private int _quantity { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public GasDensityCompoundCalculator()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		{
			_elementService = ServiceLocator.GetService<IElementService>();
		}

		public GasDensityCompoundCalculator(Compound compound, int quantity)
		{
			_compound = compound;
			_quantity = quantity;
			_elementService = ServiceLocator.GetService<IElementService>();
		}

		public void AddOfCompound(Compound compound, int quantity)
		{
			try
			{
				_compound = compound;
				_quantity = quantity;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public float GetEquals(Compound compound)
		{
			var atomicMassOfDelement = _compound.CalculateMolecularWeight() * _quantity;
			return compound.CalculateMolecularWeight() / atomicMassOfDelement;
		}

		public async Task<ElementDto> GetElementAsync()
		{
			try
			{
				var atomicMassOfElement = _compound.CalculateMolecularWeight() * _quantity;

				return await _elementService.GetByAtomicMassAsync((int)atomicMassOfElement);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}