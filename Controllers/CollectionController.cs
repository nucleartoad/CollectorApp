using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollectionController : ControllerBase
	{
		private static CollectionService _service;
		public CollectionController(CollectionService service)
		{
			_service = service;
		}

		// get collections
		[HttpGet]
		public async Task<ActionResult<List<Collection>>> GetCollections()
		{
			var collections = await _service.GetCollections();
			if(collections == null) 
			{
				return NotFound();
			}

			return collections;
		}

		// get collection
		[HttpGet("{id}")]
		public async Task<ActionResult<Collection>> GetCollection(int id)
		{
			var collection = await _service.GetCollection(id);
			if(collection == null)
			{
				return NotFound();
			}

			return collection;
		}

		// create collection
		[HttpPost]
		public async Task<IActionResult> CreateCollection(Collection collection)
		{
			var newCollection = await _service.CreateCollection(collection);
			return CreatedAtAction(nameof(GetCollection), new { id = newCollection.Id }, newCollection);
		}

		// update collection
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCollection(int id, Collection collection)
		{
			if (id != collection.Id)
			{
				return BadRequest();
			}

			try
			{
				await _service.UpdateCollection(collection);
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}

			return NoContent();
		}

		// delete collection
		[HttpDelete]
		public async Task<IActionResult> DeleteCollection(int id)
		{
			await _service.DeleteCollection(id);
			return NoContent();
		}
	}
}