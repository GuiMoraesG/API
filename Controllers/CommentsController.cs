using API.Dtos.Comments;
using API.Interfaces;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepo;
        private readonly IStockRepository _stockRepo;

        public CommentsController(ICommentsRepository commentsRepo, IStockRepository stockRepo)
        {
            _commentsRepo = commentsRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentsRepo.GetAllAsync();
            var commentsDto = comments.Select(s => s.ToCommentsDto());

            return Ok(commentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentsRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentsDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentsDto createDto)
        {
            if (!await _stockRepo.StockExist(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = createDto.ToCommentsFromCreate(stockId);

            await _commentsRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentsDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentsDto updateDto)
        {
            var comments = await _commentsRepo.UpdateAsync(id, updateDto.ToCommentsFromUpdate());

            if (comments == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comments.ToCommentsDto());
        }
    }
}
