using Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
	public class CollectionService
	{
		private static AppDbContext _context;
		public CollectionService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Collection>> GetCollections()
		{
			var collections = await _context.Collections.Include( e => e.Items ).ToListAsync();
			return collections;
		}

		public async Task<Collection> GetCollection(int id)
		{
			var collection = await _context.Collections.Include( e => e.Items ).FirstOrDefaultAsync( e => e.Id == id );
			return collection;
		}

		public async Task<Collection> CreateCollection(Collection collection)
		{
			_context.Add(collection);
			await _context.SaveChangesAsync();
			return collection;
		}

		public async Task UpdateCollection(Collection collection)
		{
			_context.Entry(collection).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCollection(int id)
		{
			var collection = await _context.Collections.FindAsync(id);
			_context.Collections.Remove(collection);
			await _context.SaveChangesAsync();
		}
	}
}