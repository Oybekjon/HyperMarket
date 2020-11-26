using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public static class MockHelper
    {
        public static Mock<T> GetMock<T>() where T : class
        {
            return MockContainer<T>.Instance.MockedObject;
        }
    }
}
