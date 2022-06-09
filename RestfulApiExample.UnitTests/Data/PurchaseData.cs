using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Data
{
    public static class PurchaseData
    {
        public static IEnumerable<TestCaseData> GetCreateFailedRequests()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new Entities.DTO.CreatePurchaseRequest() { });
            yield return new TestCaseData(new Entities.DTO.CreatePurchaseRequest() { CustomerId = Guid.Empty });
        }

        public static IEnumerable<TestCaseData> GetCreateSuccesfulRequests()
        {
            yield return new TestCaseData(new Entities.DTO.CreatePurchaseRequest() { CustomerId = Guid.NewGuid() });
        }
    }
}
