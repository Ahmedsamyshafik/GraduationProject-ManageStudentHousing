﻿using AutoMapper;
using Core.Bases;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Commands.Results;
using Domin.Constant;
using Domin.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Services.Abstracts;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Handlers
{
    public class ApartmentCommandHandler : ResponseHandler,
                    IRequestHandler<AddApartmentCommand, Response<AddApartmentResponse>>,
                    IRequestHandler<AddCommentApartmentCommand, Response<string>>,
                    IRequestHandler<AddReactApartmentCommand, Response<string>>,
                    IRequestHandler<PendingApartmentAction, Response<string>>,
                    IRequestHandler<DeleteApartmentCommand, Response<string>>,
                    IRequestHandler<EditApartmentCommand, Response<string>>
    {
        #region Fields
        private readonly IApartmentServices _apartmentServices;
        private readonly IMapper _mapper;
        private readonly IUploadingMedia _media;
        private readonly IVideosServices _videos;
        private readonly IImagesServices _images;
        private readonly IRoyalServices _royal;
        private readonly ICommentServices _comment;
        private readonly IReactServices _react;
        private readonly IUsersApartmentsServices _usersApartments;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Ctor
        public ApartmentCommandHandler(IApartmentServices apartmentServices, IMapper mapper, IUploadingMedia media
                    , IVideosServices videos, IImagesServices images, IRoyalServices royal, UserManager<ApplicationUser> userManager
                   , ICommentServices comment, IReactServices react, IUsersApartmentsServices usersApartments, IStringLocalizer<SharedResources> localizer)
        {
            _apartmentServices = apartmentServices;
            _mapper = mapper;
            _media = media;
            _videos = videos;
            _images = images;
            _royal = royal;
            _userManager = userManager;
            _comment = comment;
            _react = react;
            _usersApartments = usersApartments;
            _localizer = localizer;
        }
        #endregion


        #region Handle Functions

        public async Task<Response<AddApartmentResponse>> Handle(AddApartmentCommand request, CancellationToken cancellationToken)
        {
            //valid userID!
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.UserNotFound]);
            if (!user.EmailConfirmed) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.NotConfirmed]);
            //mapping from AddApartmentCommand To Apartment
            var mapper = _mapper.Map<Apartment>(request);
            //Service To Add Apartment
            try
            {
                // Adding Apartment to get (ApartmentID)
                var apart = await _apartmentServices.AddApartmentAsync(mapper);
                //Services To Save this imges and videos..
                if (request.video != null)
                {
                    var videoPath = await _media.SavingImage(request.video, request.RequestScheme, request.Requesthost, Constants.ApartmentVids);
                    if (!videoPath.success) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.FaildSaveVid]);
                    var x = await _videos.AddVideo(videoPath.message, apart.Id, videoPath.name);
                    apart.ApartmentVideoID = x.Id;
                }
                if (request.Pics != null)
                {

                    foreach (var Pic in request.Pics)
                    {
                        if (Pic.Length > 0)
                        {
                            var PicsPaths = await _media.SavingImage(Pic, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                            if (!PicsPaths.success) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.FaildInImages]);
                            await _images.AddImage(PicsPaths.message, apart.Id, PicsPaths.name);
                        }
                    }
                    // Cover!
                    if (request.CoverImage != null)
                    {
                        var CoverImage = await _media.SavingImage(request.CoverImage, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                        if (!CoverImage.success) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.FaildSaveCoverImg]);
                        apart.CoverImageName = CoverImage.name;
                        apart.CoverImageUrl = CoverImage.message;
                    }
                }
                if (request.RoyalDocument != null)
                {
                    var RoyalPath = await _media.SavingImage(request.RoyalDocument, request.RequestScheme, request.Requesthost, Constants.ApartmentRoyalDocPics);
                    if (!RoyalPath.success) return BadRequest<AddApartmentResponse>(_localizer[SharedResourcesKeys.FaildSaveRoyal]);
                    var x = await _royal.AddRoyal(RoyalPath.message, apart.Id, RoyalPath.name);
                    apart.RoyalDocumentID = x.Id;
                }
                await _apartmentServices.UpdateApartmentAsync(apart);
                var returned = new AddApartmentResponse() { id = apart.Id };
                return Success(returned);
            }
            catch (Exception ex)
            {
                return BadRequest<AddApartmentResponse>(ex.Message);
            }
        }

        public async Task<Response<string>> Handle(AddCommentApartmentCommand request, CancellationToken cancellationToken)
        {
            //validation userid-apartmentid
            var user = await _userManager.FindByIdAsync(request.UserID);
            if (user == null) return BadRequest<string>(_localizer[SharedResourcesKeys.UserNotFound]);
            if (!user.EmailConfirmed) return BadRequest<string>(_localizer[SharedResourcesKeys.NotConfirmed]);

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>(_localizer[SharedResourcesKeys.ApartmentNotFound]);
            //mapping
            var comment = new UserApartmentsComment { ApartmentId = request.ApartmentID, Comment = request.Comment, UserId = request.UserID };
            //adding
            //check abilite to add?  
            var checkVar = await _comment.CanCommentOrNo(request.UserID, request.ApartmentID, 3);
            if (checkVar)
            {
                // Add To Notification Table..
                var result = await _comment.AddCommentAsync(comment);
                //return
                if (result == _localizer[SharedResourcesKeys.Success]) return Success("");
                return BadRequest<string>("");
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.CannotComment]);

        }

        public async Task<Response<string>> Handle(AddReactApartmentCommand request, CancellationToken cancellationToken)
        {
            //validation userid-apartmentid
            var user = await _userManager.FindByIdAsync(request.UserID);
            if (user == null) return BadRequest<string>(_localizer[SharedResourcesKeys.UserNotFound]);
            if (!user.EmailConfirmed) return BadRequest<string>(_localizer[SharedResourcesKeys.NotConfirmed]);

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>(_localizer[SharedResourcesKeys.ApartmentNotFound]);
            //mapping
            var react = new UserApartmentsReact { ApartmentId = request.ApartmentID, UserId = request.UserID };
            //adding
            //check abilite to add?  
            var checkVar = await _react.CanReactOrNo(request.UserID, request.ApartmentID);
            if (checkVar)
            { // Add To Notification Table..
                var result = await _react.AddReactAsync(react);
                if (result == _localizer[SharedResourcesKeys.Success])
                {
                    apartment.Likes++;
                    await _apartmentServices.UpdateApartmentAsync(apartment);
                    return Success("");
                }
                return BadRequest<string>("");
            }
            return BadRequest<string>(_localizer[SharedResourcesKeys.CannotReact]);


        }

        public async Task<Response<string>> Handle(PendingApartmentAction request, CancellationToken cancellationToken)
        {
            //valid apartment id

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>(_localizer[SharedResourcesKeys.ApartmentNotFound]);
            //do operation in service + Delete images if deleted!
            await _apartmentServices.HandlePendingApartments(request.Accept, apartment);
            return Success("");
        }

        public async Task<Response<string>> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
        {
            //valid id?
            var apartment = await _apartmentServices.GetApartment(request.ApartmentId);
            if (apartment == null) return NotFound<string>(_localizer[SharedResourcesKeys.ApartmentNotFound]);
            if (apartment.OwnerId != request.userID) return BadRequest<string>(_localizer[SharedResourcesKeys.ApartmentToWrongOwner]);
            //Any Studnets?
            bool ExistStudent = _usersApartments.AnyStudnets(request.ApartmentId);
            if (ExistStudent) return BadRequest<string>(_localizer[SharedResourcesKeys.ExistStudents]);
            //Delete Files
            await _apartmentServices.DeleteApartmentFilesOnly(apartment);

            await _apartmentServices.DeleteApartmentAsync(apartment);
            //return Response
            return Deleted<string>("");
        }

        public async Task<Response<string>> Handle(EditApartmentCommand request, CancellationToken cancellationToken)
        {
            //mapping 
            //from EditApartmentCommand => Apartment
            var map = _mapper.Map<Apartment>(request);

            //Service
            var result = await _apartmentServices.EditApartment(map, request.NewCoverImage, request.NewVideo, request.NewPics, request.ApartmentsImagesUrl, request.RequestScheme, request.Requesthost);
            if (result == _localizer[SharedResourcesKeys.Success]) return Success("");
            return BadRequest<string>(result);
        }
        #endregion



    }
}
