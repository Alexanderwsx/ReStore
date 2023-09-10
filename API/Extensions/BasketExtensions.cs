using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;
/*
    Ce code définit une classe d'extension BasketExtensions. Elle contient deux méthodes d'extension qui facilitent certaines opérations liées aux objets Basket et BasketDto:
*/
namespace API.Extensions
{
    public static class BasketExtensions // Déclaration de la classe d'extension. Elle doit être `static`.
    {
        // Méthode d'extension pour convertir un objet `Basket` en `BasketDto`.
        public static BasketDto MapBasketToDto(this Basket basket)
        {
            return new BasketDto // Création d'une nouvelle instance de `BasketDto`.
            {
                Id = basket.Id, // Copie de l'ID.
                BuyerId = basket.BuyerId, // Copie de l'ID de l'acheteur.
                Items = basket.Items.Select(item => new BasketItemDto // Transformation de chaque `BasketItem` en `BasketItemDto`.
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Brand = item.Product.Brand,
                    Type = item.Product.Type,
                    Quantity = item.Quantity
                }).ToList() // Convertit le résultat en liste.
            };
        }

        // Méthode d'extension pour récupérer un panier avec ses articles associés à partir d'une requête IQueryable<Basket>.
        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
        {
            return query
            .Include(x => x.Items) // Inclut les articles du panier dans la requête.
            .ThenInclude(p => p.Product) // Ensuite, inclut les produits associés à chaque article.
            .Where(b => b.BuyerId == buyerId); // Filtre le panier par ID d'acheteur.
        }
    }
}

/*
    La première méthode, MapBasketToDto, facilite la conversion d'un objet Basket en objet BasketDto. Elle est utile lorsque vous souhaitez transformer une entité de la base de données en un objet de transfert de données pour l'envoyer à un client ou pour une autre logique métier.

La deuxième méthode, RetrieveBasketWithItems, est utile lorsque vous souhaitez récupérer un panier et ses articles associés à partir de la base de données. La méthode Include d'Entity Framework permet de charger des relations liées, et ThenInclude permet d'enchaîner et de charger les relations des relations.
*/