﻿using Microsoft.AspNetCore.Identity;

namespace CMSApplication.Data.Entity
{
    public class User : IdentityUser
    {

        public string FullName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
