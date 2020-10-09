using Newtonsoft.Json;
using System;

namespace HyperMarket.Queries.ViewModels {
    public class ErrorViewModel : ResultViewModel {
        public ErrorViewModel() : base(false) { }
        public String Error { get; set; }
        public String StackTrace { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Object Data { get; set; }
    }
}