using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace FinancialControl.API.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult ReturnBadRequest()
        {
            if (ModelState.IsValid) return null;
            var messageSB = new StringBuilder();

            var erroneousFields =
                ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Description = GetErrorsInline(x.Value.Errors)
                    });

            var itemIndex = 1;
            foreach (var item in erroneousFields)
            {
                var fieldName = item.Field;
                if (item.Field.Contains('.'))
                    fieldName = item.Field.Split('.')[1];

                messageSB.AppendLine($"{itemIndex} - {fieldName}: {item.Description}");
                itemIndex++;
            }

            return new BadRequestObjectResult(messageSB.ToString());
        }  

        protected OkObjectResult ReturnPagedSearchViewResult(PagedSearchResult pagedSearchResult, object viewModel)
        {
            var result = new PagedSearchViewResult
            {
                SearchResult = viewModel,
                PageSize = pagedSearchResult.PageSize,
                PageCount = pagedSearchResult.PageCount,
                PageIndex = pagedSearchResult.PageIndex,
                TotalRecords = pagedSearchResult.TotalRecords
            };

            return Ok(result);
        }

        private static string GetErrorsInline(ModelErrorCollection errors)
        {
            var errorList = errors.Select(c => c.ErrorMessage).ToList();
            return string.Join(",", errorList);
        }
    }
}
