using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Baskets;
using Store.Shared.Dtos.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string basketId)
        {
            
            var result = await serviceManager.BasketService.GetBasketAsync(basketId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(BasketDto basketDto)
        {

            var result = await serviceManager.BasketService.CreateUpdataBasketAsync(basketDto,TimeSpan.FromDays(1));

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketById(string basketId)
        {

            var result = await serviceManager.BasketService.DeleteBasketAsync(basketId);

            return NoContent();//204
        }
    }
}
