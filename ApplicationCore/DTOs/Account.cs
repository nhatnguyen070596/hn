using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DTOs
{
    public class Account
    {
        public class LoginModel
        {
            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }



        }
        public class CreateIdentityUserModel
        {
            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string UserName { get; set; }

            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }
        public class UpdateIdentityUserRequest : LoginModel
        {
            [Required]
            [Range(0, 100)]
            public string Description { get; set; }
        }

        public class IdentityUserResponse
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string IsActive { get; set; }
            public int Description { get; set; }
        }
    }
}

