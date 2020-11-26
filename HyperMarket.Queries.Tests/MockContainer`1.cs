using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public class MockContainer<T> where T : class
    {
        private static readonly Lazy<MockContainer<T>> _LazyInstance =
            new Lazy<MockContainer<T>>(() => new MockContainer<T>());
        public static MockContainer<T> Instance => _LazyInstance.Value;

        private Mock<T> _MockedObject;
        public Mock<T> MockedObject
        {
            get
            {
                if (_MockedObject == null)
                {
                    _MockedObject = new Mock<T>();
                }
                return _MockedObject;
            }
        }

        private MockContainer()
        {

        }
    }
}
