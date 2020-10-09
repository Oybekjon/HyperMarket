﻿using System;
using System.Text;

namespace HyperMarket {
    public class PostValueParam : IMultipartParameter {
        public String ParamName { get; private set; }
        public String Value { get; private set; }
        public PostValueParam(String paramName, String value) {
            Guard.NotNullOrEmpty(paramName, "paramName");
            Guard.NotNullOrEmpty(value, "value");
            ParamName = paramName;
            Value = value;
        }
        Byte[] IMultipartParameter.Value {
            get { return Value.Return(x => Encoding.UTF8.GetBytes(Value), null); }
        }
        String IMultipartParameter.FileName {
            get { return null; }
        }
        String IMultipartParameter.ContentType {
            get { return null; }
        }
    }
}