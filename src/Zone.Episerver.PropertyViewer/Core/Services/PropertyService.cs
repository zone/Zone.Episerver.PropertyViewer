﻿using System;
using EPiServer;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;
using Zone.Episerver.PropertyViewer.Models;

namespace Zone.Episerver.PropertyViewer.Core.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;

        public PropertyService(IContentLoader contentLoader, IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
        }

        public IEnumerable<string> GetPropertyNames(int pageId)
        {
            var page = _contentLoader.Get<PageData>(new ContentReference(pageId));

            return GetProperties(page).Select(x => x.Name);
        }

        public IEnumerable<string> GetBlockPropertyNames(PropertyReference reference)
        {
            var property = GetProperty(reference);
            if (property.Type != PropertyDataType.Block)
            {
                throw new Exception($"Property '{reference.PropertyName}' is not a block");
            }

            return GetProperties((BlockData) property.Value).Select(x => x.Name);
        }

        public IEnumerable<PropertyValue> GetPropertyValues(PropertyReference reference)
        {
            var languageVersions = _contentRepository.GetLanguageBranches<PageData>(new ContentReference(reference.PageId));

            return languageVersions.Select(x => new PropertyValue
            {
                Language = x.Language.Name,
                ContentLink = x.ContentLink,
                Value = x.GetPropertyValue(reference.PropertyName)
            });
        }

        public IEnumerable<PropertyValue> GetBlockPropertyValues(LocalBlockPropertyReference reference)
        {
            var languageVersions = _contentRepository.GetLanguageBranches<PageData>(new ContentReference(reference.PageId));

            return languageVersions.Select(x => new PropertyValue
            {
                Language = x.Language.Name,
                ContentLink = x.ContentLink,
                Value = x.Property
                    .GetPropertyValue<BlockData>(reference.PropertyName)
                    .GetPropertyValue(reference.BlockPropertyName)
            });
        }

        public bool IsBlock(PropertyReference reference)
        {
            var property = GetProperty(reference);

            return property.Type == PropertyDataType.Block;
        }

        private PropertyData GetProperty(PropertyReference reference)
        {
            var page = _contentLoader.Get<PageData>(new ContentReference(reference.PageId));

            return page.Property.Get(reference.PropertyName);
        }

        private IEnumerable<PropertyData> GetProperties(IContentData content)
        {
            return content.Property
                .Where(x => x.IsPropertyData)
                .OrderBy(x => x.Name);
        }
    }
}