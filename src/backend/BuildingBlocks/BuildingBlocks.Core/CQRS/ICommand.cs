// ICommand.cs - Dùng cho các hành động GHI (Create, Update, Delete)
using MediatR;

namespace BuildingBlocks.Core.CQRS
{
    // ICommand trả về Unit (void)
    public interface ICommand : ICommand<Unit>
    {
    }

    // ICommand trả về TResponse
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}