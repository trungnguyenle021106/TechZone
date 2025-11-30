using BuildingBlocks.Core.CQRS;
using FluentValidation;

namespace Catalog.Application.Products.Commands.CreateProduct
{
    // 1. Định nghĩa Output (Trả về cái gì?)
    // Trả về ID của sản phẩm vừa tạo
    public record CreateProductResult(Guid Id);

    // 2. Định nghĩa Command (Input là gì?)
    // Kế thừa ICommand<CreateProductResult> từ BuildingBlocks
    public record CreateProductCommand(string Name, string Description, decimal Price)
        : ICommand<CreateProductResult>;

    // 3. Validator (Kiểm tra dữ liệu ngay tại đầu vào)
    // The "Why": Fail-fast. Nếu dữ liệu sai, chặn ngay, không cho xuống Database.
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Giá phải lớn hơn 0");
        }
    }
}