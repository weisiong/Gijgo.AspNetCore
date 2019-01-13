using Gijgo.AspNetCore.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Gijgo.AspNetCore.Binders
{
    public class GetTeamsDtoBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));
            var formReq = bindingContext.HttpContext.Request.Query;

            var page = new StringValues();
            formReq.TryGetValue("page", out page);
            var limit = new StringValues();
            formReq.TryGetValue("limit", out limit);
            var playerId = new StringValues();
            formReq.TryGetValue("playerId", out playerId);

            var result = new GetTeamsDto()
            {
                page = Convert.ToInt16(page),
                limit = Convert.ToInt16(limit),
                playerId = Convert.ToInt16(playerId)
            };

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}
