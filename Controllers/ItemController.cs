using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Models.Dtos;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemController : ControllerBase
	{
		private static ItemService _service;
		public ItemController(ItemService service)
		{
			_service = service;
		}
		
		// get item
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("{collectionId}/{itemId}")]
		public async Task<ActionResult<Item>> GetItem(int collectionId, int itemId)
		{
			Thread.Sleep(1000);
			Item item = await _service.GetItem(itemId);
			if(item == null)
			{
				return NotFound();
			}

			return item;
		}

		// get items
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("{collectionId}")]
		public async Task<ActionResult<List<Item>>> GetItemsFromCollection(int collectionId)
		{
			Thread.Sleep(1000);
			List<Item> items = await _service.GetItemsFromCollection(collectionId.ToString());
			if(items == null)
			{
				return NotFound();
			}

			return items;
		}

		// add item
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost]
		public async Task<IActionResult> AddItem([FromBody] ItemDto itemDto)
		{
			var newItem = await _service.AddItem(itemDto);
			// return CreatedAtAction(nameof(newItem), new { id = newItem.Id}, newItem);
			return Ok(newItem);
		}

		// update item
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateItem(int id, Item item)
		{
			if(id != item.Id)
			{
				return BadRequest();
			}

			try
			{
				await _service.UpdateItem(item);
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}

			return NoContent();
		}
		
		// delete item
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpDelete("{collectorId}/{itemId}")]
		public async Task<IActionResult> DeleteItem(int collectorId, int itemId)
		{
			await _service.DeleteItem(itemId);
			return NoContent();
		}
	}
}