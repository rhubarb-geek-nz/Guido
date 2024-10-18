// Copyright (c) 2024 Roger Brown.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Text;

namespace RhubarbGeekNz.Guido
{
    sealed public class EncodingTransformAttribute : ArgumentTransformationAttribute
    {
        static readonly IDictionary<string, Func<Encoding>> map = new Dictionary<string, Func<Encoding>>()
        {
            {"ascii", ()=> Encoding.ASCII },
            {"ansi", ()=> Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage) },
            {"bigendianunicode ", ()=> Encoding.BigEndianUnicode },
            {"bigendianutf32", ()=> new UTF32Encoding(true,true) },
            {"oem", ()=> Encoding.GetEncoding(1)  },
            {"unicode", ()=> Encoding.Unicode },
            {"utf32", ()=> Encoding.UTF32 },
            {"utf8", ()=> new UTF8Encoding(false) },
            {"utf8bom", ()=> new UTF8Encoding(true) },
            {"utf8nobom", ()=> new UTF8Encoding(false) }
        };

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            Encoding result;

            if (inputData is Encoding enc)
            {
                result = enc;
            }
            else
            {
                if (inputData is string name)
                {
                    if (map.TryGetValue(name.ToLower(), out var mapValue))
                    {
                        result = mapValue();
                    }
                    else
                    {
                        result = Encoding.GetEncoding(name);
                    }
                }
                else
                {
                    result = Encoding.GetEncoding((int)inputData);
                }
            }

            return result;
        }
    }
}
