using Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

using Models.Dtos;

namespace Services
{
	public class ItemService
	{
		private static AppDbContext _context;
		public ItemService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Item> GetItem(int itemId) {
			Thread.Sleep(1000);
			Item item = await _context.Items.FirstOrDefaultAsync( e => e.Id == itemId );
			return item;
		}

		public async Task<List<Item>> GetItemsFromCollection(string collectionId)
		{
			Thread.Sleep(1000);
			List<Item> items = await _context.Items.Where( e => e.CollectionId == collectionId).ToListAsync();
			return items;
		}

		public async Task<Item> AddItem(ItemDto itemDto)
		{
			Item item = new Item() {
				Name= itemDto.Name,
				Description= itemDto.Description,
				Value= itemDto.Value,
				CollectionId= itemDto.collectionId
			};

			_context.Items.Add(item);
			await _context.SaveChangesAsync();
			return item;
		}

		public async Task UpdateItem(Item item)
		{
			_context.Entry(item).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteItem(int itemId)
		{
			var item = await _context.Items.FindAsync(itemId);
			_context.Items.Remove(item);
			await _context.SaveChangesAsync();
		}
	}
}