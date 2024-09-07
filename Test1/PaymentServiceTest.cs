
using Application.Abstraction;
using Domain.Abstractions;
using Domain.Entities;
using Moq;

namespace Test1
{
    public class PaymentServiceTest
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly PaymentService _service;

        public PaymentServiceTest()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _service = new PaymentService(_paymentRepositoryMock.Object );
        }
        [Fact]
        public async Task CreatePayment_AddsPayment()
        {
            var amount = 100m;
            var currency = "USD";
            var payment = new Payment
            {
                Amount = amount,
                Currency = currency,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            _paymentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            var result = await _service.CreatePayment(amount, currency);
            Assert.NotNull(result);
            Assert.Equal(amount, result.Amount);
            Assert.Equal(currency, result.Currency);
            Assert.Equal("Pending", result.Status);
            _paymentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Payment>()), Times.Once);
        }
        [Fact]
        public async Task ProcessPayment_ReturnsFalse_WhenPaymentDoesNotExist()
        {
            var paymentId = Guid.NewGuid();
            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync((Payment)null);

            var result = await _service.ProcessPayment(paymentId);
            Assert.False(result);
        }

        [Fact]
        public async Task ProcessPayment_UpdatesPaymentStatusSuccessfully() 
        { 
            var paymentId = Guid.NewGuid();
            var payment = new Payment
            {
                PaymentId = paymentId,
                Amount = 100m,
                Currency = "USD",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync(payment);
            _paymentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            var result = await _service.ProcessPayment(paymentId);

            Assert.True(result);
            Assert.Equal("Succeeded", payment.Status);
            _paymentRepositoryMock.Verify(repo => repo.UpdateAsync(payment), Times.Once);
        }
        public async Task GetPaymentById_ThrowsException_WhenIdNotFound()
        {
            var paymentId = Guid.NewGuid();
            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync((Payment)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetPaymentById(paymentId));
        }

    }
}
