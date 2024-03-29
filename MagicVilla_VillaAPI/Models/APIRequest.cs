﻿using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.StaticDetails;

namespace MagicVilla_VillaAPI.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
