using AutoMapper;
using FinancialControl.API.ViewModels;
using FinancialControl.Application.Dtos;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinancialControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService, IMapper mapper) : base(mapper)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("create-transaction")]
        [ProducesResponseType(typeof(TransactionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TransactionRequest transactionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _transactionService.AddTransactionAsync(transactionDto);

            var viewModel = _mapper.Map<TransactionViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TransactionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var response = await _transactionService.GetAllAsync();
            if (response == null) return NotFound();

            var viewModel = _mapper.Map<IEnumerable<TransactionViewModel>>(response);

            return CustomResponse(viewModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TransactionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _transactionService.GetByIdAsync(id);
            if (response == null) return NotFound();      

            var viewModel = _mapper.Map<TransactionViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("daily-daily")]
        public async Task<IActionResult> GetDailyBalance([FromQuery] DateTime date)
        {
            try
            {
                var response = await _transactionService.GetDailyBalanceAsync(date);                

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("negative-days")]
        [ProducesResponseType(typeof(DailyBalanceViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNegativeBalanceDays()
        {
            var response = await _transactionService.GetNegativeBalanceDaysAsync();

            if (response == null) return NotFound();

            var viewModel = _mapper.Map<DailyBalanceViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpPost]
        [Route("add-transaction")]
        [ProducesResponseType(typeof(TransactionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionRequest transactionDto)
        {
            var response = await _transactionService.AddTransactionAsync(transactionDto);

            if (response == null) return NotFound();

            var viewModel = _mapper.Map<TransactionViewModel>(response);

            return CustomResponse(viewModel);
        }

        [HttpPut]
        [Route("update-transaction")]
        [ProducesResponseType(typeof(TransactionViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionRequest transactionDto)
        {
            var response = await _transactionService.UpdateTransactionAsync(transactionDto);

            if (response == null) return NotFound();

            var viewModel = _mapper.Map<TransactionViewModel>(response);

            return CustomResponse(viewModel);
        }
    }
}
