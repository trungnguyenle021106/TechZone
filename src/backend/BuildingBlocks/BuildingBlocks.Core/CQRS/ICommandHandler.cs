using MediatR;

namespace BuildingBlocks.Core.CQRS
{
    // 1. Handler cho Command có trả về kết quả (TResponse)
    // Bản chất nó kế thừa IRequestHandler của MediatR
    // Ràng buộc (where): TCommand phải là ICommand<TResponse>
    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }

    // 2. Handler cho Command không trả về kết quả (void/Unit)
    public interface ICommandHandler<in TCommand>
        : IRequestHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    {
    }
}