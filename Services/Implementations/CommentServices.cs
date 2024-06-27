using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Services.Abstracts;
using Services.Localiz;

namespace Services.Implementations
{
    public class CommentServices : ICommentServices
    {
        #region Fields
        private readonly ICommentRepository _comment;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Ctor
        public CommentServices(ICommentRepository comment , IStringLocalizer<SharedResources> localizer)
        {
            _comment = comment;
            _localizer = localizer;
        }

        #endregion

        #region Handle Functions

        public async Task<string> AddCommentAsync(UserApartmentsComment comment)
        {
            var x = await _comment.AddAsync(comment);
            if (x != null) return _localizer[SharedResourcesKeys.Success];
            return _localizer[SharedResourcesKeys.Faild];
        }

        public async Task<bool> CanCommentOrNo(string userid, int apartmentid, int Counter)
        {
            var lista = _comment.GetTableNoTracking().Where(x => x.UserId == userid && x.ApartmentId == apartmentid);
            if (lista.Count() >= Counter) return false;
            return true;
        }

        public async Task<List<UserApartmentsComment>> GetApartmentsComments(List<int> apartmentids)
        {
            var comments = new List<UserApartmentsComment>();
            foreach (var id in apartmentids)
            {
                var comment = await _comment.GetTableNoTracking().Where(x => x.ApartmentId == id).ToListAsync();
                if (comment.Count() > 0)
                {
                    foreach (var react in comment) comments.Add(react);
                }
            }
            return comments;
        }
        public List<UserApartmentsComment> GetApartmentComment(int apartmentid)
        {
            return _comment.GetTableNoTracking().Include(x=>x.user).Where(x => x.ApartmentId == apartmentid).ToList();
        }
        #endregion


    }
}
