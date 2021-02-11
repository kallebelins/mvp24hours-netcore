using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mvp24Hours.Core.DTOs.Models
{
    [DataContract, Serializable]
    public class PagingCriteriaRequest
    {
        [DataMember]
        public int Limit { get; set; }
        [DataMember]
        public int Offset { get; set; }
        [DataMember]
        public List<string> OrderBy { get; set; }
        [DataMember]
        public List<string> Navigation { get; set; }
    }
}
