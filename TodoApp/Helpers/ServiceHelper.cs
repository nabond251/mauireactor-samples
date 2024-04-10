﻿using System;
using System.Threading.Tasks;

namespace TodoApp.Helpers;

public static class ServiceHelper
{
    private static readonly TaskCompletionSource<IServiceProvider> serviceProviderTcs = new();

    public static Task<IServiceProvider> ServicesAsync => serviceProviderTcs.Task;

    public static void Initialize(IServiceProvider serviceProvider) =>
        serviceProviderTcs.TrySetResult(serviceProvider);
}
