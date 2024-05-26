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
                StockId = commentsModel.StockId,
            };
        }
    }
}
