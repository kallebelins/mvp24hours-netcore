//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
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
