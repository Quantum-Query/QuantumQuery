﻿using LiteDB;
using QuantumQuery.Core.LiteDB.Interfaces;

namespace QuantumQuery.Core.LiteDB.Services.BaseService
{
	public class BaseService<T> : IServices<T> where T : class
	{
		private readonly string _databasePath;

		public BaseService(string databasePath)
		{
			_databasePath = databasePath;
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return new List<T>(collection.FindAll());
				}
			});
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return collection.FindById(new BsonValue(id));
				}
			});
		}

		public async Task<bool> UpdateAsync(T item)
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return collection.Update(item);
				}
			});
		}

		public async Task<BsonValue> InsertAsync(T item)
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return collection.Insert(item);
				}
			});
		}

		public async Task<bool> UpdateOrInsertAsync(T item)
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return collection.Upsert(item);
				}
			});
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await Task.Run(() =>
			{
				using (var db = new LiteDatabase(_databasePath))
				{
					var collection = db.GetCollection<T>();
					return collection.Delete(new BsonValue(id));
				}
			});
		}
	}
}