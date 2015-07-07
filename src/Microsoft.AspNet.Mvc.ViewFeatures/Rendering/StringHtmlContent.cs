// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.Mvc.Rendering
{
    /// <summary>
    /// <see cref="IHtmlContent"/> specific to strings.
    /// </summary>
    public class StringHtmlContent : IHtmlContent
    {
        /// <summary>
        /// Returns a <see cref="StringHtmlContent"/> with empty string.
        /// </summary>
        public static readonly StringHtmlContent Empty = new StringHtmlContent(string.Empty, encodeOnWrite: false);

        private bool _encodeOnWrite;
        private string _text;

        /// <summary>
        /// Creates a new instance of <see cref="StringHtmlContent"/> with the encoded string.
        /// </summary>
        /// <param name="text">Encoded string to initialize <see cref="StringHtmlContent"/>.</param>
        /// <returns>A new instance of <see cref="StringHtmlContent"/>.</returns>
        public static StringHtmlContent FromEncodedText(string text)
        {
            return new StringHtmlContent(text, encodeOnWrite: false);
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="StringHtmlContent"/> with the specified string
        /// which will be encoded when written to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="text">String to initialize <see cref="StringHtmlContent"/>.</param>
        public StringHtmlContent(string text)
            : this(text, encodeOnWrite: true)
        {
        }

        protected StringHtmlContent(string text, bool encodeOnWrite)
        {
            _text = text;
            _encodeOnWrite = encodeOnWrite;
        }

        /// <summary>
        /// Gets the unprocessed text.
        /// </summary>
        /// <returns>The text which is stored in <see cref="StringHtmlContent"/>.</returns>
        public string GetText()
        {
            return _text;
        }

        /// <summary>
        /// Encodes and writes the specified string to the <see cref="writer"/>. If the <see cref="StringHtmlContent"/>
        /// was created with <see cref="FromEncodedText(string)"/> then the text is written without being encoded.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> to which the output is written.</param>
        /// <param name="encoder"><see cref="IHtmlEncoder"/> which encodes the output.</param>
        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            if (_encodeOnWrite)
            {
                encoder.HtmlEncode(_text, writer);
            }
            else
            {
                writer.Write(_text);
            }
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer, new HtmlEncoder());
                return writer.ToString();
            }
        }
    }
}
