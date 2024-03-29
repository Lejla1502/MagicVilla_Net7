﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private string villaUrl;
        public VillaService(IHttpClientFactory httpClient, IConfiguration config) : base(httpClient)
        {
            //extracting api url from appsettings.json
            villaUrl = config.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaCreateDto createDto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = createDto,
                Url = villaUrl + "/api/v1/Villa",
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = villaUrl + "/api/v1/Villa/" + id,
                Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/v1/Villa",
                Token = token
            });
        }

        public async Task<T> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = villaUrl + "/api/v1/Villa/" + id,
                Token = token
            });
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDto updateDto, string token)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = updateDto,
                Url = villaUrl + "/api/v1/Villa/" + updateDto.Id,
                Token = token
            });
        }
    }
}
