using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;
        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        // Get All Cards Method
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);

        }

        // Get Single Card
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return NotFound("card not found");
            }
            return Ok(card);
        }

        // Add a Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();

            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }

        // Update Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (existingcard != null)
            {
                existingcard.CardHolderName = card.CardHolderName;
                existingcard.CardNumber = card.CardNumber;
                existingcard.ExpiryYear = card.ExpiryYear;
                existingcard.ExpiryMonth = card.ExpiryMonth;
                existingcard.CVC = card.CVC;

                await cardsDbContext.SaveChangesAsync();

                return Ok(existingcard);
            }

            return NotFound("Card Not Found");

        }


        // Delete Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (existingcard != null)
            {
                cardsDbContext.Cards.Remove(existingcard);
                await cardsDbContext.SaveChangesAsync();

                return Ok(existingcard);
                
            }

            return NotFound("Card Not Found");

        }


    }
}
