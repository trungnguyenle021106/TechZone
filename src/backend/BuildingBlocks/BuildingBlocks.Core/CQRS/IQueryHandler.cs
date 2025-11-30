using MediatR;

namespace BuildingBlocks.Core.CQRS
{
    // Handler cho Query luôn luôn phải có trả về kết quả (TResponse)
    // Ràng buộc: TQuery phải là IQuery<TResponse>
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}