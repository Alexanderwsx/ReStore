using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
/*
    La classe AccountController est un contrôleur API pour gérer les opérations liées aux comptes utilisateur, telles que la connexion, l'inscription et la récupération de l'utilisateur actuellement connecté. Elle hérite de BaseApiController pour avoir accès à certaines fonctionnalités de base.
Explication ligne par ligne:
Déclarations:

    Les champs privés _userManager, _tokenService et _context sont utilisés pour interagir avec le système d'authentification, pour la génération de tokens et pour l'accès à la base de données, respectivement.
    Le constructeur initialise ces trois champs à partir des instances fournies par le système d'injection de dépendances.

Méthodes:

    Login:
        Cette méthode permet à un utilisateur de se connecter.
        Elle vérifie d'abord si l'utilisateur existe et si le mot de passe fourni est correct.
        Si l'utilisateur est valide, la méthode vérifie s'il a un panier associé à son compte et tente également de récupérer un panier anonyme (probablement créé avant que l'utilisateur ne se connecte).
        Si un panier anonyme est trouvé, il est associé à l'utilisateur et l'ancien panier de l'utilisateur est supprimé.
        Enfin, un UserDto contenant les détails de l'utilisateur, le token et le panier est renvoyé.

    Register:
        Cette méthode permet à un utilisateur de s'inscrire.
        Elle crée un nouvel objet utilisateur et tente de l'enregistrer à l'aide du gestionnaire d'utilisateurs.
        Si l'enregistrement échoue (par exemple, en raison de règles de validation du mot de passe), les erreurs sont renvoyées.
        Si l'enregistrement réussit, l'utilisateur est ajouté au rôle "Member".

    GetCurrentUser:
        Cette méthode renvoie les détails de l'utilisateur actuellement connecté.
        Elle récupère l'utilisateur à partir du gestionnaire d'utilisateurs en utilisant le nom d'utilisateur de l'utilisateur actuellement connecté.
        Elle renvoie également le panier associé à l'utilisateur.

    RetrieveBasket:
        Il s'agit d'une méthode privée utilisée pour récupérer un panier à partir de la base de données en fonction de l'ID de l'acheteur.
        Si aucun buyerId n'est fourni, cela signifie qu'il n'y a pas de cookie de panier et la méthode renvoie null.
        Sinon, elle tente de récupérer le panier avec tous ses articles et produits associés.
*/
namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly StoreContext _context;

        public AccountController(UserManager<User> userManager, TokenService tokenService,
            StoreContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized();

            var userBasket = await RetrieveBasket(loginDto.Username);
            var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);

            if (anonBasket != null)
            {
                if (userBasket != null) _context.Baskets.Remove(userBasket);
                anonBasket.BuyerId = user.UserName;
                Response.Cookies.Delete("buyerId");
                await _context.SaveChangesAsync();
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = anonBasket != null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto()
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userBasket = await RetrieveBasket(User.Identity.Name);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                Basket = userBasket?.MapBasketToDto()
            };
        }

        [Authorize]
        [HttpGet("savedAddress")]
        public async Task<ActionResult<UserAddress>> GetSavedAddress()
        {
            return await _userManager.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(user => user.Address)
                .FirstOrDefaultAsync();
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(basket => basket.BuyerId == buyerId);
        }
    }
}