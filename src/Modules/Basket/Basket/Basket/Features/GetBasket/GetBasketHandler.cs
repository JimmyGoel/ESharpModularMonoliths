
namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketQuery(string UserName)
        : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);
    public class GetBasketHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            //get basket from repository
            var basket = await basketRepository.GetBasket(query.UserName, cancellationToken: cancellationToken);
            //map basket to dto
            var basketDto = basket.Adapt<ShoppingCartDto>();

            return new GetBasketResult(basketDto);

        }
    }
}
