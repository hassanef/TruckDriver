using Microsoft.Azure.Cosmos;
using System.Net;

namespace TruckDriver.Test.TestDoubles
{
    public class MockFeedResponse<T> : FeedResponse<T>
    {
        private readonly IEnumerable<T> _data;

        public MockFeedResponse(IEnumerable<T> data)
        {
            _data = data;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override int Count => _data.Count();

        public override string ContinuationToken => throw new NotImplementedException();

        public override string IndexMetrics => throw new NotImplementedException();

        public override IEnumerable<T> Resource => throw new NotImplementedException();

        public override HttpStatusCode StatusCode => throw new NotImplementedException();

        public override CosmosDiagnostics Diagnostics => throw new NotImplementedException();

        public override Headers Headers => throw new NotImplementedException();
    }
}
