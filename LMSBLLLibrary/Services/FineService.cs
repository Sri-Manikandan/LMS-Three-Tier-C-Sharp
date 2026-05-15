using LMSBLLLibrary.Interfaces;
using LMSDALLibrary.Interfaces;
using LMSDALLibrary.Repositories;
using LMSModelLibrary.Exceptions;
using LMSModelLibrary.Models;

namespace LMSBLLLibrary.Services
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepo;
        private readonly IFinePaymentRepository _paymentRepo;
        private readonly IMemberRepository _memberRepo;

        public FineService()
        {
            _fineRepo = new FineRepository();
            _paymentRepo = new FinePaymentRepository();
            _memberRepo = new MemberRepository();
        }

        public List<Fine> GetPendingFines(int memberId)
        {
            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);
            return _fineRepo.GetPendingFinesByMember(memberId);
        }

        public FinePayment PayFine(int fineId, decimal amount, int memberId)
        {
            if (amount <= 0)
                throw new InvalidInputException("Payment amount must be greater than zero.");

            var fine = _fineRepo.GetById(fineId);
            if (fine == null)
                throw new FineNotFoundException(fineId);

            if (fine.FinePaidStatus == FineStatusEnum.paid)
                throw new FineAlreadyPaidException(fineId);

            if (fine.FinePaidStatus == FineStatusEnum.waived)
                throw new FineWaivedException(fineId);

            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);

            var payment = new FinePayment
            {
                FineId = fineId,
                MemberId = memberId,
                FinePaidAmount = amount,
                PaidDate = DateTime.Now
            };

            _paymentRepo.Add(payment);

            fine.FinePaidStatus = FineStatusEnum.paid;
            _fineRepo.Update(fine);

            return payment;
        }

        public List<Fine> GetFineHistory(int memberId)
        {
            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);
            return _fineRepo.GetFineHistoryByMember(memberId);
        }

        public decimal GetTotalUnpaidFine(int memberId)
        {
            if (_memberRepo.GetById(memberId) == null)
                throw new MemberNotFoundException(memberId);
            return _fineRepo.GetTotalUnpaidFine(memberId);
        }
    }
}
