﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Web.ViewModels.Positions;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data
{
    public class PositionService : IPositionsService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public PositionService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public async Task CreateAsync(CreatePositionInputModel inputModel)
        {
            Position position = this.mapper.Map<Position>(inputModel);
            await this.context.Positions.AddAsync(position);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PositionsAllViewModel>> GetAllAsync()
        {
            return await this.context.Positions
                .ProjectTo<PositionsAllViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}