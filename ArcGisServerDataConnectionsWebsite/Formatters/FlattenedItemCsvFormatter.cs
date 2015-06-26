using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Wsdot.ArcGis.Server.Reporting.DataContracts;

namespace Wsdot.ArcGis.Server.Reporting.Formatters
{
    /// <summary>
    /// Writes <see cref="FlattenedItem"/> <see cref="IEnumerable&lt;T&gt;"/> as CSV.
    /// </summary>
    public class FlattenedItemCsvFormatter: MediaTypeFormatter
    {
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public FlattenedItemCsvFormatter(): base()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
        }

        /// <summary>
        /// Determines of a type can be read by this converter.
        /// </summary>
        /// <param name="type">The type to be read.</param>
        /// <returns><see langword="false"/></returns>
        public override bool CanReadType(Type type)
        {
            return false;
        }

        /// <summary>
        /// Determines if this class can write a type as CSV.
        /// </summary>
        /// <param name="type">The type to be written.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="type"/> can be written, <see langword="false"/> otherwise.</returns>
        public override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<FlattenedItem>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Writes an enumeration of <see cref="FlattenedItem"/> objects to a <see cref="Stream"/> as CSV.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="writeStream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <returns></returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

            var items = value as IEnumerable<FlattenedItem>;
            if (items != null)
            {
                return Task.Run(() =>
                {
                    var textWriter = new StreamWriter(writeStream, effectiveEncoding);
                    var csvWriter = new CsvWriter(textWriter);
                    csvWriter.WriteRecords(items);
                    textWriter.FlushAsync().Wait();
                });

            }
            else
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }
    }
}