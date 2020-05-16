using MedPark.Common;
using MedPark.Common.Handlers;
using MedPark.Common.RabbitMq;
using MedPark.CustomersService.Domain;
using MedPark.CustomersService.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Handlers.Customers
{
    public class AddCustomerMedicalSchemeHandler : ICommandHandler<AddCustomerMedicalScheme>
    {
        private IMedParkRepository<Customer> _customerRepo { get; }
        private IMedParkRepository<MedicalScheme> _schemesRepo { get; }
        private IMedParkRepository<CustomerMedicalScheme> _customerSchemesRepo { get; }

        public AddCustomerMedicalSchemeHandler(IMedParkRepository<CustomerMedicalScheme> customerSchemesRepo, IMedParkRepository<Customer> customerRepo, IMedParkRepository<MedicalScheme> schemesRepo)
        {
            _customerRepo = customerRepo;
            _schemesRepo = schemesRepo;
            _customerSchemesRepo = customerSchemesRepo;
        }

        public async Task HandleAsync(AddCustomerMedicalScheme command, ICorrelationContext context)
        {
            var customer = await _customerRepo.GetAsync(command.CustomerId);

            if (customer == null)
                throw new Exception();

            var scheme = await _schemesRepo.GetAsync(command.MedicalSchemeId);

            if (scheme == null)
                throw new Exception();

            var customerScheme = await _customerSchemesRepo.FindAsync(x => x.SchemeId == command.MedicalSchemeId && x.CustomerId == command.CustomerId);

            if (customerScheme.Count() > 0)
                throw new Exception();

            CustomerMedicalScheme newSchemeLink = new CustomerMedicalScheme(Guid.NewGuid());
            newSchemeLink.SetCustomer(command.CustomerId);
            newSchemeLink.SetScheme(command.MedicalSchemeId);
            newSchemeLink.SetCustomerSchemeMembershipNo(command.MembershipNo);

            await _customerSchemesRepo.AddAsync(newSchemeLink);
        }
    }
}
