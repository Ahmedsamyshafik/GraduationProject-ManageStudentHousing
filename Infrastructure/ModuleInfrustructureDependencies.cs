﻿using Domin.Models;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Infrastructure.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace infrustructure;

public static class ModuleInfrustructureDependencies
{
    public static IServiceCollection AddInfrustructureDependencies(this IServiceCollection services)
    {
        services.AddTransient<IApartmentRepository, ApartmentRepository>();
        services.AddTransient<IimagesRepository, imagesRepository>();
        services.AddTransient<IVideoRepository, VideoRepository>();
        services.AddTransient<IRoyalDocumentRepository, RoyalDocumentRepository>();
        services.AddTransient<IViewRepository, ViewRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IReactRepository, ReactRepository>();
        services.AddTransient<IUserApartmentsRequestsRepository, UserApartmentsRequestsRepository>();
        services.AddTransient<IUsersApartmentsRepository, UsersApartmentsRepository>();


        services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        return services;
    }

}
