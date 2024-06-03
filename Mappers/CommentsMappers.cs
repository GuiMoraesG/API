using API.Dtos.Comments;
using API.Models;

namespace API.Mappers
{
    public static class CommentsMappers
    {
        public static CommentsDto ToCommentsDto(this Comment commentsModel)
        {
            return new CommentsDto
            {
                Id = commentsModel.Id,
                Title = commentsModel.Title,
                Content = commentsModel.Content,
                CreatedOn = commentsModel.CreatedOn,
                CreatedBy = commentsModel.AppUser.UserName,
                StockId = commentsModel.StockId,
            };
        }

        public static Comment ToCommentsFromCreate(this CreateCommentsDto createDto, int stockId)
        {
            return new Comment
            {
                Title = createDto.Title,
                Content = createDto.Content,
                StockId = stockId,
            };
        }

        public static Comment ToCommentsFromUpdate(this UpdateCommentsDto updateDto)
        {
            return new Comment
            {
                Title = updateDto.Title,
                Content = updateDto.Content,
            };
        }
    }
}
