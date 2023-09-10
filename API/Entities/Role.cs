using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
//la classe sert à ajouter des roles à l'identité de l'utilisateur avec un id de type int
namespace API.Entities
{
    public class Role : IdentityRole<int>
    {

    }
}