﻿using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketAPI.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            try
            {
                var discountRequest = new GetDiscountRequest { ProductName = productName };
                return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
            }
            catch(Exception ex)
            {
                return null;
            }
        }  
    }
}
