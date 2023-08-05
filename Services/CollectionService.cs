using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;

namespace Services
{
	public class CollectionService
	{
		private static AppDbContext _context;
		public CollectionService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Collection>> GetCollections(string Username)
		{
			var collections = await _context.Collections.Where(e => e.Username == Username).ToListAsync();
			return collections;
		}

		public async Task<Collection> GetCollection(int id)
		{
			var collection = await _context.Collections.FirstOrDefaultAsync( e => e.Id == id );
			return collection;
		}

		public async Task<Collection> CreateCollection(CreateCollectionDto collectionDto, string username)
		{
			Collection collection = new Collection() {
				Name = collectionDto.Name,
				Username = username,
				Description = collectionDto.Description
			};

			await _context.AddAsync(collection);
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