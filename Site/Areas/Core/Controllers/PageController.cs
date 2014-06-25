﻿using Sdl.Web.Mvc;
using Sdl.Web.Mvc.Common;

namespace Site.Areas.Core.Controllers
{
    public class PageController : BaseController
    {
        public PageController(IContentProvider contentProvider, IRenderer renderer)
        {
            ContentProvider = contentProvider;
            Renderer = renderer;
        }
    }
}
