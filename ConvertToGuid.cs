// Copyright (c) 2024 Roger Brown.
// Licensed under the MIT License.

// from an idea by https://github.com/jborean93

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography;

namespace RhubarbGeekNz.Guido
{
    [Cmdlet(VerbsData.ConvertTo, "Guid")]
    [OutputType(typeof(Guid))]
    sealed public class ConvertToGuid : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public Guid Namespace;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 1)]
        public object Value;

        [Parameter()]
        [EncodingTransform]
        public System.Text.Encoding Encoding;

        protected override void ProcessRecord()
        {
            byte[] valueBytes;
            object baseObject = Value is PSObject psobj ? psobj.BaseObject : Value;

            if (baseObject is IEnumerable<byte> e)
            {
                valueBytes = e.ToArray();
            }
            else
            {
                if (Encoding == null)
                {
                    Encoding = System.Text.Encoding.UTF8;
                }

                valueBytes = Encoding.GetBytes(Value.ToString());
            }

            if (Encoding == null) Encoding = System.Text.Encoding.UTF8;
            byte[] namespaceBytes = Namespace.ToByteArray();
            Array.Reverse(namespaceBytes, 0, 4);
            Array.Reverse(namespaceBytes, 4, 2);
            Array.Reverse(namespaceBytes, 6, 2);
            byte[] buffer = new byte[namespaceBytes.Length + valueBytes.Length];
            Array.Copy(namespaceBytes, buffer, namespaceBytes.Length);
            Array.Copy(valueBytes, 0, buffer, namespaceBytes.Length, valueBytes.Length);
            byte[] hash = SHA1.Create().ComputeHash(buffer);
            byte[] guidBytes = new byte[16];
            Array.Copy(hash, guidBytes, guidBytes.Length);
            guidBytes[6] =(byte)((guidBytes[6] & 0xF) | 0x50);
            guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);
            Array.Reverse(guidBytes, 0, 4);
            Array.Reverse(guidBytes, 4, 2);
            Array.Reverse(guidBytes, 6, 2);
            WriteObject(new Guid(guidBytes));
        }
    }
}
