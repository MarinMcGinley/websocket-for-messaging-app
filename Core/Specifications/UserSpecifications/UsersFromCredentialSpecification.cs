using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Auth;
using Core.Entities;

namespace Core.Specifications.UserSpecifications
{
    public class UsersFromCredentialSpecification : BaseSpecification<User>
    {
        public UsersFromCredentialSpecification(Credential credential): base(user => user.Email == credential.Email && user.Password == credential.Password) 
        {

        }
    }
}