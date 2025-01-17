﻿using Domin.Models;

using Infrastructure.DTO;
using Infrastructure.DTO.Owner;
using Infrastructure.DTO.Student;
using Microsoft.AspNetCore.Http;

namespace Services.Abstracts
{
    public interface IAuthService
    {

        Task<UserDTO> RegisterAsync(RegisterDTO model);
        Task<UserDTO> Login(LoginDTO model);
        Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token);
        Task<UserManagerResponseDTO> ForgetPassword(string email);
        Task<UserManagerResponseDTO> ConfirmForgetPassword(string email, string code);
        Task<UserManagerResponseDTO> ResetPassword(string email, string Password, string ConfrimPassword);
        Task<UserManagerResponseDTO> ChangePassword(string email, string oldPassword, string newPassword);
        Task<bool> UserISExist(string userId);
        Task<bool> UserAndApartmentISExist(string userId, int ApartmentID);
        bool NameIsExist(string name, string email);
        Task<string> UpdateProfile(ApplicationUser user, IFormFile? img, string requestSchema, HostString hostString);
        Task<ReturnedUserDataDto> GetUserData(string ViewerID, string ViewedID);
        Task<List<GetAllUsersResponse>> GetAllUsers(string? search,string? Role);
        Task<string> DeleteUser(string userId);
        Task<string> GetUserMaxRole(string userId);

        Task<List<ApartmentRequestsResponse>> GetStudentRequestApartments(string OwnerId);
        IQueryable returnApartmentRequestsResponseAsQueryable(List<ApartmentRequestsResponse> list);

        Task<string> AssignStudentToApartment(string UserId, int ApartmentId);
        Task<ApplicationUser> GetUserById(string id);
        //Task<List<GetOwnerStudentsResponse>> GetOwnerStudents(string ownerId);
        Task<List<GetOwnerStudentsResponse>> GetOwnerStudentsResponses(List<UsersApartments> lop);

    }
}
