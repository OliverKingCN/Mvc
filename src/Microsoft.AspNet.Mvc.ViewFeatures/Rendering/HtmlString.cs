// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.Framework.Internal;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.Mvc.Rendering
{
    public class HtmlString : StringHtmlContent
    {
        public HtmlString(string input)
            : base(input, encodeOnWrite: false)
        {
        }

        public static new HtmlString Empty => new HtmlString(string.Empty);
    }
}
