using MedPark.Common;
using MedPark.Common.Handlers;
using MedPark.Common.RabbitMq;
using MedPark.Common.Types;
using MedPark.CustomersService.Domain;
using MedPark.CustomersService.Messages.Commands;
using MedPark.CustomersService.Messages.Events;
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

        private IBusPublisher _busPublisher;

        public AddCustomerMedicalSchemeHandler(IMedParkRepository<CustomerMedicalScheme> customerSchemesRepo, IMedParkRepository<Customer> customerRepo, IMedParkRepository<MedicalScheme> schemesRepo, IBusPublisher busPublisher)
        {
            _customerRepo = customerRepo;
            _schemesRepo = schemesRepo;
            _customerSchemesRepo = customerSchemesRepo;

            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(AddCustomerMedicalScheme command, ICorrelationContext context)
        {
            var customer = await _customerRepo.GetAsync(command.CustomerId);

            if (customer == null)
                throw new MedParkException("customer_does_not_Exist", "Customer does not exist.");

            var scheme = await _schemesRepo.GetAsync(command.MedicalSchemeId);

            if (scheme == null)
                throw new MedParkException("medical_scheme_does_not_Exist", "The medical scheme to link to the customer does not exist.");

            var customerScheme = await _customerSchemesRepo.FindAsync(x => x.SchemeId == command.MedicalSchemeId && x.CustomerId == command.CustomerId);

            if (customerScheme.Count() > 0)
                throw new MedParkException("already_linked", $"The customer {command.CustomerId} has already been linked to medical scheme {command.MedicalSchemeId}");

            CustomerMedicalScheme newSchemeLink = new CustomerMedicalScheme(Guid.NewGuid());

            newSchemeLink.SetCustomer(command.CustomerId);
            newSchemeLink.SetScheme(command.MedicalSchemeId);
            newSchemeLink.SetCustomerSchemeMembershipNo(command.MembershipNo);

            //Save
            await _customerSchemesRepo.AddAsync(newSchemeLink);

            //Publish event that medical scheme was linked to customer
            await _busPublisher.PublishAsync(new CustomerMedicalSchemeAdded(command.CustomerId, command.MedicalSchemeId), null);
        }
    }
}
