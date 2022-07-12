﻿using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}
