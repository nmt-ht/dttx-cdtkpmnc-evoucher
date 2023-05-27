﻿using eVoucher.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVourcher.Services
{
    public interface ICampaignService
    {
        Task<IList<Campaign>> GetCampaigns();
        Task<Campaign> GetCampaignById(Guid id);
        Task<bool> CreateCampaign(Campaign campaign);
        Task<bool> UpdateCampaign(Campaign campaign);
        Task<bool> DeleteCampaign(Guid id);
        Task<bool> DeleteGame(Guid id);
        Task<bool> UpdateGame(Game game);

    }

   
}
