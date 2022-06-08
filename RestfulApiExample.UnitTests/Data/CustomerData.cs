using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.UnitTests.Data
{
    public static class CustomerData
    {
        public static IEnumerable<TestCaseData> GetCreateFailedRequests()
        {
            yield return new TestCaseData(null);
            yield return new TestCaseData(new Entities.DTO.CreateCustomerRequest() { });
            yield return new TestCaseData(new Entities.DTO.CreateCustomerRequest() { Name = ""});
        }

        public static IEnumerable<TestCaseData> GetCreateSuccessfulRequests()
        {
            yield return new TestCaseData(new Entities.DTO.CreateCustomerRequest() { Name = "Javier Vazquez" });
        }

        public static IEnumerable<TestCaseData> GetUpdateFailedRequests()
        {
            yield return new TestCaseData(new Guid("00000000-0000-0000-0000-000000000000"), null);
            yield return new TestCaseData(new Guid("9cd67638-993a-48b1-aa27-6619d722472a"), null);
            yield return new TestCaseData(new Guid("9cd67638-993a-48b1-aa27-6619d722472a"), new Entities.DTO.UpdateCustomerRequest() { Id = new Guid("00000000-0000-0000-0000-000000000000") });
            yield return new TestCaseData(new Guid("9cd67638-993a-48b1-aa27-6619d722472a"), new Entities.DTO.UpdateCustomerRequest() { Id = new Guid("9cd67638-993a-48b1-aa27-6619d722472a") });
            yield return new TestCaseData(new Guid("9cd67638-993a-48b1-aa27-6619d722472a"), new Entities.DTO.UpdateCustomerRequest() { Id = new Guid("9cd67638-993a-48b1-aa27-6619d722472a"), Name = "" });
        }
    }
}
