using API.Dtos.Comments;
using API.Extension;
using API.Interfaces;
using API.Mappers;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;

        public CommentsController(ICommentsRepository commentsRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentsRepo = commentsRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentsRepo.GetAllAsync();
            var commentsDto = comments.Select(s => s.ToCommentsDto());

            return Ok(commentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentsRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentsDto());
        }

        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentsDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepo.StockExist(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);
            var commentModel = createDto.ToCommentsFromCreate(stockId);

            commentModel.AppUserId = appUser.Id;

            await _commentsRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentsDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentsDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentsRepo.UpdateAsync(id, updateDto.ToCommentsFromUpdate());

            if (comments == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comments.ToCommentsDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentsRepo.DeleteAsync(id);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment);
        }
    }
}
