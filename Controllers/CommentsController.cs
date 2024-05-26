﻿using API.Interfaces;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepo;

        public CommentsController(ICommentsRepository commentsRepo)
        {
            _commentsRepo = commentsRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentsRepo.GetAllAsync();
            var commentsDto = comments.Select(s => s.ToCommentsDto());

            return Ok(commentsDto);
        }
    }
}
