using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FinancialControl.API.Api.Controllers;
using FinancialControl.Api.ViewModels;

namespace FinancialControl.API.Controllers
{
    [Route("")]
    [ApiController]
    public class BaseController : BaseApiController
    {
        public readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IActionResult CustomQueryResponse<V>(PagedSearchResult pagedSearchResult)
        {
            if (pagedSearchResult != null)
            {
                var viewmodel = _mapper.Map<V>(pagedSearchResult.SearchResult);

                var result = new PagedSearchViewResult
                {
                    SearchResult = viewmodel,
                    PageSize = pagedSearchResult.PageSize,
                    PageCount = pagedSearchResult.PageCount,
                    PageIndex = pagedSearchResult.PageIndex,
                    TotalRecords = pagedSearchResult.TotalRecords
                };

                return Ok(new ApiResult(true, "Success", result));
            }
            else
                return BadRequest(new ApiResult(false, "Fail", null));
        }

        protected IActionResult CustomResponse(IViewModel viewmodel)
        {
            return Ok(new ApiResult(true, "Sucesso", viewmodel));
        }
    }
}