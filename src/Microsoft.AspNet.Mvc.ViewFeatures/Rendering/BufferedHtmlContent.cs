// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.Framework.Internal;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.Mvc.Rendering
{
    /// <summary>
    /// Enumerable object collection which knows how to write itself.
    /// </summary>
    public class BufferedHtmlContent : IHtmlContent, IEnumerable<IHtmlContent>
    {
        private const int MaxCharToStringLength = 1024;
        private List<IHtmlContent> _entries = new List<IHtmlContent>();

        /// <summary>
        /// Creates a new <see cref="StringHtmlContent"/> from the specified string and appends it to the collection.
        /// </summary>
        /// <param name="value">The <c>string</c> to be appended.</param>
        public void Append(string value)
        {
            if (value != null)
            {
                _entries.Add(StringHtmlContent.FromEncodedText(value));
            }
        }

        public void Append([NotNull] char[] value, int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0 || value.Length - index < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            while (count > 0)
            {
                // Split large char arrays into 1KB strings.
                var currentCount = count;
                if (MaxCharToStringLength < currentCount)
                {
                    currentCount = MaxCharToStringLength;
                }

                Append(new string(value, index, currentCount));
                index += currentCount;
                count -= currentCount;
            }
        }

        /// <summary>
        /// Appends a <see cref="IHtmlContent"/> to the collection.
        /// </summary>
        /// <param name="htmlContent">The <see cref="IHtmlContent"/> to be appended.</param>
        public void Append(IHtmlContent htmlContent)
        {
            var bufferedHtmlContext = htmlContent as BufferedHtmlContent;
            if (bufferedHtmlContext != null)
            {
                _entries.AddRange(bufferedHtmlContext._entries);
            }
            else if (htmlContent != null)
            {
                _entries.Add(htmlContent);
            }
        }

        /// <summary>
        /// Creates a new <see cref="StringHtmlContent"/> from the encoded string and appends it to the collection.
        /// </summary>
        /// <param name="encodedValue">The encoded <c>string</c> to be appended.</param>
        public void AppendEncoded(string encodedValue)
        {
            if (encodedValue != null)
            {
                _entries.Add(StringHtmlContent.FromEncodedText(encodedValue));
            }
        }

        /// <summary>
        /// Removes all the entries from the collection.
        /// </summary>
        public void Clear()
        {
            _entries.Clear();
        }

        // <inheritdoc />
        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            foreach (var entry in _entries)
            {
                entry.WriteTo(writer, encoder);
            }
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                foreach (var item in _entries)
                {
                    item.WriteTo(writer, new HtmlEncoder());
                }

                return writer.ToString();
            }
        }

        // <inheritdoc />
        public IEnumerator<IHtmlContent> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        // <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
