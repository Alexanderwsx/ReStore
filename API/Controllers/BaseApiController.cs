using Microsoft.AspNetCore.Mvc;


namespace API.Controllers // Définition de l'espace de noms. 
{
    // L'attribut [ApiController] indique que cette classe est destinée à être utilisée comme un contrôleur API.
    [ApiController]

    // L'attribut [Route] définit la route par défaut pour ce contrôleur. 
    // "api/[controller]" signifie que l'URL sera "api/" suivi du nom du contrôleur sans le mot "Controller".
    // Par exemple, pour un contrôleur nommé "ProductController", l'URL serait "api/Product".
    [Route("api/[controller]")]

    public class BaseApiController : ControllerBase // Déclaration de la classe. Elle hérite de "ControllerBase".
    {
        // Actuellement, cette classe est vide, mais elle peut être étendue à l'avenir pour inclure 
        // des fonctionnalités ou des propriétés communes à tous les contrôleurs qui en héritent.
    }
}

/*En utilisant une telle classe de base, vous pouvez centraliser certaines fonctionnalités ou configurations qui doivent être partagées par tous vos contrôleurs API. C'est une pratique courante dans les applications ASP.NET Core pour garder le code DRY (Don't Repeat Yourself) et faciliter la maintenance.
*/