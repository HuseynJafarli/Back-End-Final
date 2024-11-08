//using System.Linq.Expressions;
//using YouPlay.Business.DTOs.CommentDTOs;
//using YouPlay.Business.Exceptions.Common;
//using YouPlay.Business.Services.Interfaces;
//using YouPlay.Core.Entities;
//using YouPlay.Core.Repositories;

//namespace YouPlay.Business.Services.Implementations
//{
//    public class CommentService : ICommentService
//    {
//        private readonly ICommentRepository commentRepository;

//        public CommentService(ICommentRepository commentRepository)
//        {
//            this.commentRepository = commentRepository;
//        }
//        public async Task CreateAsync(CommentCreateDto dto)
//        {
//            var data = _mapper.Map<Comment>(dto);

//            await commentRepository.CreateAsync(data);
//            await commentRepository.CommitAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            if (id > 0) throw new NotValidIdException();

//            var data = await commentRepository.GetByIdAsync(id);
//            if (data == null) throw new EntityNotFoundException();

//            commentRepository.DeleteAsync(data);
//            await commentRepository.CommitAsync();

//        }

//        public async Task<ICollection<CommentGetDto>> GetByExpressionAsync(Expression<Func<Comment, bool>>? expression = null, bool asNoTracking = false, params string[] includes)
//        {
            
//        }

//        public async Task<CommentGetDto> GetByIdAsync(int id)
//        {
//            if (id > 0) throw new NotValidIdException();

//            var data = await commentRepository.GetByIdAsync(id);
//            if (data == null) throw new EntityNotFoundException();

//            return _mapper.Map<CommentGetDto>(data);
//        }

//        public async Task<CommentGetDto> GetSingleByExpressionAsync(Expression<Func<Comment, bool>>? expression = null, bool asNoTracking = false, params string[] includes)
//        {
            
//        }

//        public async Task UpdateAsync(int? id, CommentUpdateDto dto)
//        {
//            if (id > 0) throw new NotValidIdException();

//            var data = await commentRepository.GetByIdAsync((int)id);

//            if (data == null) throw new EntityNotFoundException();

//            _mapper.Map(dto, data);

//            data.ModifiedDate = DateTime.Now;
//            await commentRepository.CommitAsync();
//        }
//    }
//}
