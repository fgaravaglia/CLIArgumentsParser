﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser.ErrorManagement
{
    [Serializable]
    public class UnknownVerbException : CliArgumentParserException
    {
        public string Verb { get; private set; }

        public UnknownVerbException() : base()
        {
            this.Verb = "";
        }

        public UnknownVerbException(string verb) : base($"Wrong Usage: Unkown verb {verb}")
        {
            this.Verb = verb;
        }
        protected UnknownVerbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Verb = "";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);

            info.AddValue("Verb", Verb);
        }

    }
}
