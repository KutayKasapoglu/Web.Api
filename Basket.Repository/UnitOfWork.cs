
using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Repository
{
	// Unit Of Work design pattern ile aksiyonun anlık olarak veritabanına aktarılmasını engelleyip,
	// transaction üzerinden toplu işlemler yapılması sağlanmıştır
	// Böylece hem veritabanı maliyetleri düşürülevektir hem de bağlı tabloların güncellemesinde oluşabilecek hataların kontrolü optimum düzeyde sağlanacaktır

	public class UnitOfWork : IUnitOfWork
	{
		private DbContext _dbContext;

		public DbContext DbContext => this._dbContext;

		public UnitOfWork(DbContext dbContext)
		{
			this._dbContext = dbContext;
		}

		public void SaveChanges()
		{
			_dbContext.SaveChanges();
		}

		public Task<int> SaveChangesAsync()
		{
			return _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{

		}
	}
}
