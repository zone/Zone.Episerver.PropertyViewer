﻿using System.Diagnostics.CodeAnalysis;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Zone.Episerver.PropertyViewer.Core.Services;

namespace Zone.Episerver.PropertyViewer.Core.Initialization
{
    [InitializableModule]
    [ExcludeFromCodeCoverage]
    public class DependencyInitialization : IConfigurableModule 
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IPropertyService, PropertyService>();
            context.Services.AddTransient<IContentTreeService, ContentTreeService>();
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
