﻿using System.Collections.Generic;
using Sdl.Web.Common.Configuration;
using Sdl.Web.Common.Interfaces;
using Sdl.Web.Common.Models;

namespace Sdl.Web.Tridion.Tests.Models
{
    [SemanticEntity("TSI1758")]
    public class Tsi1758TestEntity : EntityModel
    {
        public List<Tsi1758TestEmbeddedEntity> EmbedField1 { get; set; }
        public List<Tsi1758TestEmbeddedEntity> EmbedField2 { get; set; }
    }

    [SemanticEntity("TSI1758Embed")]
    public class Tsi1758TestEmbeddedEntity : EntityModel
    {
        public string TextField { get; set; }
        public Link EmbedField1 { get; set; }
        public Tsi1758TestEmbedded2Entity EmbedField2 { get; set; }

        public override MvcData GetDefaultView(ILocalization localization)
        {
            return new MvcData("Test:TSI1758TestEmbedded");
        }
    }

    [SemanticEntity("TSI1758Embed2")]
    public class Tsi1758TestEmbedded2Entity : EntityModel
    {
        public string TextField { get; set; }
        public Link EmbedField2 { get; set; }

        public override MvcData GetDefaultView(ILocalization localization)
        {
            return new MvcData("Test:TSI1758TestEmbedded2");
        }
    }
}
