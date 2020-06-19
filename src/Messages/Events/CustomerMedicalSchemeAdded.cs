using MedPark.Common.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPark.CustomersService.Messages.Events
{
    public class CustomerMedicalSchemeAdded : IEvent
    {
        public Guid CustomerId { get; }
        public Guid MedicalSchemeId { get; }

        [JsonConstructor]
        public CustomerMedicalSchemeAdded(Guid customerId, Guid medicalSchemeId)
        {
            CustomerId = customerId;
            MedicalSchemeId = medicalSchemeId;
        }
    }
}
