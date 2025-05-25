using Application.DTos.Order;
using FluentValidation;

namespace ECommercePaymentApi.Validators
{
    public class OrderValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderValidator()
        {
            RuleFor(model => model.Items)
                .NotEmpty()
                .WithMessage("Sipariş en az 1 ürün içermelidir.");

            RuleForEach(model => model.Items).SetValidator(new OrderItemValidator());
        }
    }

    public class OrderItemValidator : AbstractValidator<OrderItemCreateDto>
    {
        public OrderItemValidator()
        {
            RuleFor(model => model.Quantity).GreaterThan(0).WithMessage("Sipariş satırındaki ürün miktarı 0'da büyük olmalıdır.");
            RuleFor(model => model.ProductCode).NotEmpty().WithMessage("Ürün kodu boş olamaz.");
        }
    }
}
