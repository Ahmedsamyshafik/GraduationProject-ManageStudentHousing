﻿using Infrastructure.CustomValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Apartments.Commands.Models
{
    public class AddApartmentDTO
    {

        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Room { get; set; }
        public int NumberOfUsers { get; set; }

        public string City { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }

        [Required]
        public IFormFile RoyalDocument { get; set; }
        [MaxFileSize(1024 * 1024 * 100, ErrorMessage = "Maximum allowed video size is 100 MB.")]
        public IFormFile? video { get; set; }
        [Required]
        [MaxFileSize(1024 * 1024, ErrorMessage = "Maximum allowed pic size is 1 MB.")]
        public IFormFile CoverImage { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Maximum allowed Pics is 3.")]
        [MaxFileSize(1024 * 1024, ErrorMessage = "Maximum allowed pic size is 1 MB.")]
        public ICollection<IFormFile> Pics { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
