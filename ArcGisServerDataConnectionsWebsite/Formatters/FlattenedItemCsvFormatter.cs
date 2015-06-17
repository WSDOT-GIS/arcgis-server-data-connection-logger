using CsvHelper;
using DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ArcGisServerDataConnectionsWebsite.Formatters
{
    public class FlattenedItemCsvFormatter: MediaTypeFormatter
    {
        public FlattenedItemCsvFormatter(): base()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<FlattenedItem>).IsAssignableFrom(type);
        }

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

                    ////using (var textWriter = new StreamWriter(writeStream, effectiveEncoding))
                    ////using (var csvWriter = new CsvWriter(textWriter))
                    ////{
                    ////    csvWriter.WriteRecords(items);
                    ////}

                });

            }
            else
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }
    }
}