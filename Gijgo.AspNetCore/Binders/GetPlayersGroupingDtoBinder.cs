using Gijgo.AspNetCore.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Gijgo.AspNetCore.Binders
{
    public class GetPlayersGroupingDtoBinder : IModelBinder
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
            var groupBy = new StringValues();
            formReq.TryGetValue("groupBy", out groupBy);
            var groupByDirection = new StringValues();
            formReq.TryGetValue("groupByDirection", out groupByDirection);

            var result = new GetPlayersGroupingDto()
            {
                page = Convert.ToInt16(page),
                limit = Convert.ToInt16(limit),
                groupBy = groupBy.ToString(),
                groupByDirection = groupByDirection.ToString()
            };
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }

    }
}
