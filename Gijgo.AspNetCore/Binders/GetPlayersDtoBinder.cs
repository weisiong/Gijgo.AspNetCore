using Gijgo.AspNetCore.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Gijgo.AspNetCore.Binders
{
    public class GetPlayersDtoBinder : IModelBinder
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
            var sortBy = new StringValues();
            formReq.TryGetValue("sortBy", out sortBy);
            var direction = new StringValues();
            formReq.TryGetValue("direction", out direction);
            var name = new StringValues();
            formReq.TryGetValue("name", out name);
            var nationality = new StringValues();
            formReq.TryGetValue("nationality", out nationality);
            var placeOfBirth = new StringValues();
            formReq.TryGetValue("placeOfBirth", out placeOfBirth);

            var result = new GetPlayersDto()
            {
                page = Convert.ToInt16(page),
                limit = Convert.ToInt16(limit),
                sortBy = sortBy.ToString(),
                direction = direction.ToString(),
                name = name.ToString(),
                nationality = nationality.ToString(),
                placeOfBirth = placeOfBirth.ToString()
            };
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
